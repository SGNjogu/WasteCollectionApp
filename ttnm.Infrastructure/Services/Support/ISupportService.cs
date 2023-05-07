using ttnm.Infrastructure.Services.Support.DTOs;

namespace ttnm.Infrastructure.Services.Support
{
    public interface ISupportService
    {
        Task<SupportResponse> SubmitQuery(string mobile, string email, string subject, string message, string name);
    }
}