using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.Windows.AppNotifications;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Windows.Storage;
using Newtonsoft.Json;
using WinUIEx;
using Microsoft.UI;
using Windows.ApplicationModel;
using Microsoft.UI.Windowing;
using Windows.UI.WindowManagement;
using Windows.UI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ToDoPc
{
    public sealed partial class MainWindow : Window
    {
        private string filePath = "tasks.json";
        private List<TaskItem> tasks = new List<TaskItem>();
        private DispatcherTimer timer;


        public MainWindow()
        {
            this.InitializeComponent();
            this.InitializeTimer();
            WindowSetup();
            LoadTasks();
            RefreshTasks();
            Microsoft.UI.Windowing.AppWindow m_AppWindow = this.AppWindow;
            SetTitleBarColors(m_AppWindow);
        }
        private bool SetTitleBarColors(Microsoft.UI.Windowing.AppWindow m_AppWindow)
        {
            // Check to see if customization is supported.
            // The method returns true on Windows 10 since Windows App SDK 1.2,
            // and on all versions of Windows App SDK on Windows 11.
            if (Microsoft.UI.Windowing.AppWindowTitleBar.IsCustomizationSupported())
            {
                Microsoft.UI.Windowing.AppWindowTitleBar m_TitleBar = m_AppWindow.TitleBar;

                // Set active window colors.
                // Note: No effect when app is running on Windows 10
                // because color customization is not supported.
                m_TitleBar.ForegroundColor = Colors.White;
                m_TitleBar.BackgroundColor = Color.FromArgb(80, 113, 40, 224);
                m_TitleBar.ButtonForegroundColor = Colors.White;
                m_TitleBar.ButtonBackgroundColor = Color.FromArgb(80, 113, 40, 224);
                m_TitleBar.ButtonHoverForegroundColor = Colors.Gainsboro;
                m_TitleBar.ButtonHoverBackgroundColor = Color.FromArgb(100, 130, 46, 255);
                m_TitleBar.ButtonPressedForegroundColor = Colors.Gray;
                m_TitleBar.ButtonPressedBackgroundColor = Color.FromArgb(100, 130, 46, 255);

                // Set inactive window colors.
                // Note: No effect when app is running on Windows 10
                // because color customization is not supported.
                m_TitleBar.InactiveForegroundColor = Colors.Gainsboro;
                m_TitleBar.InactiveBackgroundColor = Colors.SeaGreen;
                m_TitleBar.ButtonInactiveForegroundColor = Colors.Gainsboro;
                m_TitleBar.ButtonInactiveBackgroundColor = Colors.SeaGreen;

                m_TitleBar.IconShowOptions = IconShowOptions.HideIconAndSystemMenu;
                return true;
            }
            return false;
        }
        private void WindowSetup()
        {
            Title = AppInfo.Current.DisplayInfo.DisplayName;

            WindowManager.Get(this).IsMaximizable = true;
            WindowManager.Get(this).Width = 600;
            WindowManager.Get(this).Height = 700;
            WindowManager.Get(this).IsResizable = true;
            WindowManager.Get(this).WindowState = WindowState.Normal;
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, object e)
        {
            //SendNotificationToast("Powiadomienie", "ok");
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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string taskText = TaskInput.Text;

            if (!string.IsNullOrWhiteSpace(taskText))
            {
                var newTask = new TaskItem { Task = taskText, IsCompleted = false };
                tasks.Add(newTask);

                string jsonString = JsonConvert.SerializeObject(tasks, Newtonsoft.Json.Formatting.Indented);


                try
                {
                    var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(filePath, CreationCollisionOption.ReplaceExisting);
                    await FileIO.WriteTextAsync(file, jsonString);
                    TaskInput.Text = null;
                    SendNotificationToast(taskText, "New tasks added");

                    RefreshTasks();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);

                }
            }
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
                System.Diagnostics.Debug.WriteLine($"B³¹d zapisu do pliku: {ex.Message}");
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
                }
                RefreshTasks();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"B³¹d podczas ³adowania pliku: {ex.Message}");
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = (CheckBox)sender;
            var tastItem = (TaskItem)checkBox.DataContext;
            tastItem.IsCompleted = true;
            var grid = (Grid)checkBox.Parent;
            grid.Background = new SolidColorBrush(Microsoft.UI.Colors.LightGreen);
            SaveTasks();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var checkbox = (CheckBox)sender;
            var taskItem = (TaskItem)checkbox.DataContext;
            taskItem.IsCompleted = false;
            var grid = (Grid)checkbox.Parent;
            grid.Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
            SaveTasks();
        }

        private void EditTask_Click(object sender, RoutedEventArgs e)
        {

        }
    }


    public class TaskItem
    {
        public string Task { get; set; }
        public bool IsCompleted { get; set; }
    }
}
