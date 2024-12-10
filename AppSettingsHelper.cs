using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Storage;
using Windows.System;
using Windows.System.UserProfile;

namespace ToDoPc;

public class AppSettingsHelper
{
    private static readonly ApplicationDataContainer LocalSettings = ApplicationData.Current.LocalSettings;
    public static string displayEmail, displayName;

    public static bool IsFirstLaunch()
    {
        const string key = "IsFirstLaunch";

        if (LocalSettings.Values.ContainsKey(key))
        {
            // Nie jest to pierwsze uruchomienie
            return false;
        }

        LocalSettings.Values[key] = false;
        return true;
    }


}
