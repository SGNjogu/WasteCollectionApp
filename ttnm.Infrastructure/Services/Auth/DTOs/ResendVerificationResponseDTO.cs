using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ttnm.Infrastructure.Services.Auth.DTOs
{
    public class ResendVerificationResponseDTO
    {
        public string? status { get; set; }
        public string? mobile { get; set; }
    }
}
