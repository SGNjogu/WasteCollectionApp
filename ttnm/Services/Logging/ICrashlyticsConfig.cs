using Microsoft.AppCenter.Crashes;

namespace ttnm.Services.Logging
{
    public interface ICrashlyticsConfig
    {
        Task<ErrorAttachmentLog[]> Attachments();
    }
}