namespace ttnm.Infrastructure.Services.Auth.DTOs
{
    public class LoginResponse
    {
        public int status_code { get; set; }
        public string? msg { get; set; }
        public User? user { get; set; }
    }

    public class User
    {
        public int id { get; set; }
        public string? name { get; set; }
        public string? email { get; set; }
        public string? phone { get; set; }
        public string? access_token { get; set; }
        public string? role { get; set; }
        public int? household_id { get; set; }
        public int? collector_id { get; set; }
        public int? aggregator_id { get; set; }
        public string? address { get; set; }
        public double? latitude { get; set; }
        public double? longitude { get; set; }
        public int? is_pwd { get; set; }
        public int? nhif_registered { get; set; }
        public int? sacco_registered { get; set; }
        public string? sacco_name { get; set; }
        public string? zone { get; set; }
    }
}
