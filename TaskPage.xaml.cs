using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ToDoPc
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TaskPage : Page
    {
        private TaskItem _task;
        private MainPage _mainPage;
        

        public TaskPage()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is MainPage mainPage)
            {
                _mainPage = mainPage;
            }

            if (e.Parameter is TaskItem taskItem)
            {
                LoadToForms(taskItem);   

            }
        }

        private void LoadToForms(TaskItem task)
        {
            _task = task;
            TaskNameTextBox.Text = _task.Task;
            TaskNameTextBox.SelectionStart = TaskNameTextBox.Text.Length;
            string text = _task.Description;
            TaskDescriptionTextBox.Document.SetText(Microsoft.UI.Text.TextSetOptions.None, text);
            TaskTimePicker.SelectedDates.Add(_task.DueDate);
        }


        private async void SaveTask_Click_1(object sender, RoutedEventArgs e)
        {
            DateTime taskTime = DateTime.Now;
            string taskName = TaskNameTextBox.Text;
            string taskDescription;
            TaskDescriptionTextBox.Document.GetText(Microsoft.UI.Text.TextGetOptions.None, out taskDescription);
            if (TaskTimePicker.SelectedDates.Count > 0)
            {
                taskTime = TaskTimePicker.SelectedDates[0].Date;
            }
            string taskCategory = (TaskCategoryComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (string.IsNullOrEmpty(taskName))
            {
                return;
            }
            var newTask = new TaskItem
            {
                Task = taskName,
                Description = taskDescription,
                DueDate = taskTime,
                IsCompleted = false,
                Category = "dw"
            };


            MainPage.tasks.Remove(_task);
            MainPage.tasks.Add(newTask);
            MainPage.SaveTasks();
            Frame.Navigate(typeof(MainPage));
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void TaskNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(string.IsNullOrEmpty(TaskNameTextBox.Text))
            {
                TaskNameTextBox.BorderThickness = new Microsoft.UI.Xaml.Thickness(1);
                TaskNameTextBox.BorderBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(100, 255, 0, 0));
                ErrorMessage.Visibility = Visibility.Visible;

                DoubleAnimation fadeInAnimation = new DoubleAnimation()
                {
                    From = 0,
                    To = 1,
                    Duration = new Duration(TimeSpan.FromMilliseconds(500))
                };
                Storyboard.SetTarget(fadeInAnimation, ErrorMessage);
                Storyboard.SetTargetProperty(fadeInAnimation, "Opacity");

                Storyboard storyboard = new Storyboard();
                storyboard.Children.Add(fadeInAnimation);

                storyboard.Begin();
            }
            else
            {
                DoubleAnimation fadeOutAnimation = new DoubleAnimation()
                {
                    From = 1,
                    To = 0,
                    Duration = new Duration(TimeSpan.FromMilliseconds(500))
                };
                Storyboard.SetTarget(fadeOutAnimation, ErrorMessage);
                Storyboard.SetTargetProperty(fadeOutAnimation, "Opacity");

                Storyboard storyboard = new Storyboard();
                storyboard.Children.Add(fadeOutAnimation);

                storyboard.Begin();

                TaskNameTextBox.BorderThickness = new Microsoft.UI.Xaml.Thickness(1);
                TaskNameTextBox.BorderBrush = null;
                //ErrorMessage.Visibility = Visibility.Collapsed;
            }
        }

        private void TaskDescriptionTextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            TaskDescriptionTextBox.Document.GetText(Microsoft.UI.Text.TextGetOptions.None, out string taskDescription);
            string trimmedDescription = taskDescription.Trim();
            if (string.IsNullOrEmpty(trimmedDescription))
            {
                TaskDescriptionTextBox.BorderThickness = new Microsoft.UI.Xaml.Thickness(1);
                TaskDescriptionTextBox.BorderBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(100, 255, 0, 0));
                ErrorMessageDescription.Visibility = Visibility.Visible;

                DoubleAnimation fadeInAnimation = new DoubleAnimation()
                {
                    From = 0,
                    To = 1,
                    Duration = new Duration(TimeSpan.FromMilliseconds(500))
                };
                Storyboard.SetTarget(fadeInAnimation, ErrorMessageDescription);
                Storyboard.SetTargetProperty(fadeInAnimation, "Opacity");

                Storyboard storyboard = new Storyboard();
                storyboard.Children.Add(fadeInAnimation);

                storyboard.Begin();
            }
            else
            {
                DoubleAnimation fadeOutAnimation = new DoubleAnimation()
                {
                    From = 1,
                    To = 0,
                    Duration = new Duration(TimeSpan.FromMilliseconds(500))
                };
                Storyboard.SetTarget(fadeOutAnimation, ErrorMessageDescription);
                Storyboard.SetTargetProperty(fadeOutAnimation, "Opacity");

                Storyboard storyboard = new Storyboard();
                storyboard.Children.Add(fadeOutAnimation);

                storyboard.Begin();

                TaskDescriptionTextBox.BorderThickness = new Microsoft.UI.Xaml.Thickness(1);
                TaskDescriptionTextBox.BorderBrush = null;
                //ErrorMessage.Visibility = Visibility.Collapsed;
            }
        }
    }
}
