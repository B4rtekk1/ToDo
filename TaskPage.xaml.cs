using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using Windows.Media.Playback;
using Windows.Storage.Pickers;
using Windows.Storage;
using WinRT.Interop;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;
using Microsoft.UI.Xaml.Media.Imaging;
using System.IO;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ToDoPc;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class TaskPage : Page
{
    private TaskItem _task;
    private MainPage _mainPage;
    private Storyboard _storyboardTask, _storyboardDescription;
    public static List<StorageFile> selectedFiles = new();
    

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

    private async void LoadToForms(TaskItem task)
    {
        _task = task;
        TaskNameTextBox.Text = _task.Task;
        TaskNameTextBox.SelectionStart = TaskNameTextBox.Text.Length;
        var text = _task.Description;
        TaskDescriptionTextBox.Document.SetText(Microsoft.UI.Text.TextSetOptions.None, text);
        TaskTimePicker.SelectedDates.Add(_task.DueDate);
        foreach (var item in TaskCategoryComboBox.Items)
        {
            if (item is ComboBoxItem comboBoxItem && comboBoxItem.Content.ToString() == _task.Category)
            {
                TaskCategoryComboBox.SelectedItem = comboBoxItem;
                break;
            }
        }
        DisplayFiles();
    }


    private void SaveTask_Click_1(object sender, RoutedEventArgs e)
    {
        DateTime taskTime = DateTime.Now;
        var taskName = TaskNameTextBox.Text;
        string taskDescription;
        TaskDescriptionTextBox.Document.GetText(Microsoft.UI.Text.TextGetOptions.None, out taskDescription);
        if (TaskTimePicker.SelectedDates.Count > 0)
        {
            taskTime = TaskTimePicker.SelectedDates[0].Date;
        }
        var taskCategory = (TaskCategoryComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
        if (taskCategory == null)
        {
            CategoryLabelErrorShow();
        }

        if (string.IsNullOrEmpty(taskName) || string.IsNullOrEmpty(taskCategory) || string.IsNullOrEmpty(taskDescription))
        {
            return;
        }
        var newTask = new TaskItem
        {
            Task = taskName,
            Description = taskDescription,
            DueDate = taskTime,
            IsCompleted = false,
            Category = taskCategory
        };


        MainPage.tasks.Remove(_task);
        MainPage.tasks.Add(newTask);
        MainPage.SaveTasks(selectedFiles);
        Frame.Navigate(typeof(MainPage));
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        if (_task.Equals(MainPage.tasks[MainPage.tasks.Count - 1]) && _task.Category=="New task3")
        {
            MainPage.tasks.RemoveAt(MainPage.tasks.Count - 1);
        }
        Frame.Navigate(typeof(MainPage));
    }


    private void TaskNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        _storyboardTask?.Stop();
        TaskNameTextBox.BorderBrush = null;
        var trimmedTextBox = TaskNameTextBox.Text.Trim();
        if (string.IsNullOrEmpty(trimmedTextBox))
        {
            TaskNameTextBox.BorderThickness = new Microsoft.UI.Xaml.Thickness(1);
            TaskNameTextBox.BorderBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(100, 255, 0, 0));
            ErrorMessage.Visibility = Visibility.Visible;

            DoubleAnimation fadeInAnimation = new DoubleAnimation()
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromMilliseconds(350))
            };
            Storyboard.SetTarget(fadeInAnimation, ErrorMessage);
            Storyboard.SetTargetProperty(fadeInAnimation, "Opacity");

            _storyboardTask = new Storyboard();
            _storyboardTask.Children.Add(fadeInAnimation);
            _storyboardTask.Completed += (s, ev) => _storyboardTask = null;
            _storyboardTask.Begin();
        }
        else if (ErrorMessage.Opacity == 1)
        {
            DoubleAnimation fadeOutAnimation = new DoubleAnimation()
            {
                From = 1,
                To = 0,
                Duration = new Duration(TimeSpan.FromMilliseconds(350))
            };
            Storyboard.SetTarget(fadeOutAnimation, ErrorMessage);
            Storyboard.SetTargetProperty(fadeOutAnimation, "Opacity");

            _storyboardTask = new Storyboard();
            _storyboardTask.Children.Add(fadeOutAnimation);
            _storyboardTask.Completed += (s, ev) => _storyboardTask = null;
            _storyboardTask.Begin();

            TaskNameTextBox.BorderThickness = new Microsoft.UI.Xaml.Thickness(1);
            TaskNameTextBox.BorderBrush = null;
        }
    }

    private void TaskDescriptionTextBox_TextChanged(object sender, RoutedEventArgs e)
    {
        _storyboardDescription?.Stop();
        TaskDescriptionTextBox.BorderBrush = null;
        TaskDescriptionTextBox.Document.GetText(Microsoft.UI.Text.TextGetOptions.None, out var taskDescription);
        var trimmedDescription = taskDescription.Trim();
        if (string.IsNullOrEmpty(trimmedDescription))
        {
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

            _storyboardDescription = new Storyboard();
            _storyboardDescription.Children.Add(fadeInAnimation);
            _storyboardDescription.Completed += (s, ev) => _storyboardDescription = null;
            _storyboardDescription.Begin();
        }
        else if(ErrorMessageDescription.Opacity == 1) 
        {
            DoubleAnimation fadeOutAnimation = new DoubleAnimation()
            {
                From = 1,
                To = 0,
                Duration = new Duration(TimeSpan.FromMilliseconds(500))
            };
            Storyboard.SetTarget(fadeOutAnimation, ErrorMessageDescription);
            Storyboard.SetTargetProperty(fadeOutAnimation, "Opacity");

            _storyboardDescription = new Storyboard();
            _storyboardDescription.Children.Add(fadeOutAnimation);
            _storyboardDescription.Completed += (s, ev) => _storyboardDescription = null;
            _storyboardDescription.Begin();

            TaskDescriptionTextBox.BorderBrush = null;
        }
    }

    private void DeleteTask_PointerEntered(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        DeleteTask.Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Red);

    }

    private async void LoadFileButton_Click(object sender, RoutedEventArgs e)
    {
        var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

        var window = App.MainWindow;
        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
        WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hwnd);

        openPicker.ViewMode = PickerViewMode.Thumbnail;
        openPicker.SuggestedStartLocation = PickerLocationId.Desktop;
        openPicker.FileTypeFilter.Add("*");
        
        
        IReadOnlyList<StorageFile> files = await openPicker.PickMultipleFilesAsync();

        if (files.Count > 0)
        {
            foreach (var file in files) 
            {
                if (!selectedFiles.Contains(file))
                {
                    selectedFiles.Add(file);
                }
                
            }
        }
        if (selectedFiles.Count > 3)
        {
            ContentDialog dialog = new()
            {
                XamlRoot = this.XamlRoot,
                Style = Microsoft.UI.Xaml.Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Title = "Only 3 files can be chosen",
                CloseButtonText = "OK",
                Content = "Only 3 files can be chosen, 3 files will be visible"
            };
            await dialog.ShowAsync();
            while (selectedFiles.Count > 3)
            {
                selectedFiles.RemoveAt(selectedFiles.Count - 1);
            }
        }
        DisplayFiles();

    }
    private async void DisplayFiles()
    {
        FilesPanel.Children.Clear();

        foreach (var file in selectedFiles)
        {
            var filePath = file.Path;
            StackPanel filePanel = new StackPanel() { Orientation = Orientation.Horizontal, Margin = new Microsoft.UI.Xaml.Thickness(10) };
            var fileIcon = new Microsoft.UI.Xaml.Controls.Image { Width = 40, Height = 30, Margin = new Microsoft.UI.Xaml.Thickness(10) };
            var fileThumbnail = await file.GetThumbnailAsync(Windows.Storage.FileProperties.ThumbnailMode.ListView);
            BitmapImage bitmapImage = new BitmapImage();
            using (var stream = fileThumbnail)
            {
                bitmapImage.SetSource(stream);
            }
            fileIcon.Source = bitmapImage;

            var fileName = new TextBlock { Text = file.Name, VerticalAlignment = VerticalAlignment.Center, Margin = new Microsoft.UI.Xaml.Thickness(10) };

            filePanel.Children.Add(fileIcon);
            filePanel.Children.Add(fileName);

            FilesPanel.Children.Add(filePanel);
        }
    }

    private void CategoryLabelErrorShow()
    {
        _storyboardDescription?.Stop();
        if (TaskCategoryComboBox.SelectedItem != null)
        {

            DoubleAnimation fadeOutAnimation = new DoubleAnimation()
            {
                From = 1,
                To = 0,
                Duration = new Duration(TimeSpan.FromMilliseconds(350))
            };
            Storyboard.SetTarget(fadeOutAnimation, CategoryLabelError);
            Storyboard.SetTargetProperty(fadeOutAnimation, "Opacity");

            _storyboardTask = new Storyboard();
            _storyboardTask.Children.Add(fadeOutAnimation);
            _storyboardTask.Completed += (s, ev) => _storyboardTask = null;
            _storyboardTask.Begin();
        }
        else if (CategoryLabelError.Opacity == 0)
        {
            DoubleAnimation fadeINAnimation = new DoubleAnimation()
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromMilliseconds(350))
            };
            Storyboard.SetTarget(fadeINAnimation, CategoryLabelError);
            Storyboard.SetTargetProperty(fadeINAnimation, "Opacity");

            _storyboardTask = new Storyboard();
            _storyboardTask.Children.Add(fadeINAnimation);
            _storyboardTask.Completed += (s, ev) => _storyboardTask = null;
            _storyboardTask.Begin();
        }
    }

    private void TaskCategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (TaskCategoryComboBox.SelectedItem != null && CategoryLabelError.Opacity==1)
        {

            DoubleAnimation fadeOutAnimation = new DoubleAnimation()
            {
                From = 1,
                To = 0,
                Duration = new Duration(TimeSpan.FromMilliseconds(350))
            };
            Storyboard.SetTarget(fadeOutAnimation, CategoryLabelError);
            Storyboard.SetTargetProperty(fadeOutAnimation, "Opacity");

            _storyboardTask = new Storyboard();
            _storyboardTask.Children.Add(fadeOutAnimation);
            _storyboardTask.Completed += (s, ev) => _storyboardTask = null;
            _storyboardTask.Begin();
        }
    }

    private async void DeleteTask_Click(object sender, RoutedEventArgs e)
    {

        ContentDialog contentDialog = new ContentDialog
        {
            XamlRoot = this.XamlRoot,
            Style = Microsoft.UI.Xaml.Application.Current.Resources["DefaultContentDialogStyle"] as Style,
            Title = "This action cannot be undone. \nContinue?",
            PrimaryButtonText = "Delete",
            SecondaryButtonText = "Cancel",
            DefaultButton = ContentDialogButton.Secondary,
            Content = "Are you sure you want to delete this task?"
        };

        var result = await contentDialog.ShowAsync();

        if (result == ContentDialogResult.Primary)
        {
            MainPage.tasks.Remove(_task);
            MainPage.SaveTasks();
            Frame.Navigate(typeof(MainPage));
        }

    }


}
