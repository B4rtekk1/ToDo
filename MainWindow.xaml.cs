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

namespace ToDoPc
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            WindowSetup();
            MainFrame.Navigate(typeof(MainPage));
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
                m_TitleBar.ForegroundColor = Colors.Black;
                m_TitleBar.BackgroundColor = Color.FromArgb(80, 113, 40, 224);
                m_TitleBar.ButtonForegroundColor = Colors.Black;
                m_TitleBar.ButtonBackgroundColor = Color.FromArgb(80, 113, 40, 224);
                m_TitleBar.ButtonHoverForegroundColor = Colors.Gainsboro;
                m_TitleBar.ButtonHoverBackgroundColor = Color.FromArgb(100, 130, 46, 255);
                m_TitleBar.ButtonPressedForegroundColor = Colors.Gray;
                m_TitleBar.ButtonPressedBackgroundColor = Color.FromArgb(100, 130, 46, 255);

                // Set inactive window colors.
                // Note: No effect when app is running on Windows 10
                // because color customization is not supported.
                m_TitleBar.InactiveForegroundColor = Colors.Gainsboro;
                m_TitleBar.InactiveBackgroundColor = Color.FromArgb(80, 113, 40, 224);
                m_TitleBar.ButtonInactiveForegroundColor = Colors.Gainsboro;
                m_TitleBar.ButtonInactiveBackgroundColor = Color.FromArgb(80, 113, 40, 224);

                m_TitleBar.ForegroundColor = Colors.Black;

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
    }


    public class TaskItem
    {
        public string Task { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime DueDate { get; set; }  
        public string Category { get; set; }   
        public string Description { get; set; }
    }
}
