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
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ToDoPc
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static string filePath = "tasks.json";
        public static List<TaskItem> tasks = new List<TaskItem>();
        private List<string> autoSuggestWords = new List<string>()
        {
            "project", "meeting", "report", "review", "update", "design", "development", "test",
    "fix", "debug", "implementation", "analysis", "documentation", "call", "email",
    "plan", "schedule", "presentation", "research", "idea", "client", "deadline",
    "task", "goal", "strategy", "feature", "requirement", "issue", "discussion",
    "testcase", "prototype", "feedback", "optimization", "deployment", "build",
    "version", "release", "patch", "integration", "configuration", "setup", "launch",
    "training", "demo", "maintenance", "support", "upgrade", "testing", "bugfix",
    "workflow", "priority", "backlog", "meetingnotes", "tasklist", "milestone",
    "roadmap", "session", "team", "documentationreview", "presentationprep",
    "code", "sprint", "retrospective", "agenda", "workshop", "brainstorm", "actionitem",
    "delivery", "followup", "proposal", "draft", "layout", "approval", "mockup",
    "usability", "revision", "content", "media", "security", "performance",
    "analysisreport", "template", "file", "overview", "summary", "handover",
    "lessonlearned", "checklist", "assignment", "estimation", "testingplan",
    "onboarding", "recruitment", "reviewdoc", "investigation", "metrics", "audit"
        };


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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            LoadTasks();
        }

        public static async void SaveTasks()
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

                if(file == null)
                {
                    System.Diagnostics.Debug.WriteLine("Plik nie istnieje");
                }
                var jsonString = await FileIO.ReadTextAsync(file);

                if (!string.IsNullOrWhiteSpace(jsonString))
                {
                    tasks = JsonConvert.DeserializeObject<List<TaskItem>>(jsonString);


                }
                /*else
                {
                    TaskItem task = new TaskItem()
                    {
                        IsCompleted = false,
                        Description = "New task",
                        Task = "New task",
                        DueDate = DateTime.Now,
                        Category = "New task"
                    };
                    tasks.Add(task);
                    SaveTasks();
                }*/
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
            var textblock = (TextBlock)grid.Children[1];
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
            textblock.TextDecorations = Windows.UI.Text.TextDecorations.None;
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
            Frame.Navigate(typeof(TaskPage), taskItem);
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var taskItem = new TaskItem()
            {
                IsCompleted = false,
                Task = "New task",
                Description = "New task",
                DueDate = DateTime.Now,
                Category = "New task3"
            };

            tasks.Add(taskItem);

            EditTask(taskItem);
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var searchText = sender.Text.ToLower();

                var filteredTasks = tasks.Where(task => task.Task.ToLower().Contains(searchText)).ToList();

                TaskListView.ItemsSource = filteredTasks;

                if (filteredTasks.Count == 0)
                {
                    TaskListView.ItemsSource = new List<TaskItem>
            {
                new TaskItem { Task = "No tasks found", IsCompleted = false }
            };
                }
            }
            else if (args.Reason == AutoSuggestionBoxTextChangeReason.ProgrammaticChange)
            {
                if (string.IsNullOrWhiteSpace(sender.Text))
                {
                    TaskListView.ItemsSource = tasks;
                }
            }
        }


        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
           AutoSuggestBox1.Text = args.SelectedItem.ToString();
        }
    }
}
