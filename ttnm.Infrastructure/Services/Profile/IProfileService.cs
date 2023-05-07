using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ttnm.Infrastructure.Services.Profile.DTOs;

namespace ttnm.Infrastructure.Services.Profile
{
    public interface IProfileService
    {
        Task<ProfileUpdateResponse> UpdateSacco(int userID, string isRegistered, string saccoName);

        Task<ProfileUpdateResponse> UpdateNhif(int userID, string isRegistered);

        Task<ProfileUpdateResponse> UpdateZone(int userID, string zone);

        Task<ProfileUpdateResponse> UpdatePWD(int userID, string attribute);

        Task<ProfileUpdateResponse> UpdateName(int userID, string name);
        Task<ProfileUpdateResponse> UpdateAddress(int userID, string address, string latitude, string longitude);
    }
}
