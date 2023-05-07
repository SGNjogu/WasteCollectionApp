using Microsoft.AppCenter.Crashes;
using ttnm.Services.Settings;

namespace ttnm.Services.Logging
{
    public class CrashlyticsConfig : ICrashlyticsConfig
    {
        private readonly ISettingsService _settingsService;
        public CrashlyticsConfig(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public async Task<ErrorAttachmentLog[]> Attachments()
        {
            var settings = await _settingsService.CurrentUser();

            var environment = "production";

#if DEBUG
            environment = "development";
#elif STAGING
            environment = "staging";
#endif

            var isLoggedIn = true;

            if (isLoggedIn)
            {
                var userId = settings.id;
                var userName = settings.name;
                var email = settings.email;

                return new ErrorAttachmentLog[]
                {
                    ErrorAttachmentLog.AttachmentWithText(
                        $"Username: {userName} \n" +
                        $"UserId: {userId} \n" +
                        $"Email: {email} \n" +
                        $"Environment: {environment} \n", "Details.txt")
                };
            }

            return new ErrorAttachmentLog[]
            {
                ErrorAttachmentLog.AttachmentWithText(
                        $"Environment: {environment}", "Details.txt")
            };
        }
    }
}
