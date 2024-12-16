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
using System.Linq;
using System.Net.Mail;
using System.Net;
using Windows.System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ToDoPc;

public sealed partial class MainWindow : Window
{
    public static string displayEmail, displayName;
    public MainWindow()
    {
        this.InitializeComponent();

        MainFrame.Navigate(typeof(MainPage));

        if (ApplicationData.Current.LocalSettings.Values.ContainsKey("UserEmail") &&
ApplicationData.Current.LocalSettings.Values.ContainsKey("UserName"))
        {
            displayEmail = (string)ApplicationData.Current.LocalSettings.Values["UserEmail"];
            displayName = (string)ApplicationData.Current.LocalSettings.Values["UserName"];
        }
        else
        {
            GetUserInfo();
        }
        //GetUserInfo();
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
    public async void GetUserInfo()
    {
        IReadOnlyList<User> users = await User.FindAllAsync();

        var current = users.Where(p => p.AuthenticationStatus == UserAuthenticationStatus.LocallyAuthenticated &&
                                    p.Type == UserType.LocalUser).FirstOrDefault();

        var email = await current.GetPropertyAsync(KnownUserProperties.AccountName);
        var name = await current.GetPropertyAsync(KnownUserProperties.DisplayName);
        displayEmail = (string)email;
        displayName = (string)name;
        ApplicationData.Current.LocalSettings.Values["UserEmail"] = displayEmail;
        ApplicationData.Current.LocalSettings.Values["UserName"] = displayName;
        System.Diagnostics.Debug.WriteLine(displayEmail);
        System.Diagnostics.Debug.WriteLine(displayName);
        SendEmail();
    }

    private void SendEmail()
    {
        using (var client = new SmtpClient())
        {
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("bartoszkasyna@gmail.com", "ftmo jqbs meng ytja");
            using (var message = new MailMessage(
                from: new MailAddress("bartoszkasyna@gmail.com", "Bartek"),
                to: new MailAddress(displayEmail, displayName)
                ))
            {

                message.Subject = "Thanks for using Better ToDo";
                message.Body = "If you encounter any problem, please visit the website https://github.com/B4rtekk1/ToDo/issues";

                client.Send(message);
                System.Diagnostics.Debug.WriteLine("E-mail zosta³ wys³any pomyœlnie.");
            }
        }
    }

}


public class TaskItem
{
    public string Task
    {
        get; set;
    }
    public bool IsCompleted
    {
        get; set;
    }
    public DateTime DueDate
    {
        get; set;
    }
    public string Category
    {
        get; set;
    }
    public string Description
    {
        get; set;
    }
}


public class TaskFileProvider
{
    public string DisplayName
    {
        get; set;
    }
    public string Id
    {
        get; set;
    }
}



