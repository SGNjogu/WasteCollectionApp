using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ttnm.Infrastructure.Services.Auth.DTOs
{
    public class RegistrationResponse
    {
        public string? name { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
        public string? mobile { get; set; }
        public string? gender { get; set; }
        public string? role { get; set; }
        public int household_id { get; set; }
        public int collector_id { get; set; }
        public int aggregator_id { get; set; }
        public string? address { get; set; }
        public string? latitude { get; set; }
        public string? longitude { get; set; }
        public int is_pwd { get; set; }
        public int sacco_registered { get; set; }
        public string? sacco_name { get; set; }
        public string? zone { get; set; }
    }
}
