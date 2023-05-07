using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ttnm.Infrastructure.Services.APIService;
using ttnm.Infrastructure.Services.Auth.DTOs;
using ttnm.Infrastructure.Services.Profile.DTOs;
using ttnm.Infrastructure.Services.Support.DTOs;

namespace ttnm.Infrastructure.Services.Support
{
    public class SupportService : ISupportService
    {
        private readonly IRestService _restService;

        public SupportService(IRestService restService)
        {
            _restService = restService;
        }

        public async Task<SupportResponse> SubmitQuery(string mobile, string email, string subject, string message, string name)
        {
            try
            {
                return await _restService.GETRequest<SupportResponse>($"{Constants.SupportUrl}?mobile={mobile}&email={email}&subject={subject}&msg={message}&name{name}");
            }
            catch
            {
                throw;
            }
        }
    }
}
