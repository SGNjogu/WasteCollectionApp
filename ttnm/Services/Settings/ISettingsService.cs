using ttnm.Infrastructure.Services.Auth.DTOs;

namespace ttnm.Services.Settings
{
    public interface ISettingsService
    {
        Task AddSecureSetting(SettingsService.SecureSetting preference, string setting);
        void AddSetting(SettingsService.Setting preference, string setting);
        void ClearSettings();
        SettingsService.ClientType GetClientType();
        Task<string> GetSecureSetting(SettingsService.SecureSetting preference);
        string GetSetting(SettingsService.Setting preference);
        bool IsUserLoggedIn();
        void RemoveAllSecureSettings();
        void RemoveSecureSetting(SettingsService.SecureSetting preference);
        void RemoveSetting(SettingsService.Setting preference);
        void ListenForThemeChanges();
        void GetAppTheme();
        void ChangeToDarkTheme();
        void ChangeToLightTheme();
        void ChangeToSystemPreferredTheme();
        Task<User> CurrentUser();
    }
}