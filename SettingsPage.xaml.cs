using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.System;
using Windows.System.UserProfile;
using Windows.Storage;


namespace ToDoPc;

public sealed partial class SettingsPage : Page
{
    public List<string> cos = new();
    public static string displayEmail, displayName;
    public SettingsPage()
    {
        this.InitializeComponent();
        if (ApplicationData.Current.LocalSettings.Values.ContainsKey("UserEmail") &&
    ApplicationData.Current.LocalSettings.Values.ContainsKey("UserName"))
        {
            displayEmail = (string)ApplicationData.Current.LocalSettings.Values["UserEmail"];
            displayName = (string)ApplicationData.Current.LocalSettings.Values["UserName"];
            UsernameTextblock.Text = displayName;
            UserEmail.Text = displayEmail;
        }
        else
        {
            GetUserInfo();
        }
        GetUserProfileImage();
    }

    private async void GetUserProfileImage()
    {

        var userImageProfile = UserInformation.GetAccountPicture(AccountPictureKind.LargeImage);

        //AppSettingsHelper.GetUserInfo();


        // Obs³uga zdjêcia u¿ytkownika
        if (userImageProfile != null)
        {
            var imageSource = new BitmapImage();
            using (var stream = await userImageProfile.OpenReadAsync())
            {
                imageSource.SetSource(stream);
            }

            UsernameIcon.Source = imageSource;
        }
        else
        {
            // Ustawienie domyœlnego zdjêcia, jeœli u¿ytkownik nie ma zdjêcia profilowego
            UsernameIcon.Source = new BitmapImage(new Uri("ms-appx:///Assets/DefaultUserImage.png"));
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
        UsernameTextblock.Text = displayName;
        UserEmail.Text = displayEmail;
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
                to: new MailAddress(UserEmail.Text, UsernameTextblock.Text)
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
