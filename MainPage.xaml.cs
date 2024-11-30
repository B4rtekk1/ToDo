using Microsoft.UI.Windowing;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Windows.AppNotifications;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ToDoPc
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string filePath = "tasks.json";
        public static List<TaskItem> tasks = new List<TaskItem>();
        //private DispatcherTimer timer;


        public MainPage()
        {
            this.InitializeComponent();
            LoadTasks();
            RefreshTasks();
        }
        


        private void TaskListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedTask = (TaskItem)TaskListView.SelectedItem;
            if (selectedTask != null)
            {
                selectedTask.IsCompleted = !selectedTask.IsCompleted;
                SaveTasks();
                RefreshTasks();
            }
        }


        private void RefreshTasks()
        {
            TaskListView.ItemsSource = null;
            TaskListView.ItemsSource = tasks;
        }

        public static bool SendNotificationToast(string title, string message)
        {
            var xmlPayload = new string($@"
        <toast>    
            <visual>    
                <binding template=""ToastGeneric"">    
                    <text>{title}</text>
                    <text>{message}</text>    
                </binding>
            </visual>  
        </toast>");

            var toast = new AppNotification(xmlPayload);
            AppNotificationManager.Default.Show(toast);
            return toast.Id != 0;
        }

        private async void SaveTasks()
        {
            string jsonString = JsonConvert.SerializeObject(tasks, Newtonsoft.Json.Formatting.Indented);

            try
            {
                var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(filePath, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(file, jsonString);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"B��d zapisu do pliku: {ex.Message}");
            }
        }



        private async void LoadTasks()
        {
            try
            {
                var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(filePath, CreationCollisionOption.OpenIfExists);
                var jsonString = await FileIO.ReadTextAsync(file);

                if (!string.IsNullOrWhiteSpace(jsonString))
                {
                    tasks = JsonConvert.DeserializeObject<List<TaskItem>>(jsonString);

                    foreach (var task in tasks)
                    {
                        task.Task = task.Task.ToUpper();
                    }

                }
                else
                {
                    TaskItem task = new TaskItem()
                    {
                        IsCompleted = false,
                        Description = "Task",
                        Task = "Task",
                        DueDate = DateTime.Now,
                        Category = "Task"
                    };
                    tasks.Add(task);
                }
                RefreshTasks();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"B��d podczas �adowania pliku: {ex.Message}");
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = (CheckBox)sender;
            var tastItem = (TaskItem)checkBox.DataContext;
            tastItem.IsCompleted = true;
            var grid = (Grid)checkBox.Parent;
            var textblock = (TextBlock)grid.Children[1];
            //grid.Background = new SolidColorBrush(Microsoft.UI.Colors.LightGreen);
            textblock.TextDecorations = Windows.UI.Text.TextDecorations.Strikethrough;
            SaveTasks();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var checkbox = (CheckBox)sender;
            var taskItem = (TaskItem)checkbox.DataContext;
            taskItem.IsCompleted = false;
            var grid = (Grid)checkbox.Parent;
            var textblock = (TextBlock)grid.Children[1];
            textblock.TextDecorations = Windows.UI.Text.TextDecorations.Strikethrough;
            //grid.Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
            SaveTasks();
        }

        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            var taskItem = (TaskItem)((Button)sender).DataContext;
            EditTask(taskItem);
        }
        private void EditTask(TaskItem tastitem)
        {
            TaskItem taskItem = tastitem;
            Frame.Navigate(typeof(TaskPage));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var taskItem = new TaskItem()
            {
                IsCompleted = false,
                Task = "Task",
                Description = "Task",
                DueDate = DateTime.Now,
                Category = "Task"
            };

            tasks.Add(taskItem);

            EditTask(taskItem);
        }
    }
}