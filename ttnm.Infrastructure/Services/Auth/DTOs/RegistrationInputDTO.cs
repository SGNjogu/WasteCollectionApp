namespace ttnm.Infrastructure.Services.Auth.DTOs
{
    public class RegistrationInput
    {
        public string? name { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
        public string? mobile { get; set; }
        public string? gender { get; set; }
        public string? role { get; set; }
        public string? zone { get; set; }
    }
}
