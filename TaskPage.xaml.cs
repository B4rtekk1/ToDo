using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

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
        

        public TaskPage()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

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


        private void SaveTask_Click_1(object sender, RoutedEventArgs e)
        {

            string taskName = TaskNameTextBox.Text;
            string taskDescription;
            TaskDescriptionTextBox.Document.GetText(Microsoft.UI.Text.TextGetOptions.None, out taskDescription);
            DateTime taskTime = TaskTimePicker.SelectedDates[0].Date;
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


            MainPage.tasks.Add(newTask);


            Frame.GoBack();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
