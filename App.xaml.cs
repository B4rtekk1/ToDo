using System;
using System.Net.Mail;
using System.Net;
using System.Runtime.InteropServices;
using Microsoft.UI.Xaml;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Email;

namespace ToDoPc;

public sealed partial class App : Application
{
    public static Window MainWindow
    {
        get; private set;
    }

    [DllImport("user32.dll")]
    public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    private static readonly IntPtr HWND_TOP = new IntPtr(0);
    private const uint SWP_NOZORDER = 0x0004;

    public App()
    {
        this.InitializeComponent();
    }
    

    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        var m_window = new MainWindow();
        MainWindow = m_window;

        m_window.Activate();

        m_window.DispatcherQueue.TryEnqueue(() =>
        {
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(m_window);
            SetWindowPos(hwnd, HWND_TOP, 460, 0, 1000, 1030, SWP_NOZORDER);
        });
    }
}
