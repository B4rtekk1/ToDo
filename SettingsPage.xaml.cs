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
        displayEmail = (string)ApplicationData.Current.LocalSettings.Values["UserEmail"];
        displayName = (string)ApplicationData.Current.LocalSettings.Values["UserName"];
        UsernameTextblock.Text = displayName;
        UserEmail.Text = displayEmail;

        GetUserProfileImage();
    }

    private async void GetUserProfileImage()
    {

        var userImageProfile = UserInformation.GetAccountPicture(AccountPictureKind.LargeImage);

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
            UsernameIcon.Source = new BitmapImage(new Uri("ms-appx:///Assets/DefaultUserImage.png"));
        }
    }
    

}
