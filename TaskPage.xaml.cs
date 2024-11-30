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
        

        public TaskPage(TaskItem task)
        {
            this.InitializeComponent();
            _task = task;
            TaskNameTextBox.Text = _task.Task;
            TaskDescriptionTextBox.Text = _task.Description;
            TaskTimePicker.Date = _task.DueDate;
            //_tasks = tasksList;
        }


        private void SaveTask_Click_1(object sender, RoutedEventArgs e)
        {

            // Pobranie danych z kontrolek
            string taskName = TaskNameTextBox.Text;
            string taskDescription = TaskDescriptionTextBox.Text;
            DateTime taskTime = TaskTimePicker.Date.DateTime;
            string taskCategory = (TaskCategoryComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            // Walidacja danych (np. czy nazwa zadania nie jest pusta)
            if (string.IsNullOrEmpty(taskName))
            {
                // Pokazanie komunikatu o b³êdzie lub proœba o wpisanie nazwy
                return;
            }

            // Mo¿esz teraz zapisaæ dane do modelu lub bazy danych
            var newTask = new TaskItem
            {
                Task = taskName,
                Description = taskDescription,
                DueDate = taskTime,
                IsCompleted = false,
                Category = "dw"
                //Category = taskCategory
            };

            MainPage.tasks.Remove(_task);
            MainPage.tasks.Add(newTask);

            Frame.GoBack();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
