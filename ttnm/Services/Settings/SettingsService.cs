using CommunityToolkit.Maui.Core.Platform;
using System.Diagnostics;
using ttnm.Helpers;
using UserContext = ttnm.Infrastructure.Services.Auth.DTOs.User;

namespace ttnm.Services.Settings
{
    public class SettingsService : ISettingsService
    {
        /// <summary>
        /// Settings enums for easier identity of saved items
        /// </summary>
        public enum Setting
        {
            AppTheme,
            IsLoggedIn,
            RememberLogin
        }

        /// <summary>
        /// Theme enums for easier identity of saved items
        /// </summary>
        public enum Theme
        {
            LightTheme,
            DarkTheme,
            SystemPreferred
        }

        /// <summary>
        /// SecureSettings enums for easier identity of saved items
        /// </summary>
        public enum SecureSetting
        {
            UserObject,
            UserEmail,
            UserPassword
        }

        /// <summary>
        /// Client type enum
        /// </summary>
        public enum ClientType
        {
            Desktop,
            iOS,
            Android
        }

        public enum UserRole
        {
            Collector,
            Aggregator
        }

        /// <summary>
        /// Method to add setting
        /// </summary>
        /// <param name="preference">Takes in settings enum</param>
        /// <param name="setting">takes in the setting in string</param>
        public void AddSetting(Setting preference, string setting)
        {
            Preferences.Set(EnumsConverter.ConvertToString(preference), setting);
        }

        /// <summary>
        /// Method to get setting
        /// </summary>
        /// <param name="preference">Takes in settings enum</param>
        /// <returns>Setting string</returns>
        public string GetSetting(Setting preference)
        {
            bool hasKey = Preferences.ContainsKey(EnumsConverter.ConvertToString(preference));

            if (hasKey)
            {
                return Preferences.Get(EnumsConverter.ConvertToString(preference), null);
            }

            return null;
        }

        /// <summary>
        /// Method to remove setting
        /// </summary>
        /// <param name="preference">Takes in the setting enum</param>
        public void RemoveSetting(Setting preference)
        {
            Preferences.Remove(EnumsConverter.ConvertToString(preference));
        }

        /// <summary>
        /// Method to clear all settings
        /// </summary>
        public void ClearSettings()
        {
            var savedSetting = GetSetting(Setting.RememberLogin);
            if (!string.IsNullOrEmpty(savedSetting))
            {
                var remember = bool.Parse(GetSetting(Setting.RememberLogin));

                if (remember)
                {
                    var storedPreferences = Enum.GetValues(typeof(Setting)).Cast<Setting>()
                        .Where(a => a != Setting.RememberLogin)
                        .Select(c => c.ToString())
                        .ToList();

                    foreach (var key in storedPreferences)
                    {
                        Preferences.Remove(key);
                    };
                }
                else
                {
                    Preferences.Clear();
                }
            }
            else
            {
                Preferences.Clear();
            }
           
        }

        /// <summary>
        /// Method to check if a user is logged in
        /// </summary>
        /// <returns>True if user is logged in</returns>
        public bool IsUserLoggedIn()
        {
            string isLoggedIn = GetSetting(Setting.IsLoggedIn);

            if (!string.IsNullOrEmpty(isLoggedIn))
            {
                return Convert.ToBoolean(isLoggedIn);
            }

            return false;
        }

        /// <summary>
        /// Method to get currently loggedIn user
        /// </summary>
        /// <returns>Current User</returns>
        public async Task<UserContext> CurrentUser()
        {
            if (IsUserLoggedIn())
            {
                var userJson = await GetSecureSetting(SecureSetting.UserObject);
                return await JsonConverter.ReturnObjectFromJsonString<UserContext>(userJson).ConfigureAwait(false);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Method to add secure setting
        /// </summary>
        /// <param name="preference">Takes in the secure settings enum</param>
        /// <param name="setting">Takes in the setting string</param>
        public async Task AddSecureSetting(SecureSetting preference, string setting)
        {
            try
            {
                await SecureStorage.SetAsync(EnumsConverter.ConvertToString(preference), setting);
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Method to get a secure setting
        /// </summary>
        /// <param name="preference">Takes in the secure settings enum</param>
        /// <returns>The secure setting</returns>
        public async Task<string> GetSecureSetting(SecureSetting preference)
        {
            try
            {
                return await SecureStorage.GetAsync(EnumsConverter.ConvertToString(preference));
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
                Debug.WriteLine(ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Method to remove a secure setting
        /// </summary>
        /// <param name="preference">Takes in the secure settings enum</param>
        public void RemoveSecureSetting(SecureSetting preference)
        {
            SecureStorage.Remove(EnumsConverter.ConvertToString(preference));
        }

        /// <summary>
        /// Method to remove all secure settings
        /// </summary>
        public void RemoveAllSecureSettings()
        {
            var savedSetting = GetSetting(Setting.RememberLogin);
            if (!string.IsNullOrEmpty(savedSetting))
            {
                bool remember = bool.Parse(GetSetting(Setting.RememberLogin));
                if (remember)
                {
                    var storeSecureSettings = Enum.GetValues(typeof(SecureSetting)).Cast<SecureSetting>()
                        .Where(a => a != SecureSetting.UserEmail && a != SecureSetting.UserPassword)
                        .Select(c => c.ToString())
                        .ToList();
                    foreach (var key in storeSecureSettings)
                    {
                        SecureStorage.Remove(key);
                    }
                }
                else
                {
                    SecureStorage.RemoveAll();
                }
            }
            else
            {
                SecureStorage.RemoveAll();
            }
            
        }

        /// <summary>
        /// Method to get clientType
        /// </summary>
        public ClientType GetClientType()
        {
            if (DeviceInfo.Current.Platform == DevicePlatform.iOS)
                return ClientType.iOS;

            else if (DeviceInfo.Current.Platform == DevicePlatform.Android)
                return ClientType.Android;

            return ClientType.Android;
        }

        public void GetAppTheme()
        {
            string theme = GetSetting(Setting.AppTheme);

            if (!string.IsNullOrEmpty(theme))
            {
                var appTheme = EnumsConverter.ConvertToEnum<Theme>(theme);

                switch (appTheme)
                {
                    case Theme.LightTheme:
                        ChangeToLightTheme();
                        break;
                    case Theme.DarkTheme:
                        ChangeToDarkTheme();
                        break;
                    case Theme.SystemPreferred:
                        ChangeToSystemPreferredTheme();
                        break;
                    default:
                        ChangeToSystemPreferredTheme();
                        break;
                }
            }
            else
            {
                ChangeToSystemPreferredTheme();
            }
        }

        public void ListenForThemeChanges() 
        {
            Application.Current.RequestedThemeChanged += (s, a) =>
            {
                // Respond to the theme change
                string theme = GetSetting(Setting.AppTheme);

                if (!string.IsNullOrEmpty(theme))
                {
                    var appTheme = EnumsConverter.ConvertToEnum<Theme>(theme);

                    if(appTheme == Theme.SystemPreferred)
                    {
                        var requestedTheme = a.RequestedTheme;

                        switch (requestedTheme)
                        {
                            case AppTheme.Unspecified:
                                ChangeToSystemPreferredTheme();
                                break;
                            case AppTheme.Light:
                                ChangeToLightTheme();
                                break;
                            case AppTheme.Dark:
                                ChangeToDarkTheme();
                                break;
                            default:
                                ChangeToSystemPreferredTheme();
                                break;
                        }
                    }
                }
                else
                {
                    ChangeToSystemPreferredTheme();
                }
            };
        }

        /// <summary>
        /// Changes to SystemPreferred Theme
        /// </summary>
        public void ChangeToSystemPreferredTheme()
        {
            AppTheme appTheme = Application.Current.RequestedTheme;

            if (appTheme == AppTheme.Dark)
            {
                ChangeToLightTheme();
            }
            else if (appTheme == AppTheme.Light)
            {
                ChangeToLightTheme();
            }
            else
            {
                ChangeToLightTheme();
            }

            AddSetting(Setting.AppTheme, EnumsConverter.ConvertToString(Theme.SystemPreferred));
        }

        /// <summary>
        /// Changes theme to Light Theme
        /// </summary>
        public void ChangeToLightTheme()
        {
            Application.Current.UserAppTheme = AppTheme.Light;
            AddSetting(Setting.AppTheme, EnumsConverter.ConvertToString(Theme.LightTheme));
        }

        /// <summary>
        /// Changes to Dark Theme
        /// </summary>
        public void ChangeToDarkTheme()
        {
            Application.Current.UserAppTheme = AppTheme.Dark;
            AddSetting(Setting.AppTheme, EnumsConverter.ConvertToString(Theme.DarkTheme));
        }
    }
}
