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
using Windows.UI.Text;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ToDoPc;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        this.InitializeComponent();
        MainFrame.Navigate(typeof(MainPage));
    }

    private void nvSample9_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        switch (args.InvokedItem.ToString())
        {
            case "Help":
                var uri = new Uri("https://github.com/B4rtekk1/ToDo/issues");
                Windows.System.Launcher.LaunchUriAsync(uri).Wait();
                break;
            case "Settings":
                MainFrame.Navigate(typeof(SettingsPage));
                break;
            case "Tasks":
                MainFrame.Navigate(typeof(MainPage));
                break;

        }
    }


}


public class TaskItem
{
    public string Task { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime DueDate { get; set; }  
    public string Category { get; set; }   
    public string Description { get; set; }
    public List<string> AttachedFiles
    {
        get; set;
    } = new();
}

