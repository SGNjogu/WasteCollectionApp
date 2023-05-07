using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ttnm.Infrastructure.Services.Auth.DTOs
{
    public class ResendVerificationInputDTO
    {
        public string? PhoneNumber { get; set; }
    }
}
