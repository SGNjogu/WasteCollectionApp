using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ttnm.Infrastructure.Services.APIService;
using ttnm.Infrastructure.Services.Auth.DTOs;
using ttnm.Infrastructure.Services.Profile.DTOs;

namespace ttnm.Infrastructure.Services.Profile
{
    public class ProfileService : IProfileService
    {
        private readonly IRestService _restService;

        public ProfileService(IRestService restService)
        {
            _restService = restService;
        }

        public async Task<ProfileUpdateResponse> UpdateSacco(int userID, string isRegistered,string saccoName)
        {
            try
            {
                return await _restService.GETRequest<ProfileUpdateResponse>($"{Constants.SaccoUrl}?id={userID}&sacco_registered={isRegistered}&sacco_name={saccoName}");
            }
            catch
            {
                throw;
            }
        }

        public async Task<ProfileUpdateResponse> UpdateNhif(int userID, string isRegistered)
        {
            try
            {
                return await _restService.GETRequest<ProfileUpdateResponse>($"{Constants.NhifUrl}?id={userID}&nhif_registered={isRegistered}");
            }
            catch
            {
                throw;
            }
        }

        public async Task<ProfileUpdateResponse> UpdateZone(int userID, string zone)
        {
            try
            {
                return await _restService.GETRequest<ProfileUpdateResponse>($"{Constants.ZoneUrl}?id={userID}&zone={zone}");
            }
            catch
            {
                throw;
            }
        }

        public async Task<ProfileUpdateResponse> UpdatePWD(int userID, string attribute)
        {
            try
            {
                return await _restService.GETRequest<ProfileUpdateResponse>($"{Constants.ProfileUrl}?id={userID}&type=pwd&attribute={attribute}");
            }
            catch
            {
                throw;
            }
        }

        public async Task<ProfileUpdateResponse> UpdateName(int userID, string name)
        {
            try
            {
                return await _restService.GETRequest<ProfileUpdateResponse>($"{Constants.ProfileUrl}?id={userID}&type=name&attribute={name}");
            }
            catch
            {
                throw;
            }
        }

        public async Task<ProfileUpdateResponse> UpdateAddress(int userID,string address, string latitude, string longitude)
        {
            try
            {
                return await _restService.GETRequest<ProfileUpdateResponse>($"{Constants.AddressUrl}?id={userID}&address={address}&latitude={latitude}&longitude={longitude}");
            }
            catch
            {
                throw;
            }
        }

    }
}
