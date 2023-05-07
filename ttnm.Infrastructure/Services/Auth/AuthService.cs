using ttnm.Infrastructure.Services.APIService;
using ttnm.Infrastructure.Services.Auth.DTOs;

namespace ttnm.Infrastructure.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IRestService _restService;

        public AuthService(IRestService restService)
        {
            _restService = restService;
        }

        public async Task<LoginResponse> Login(string email, string password)
        {
            try
            {
                return await _restService.POSTRequest<LoginResponse>(Constants.LoginUrl, new { email, password });
            }
            catch
            {
                throw;
            }
        }

        public async Task<LoginResponse> CheckUser(string email, string mobile)
        {
            try
            {
                return await _restService.GETRequest<LoginResponse>($"{Constants.CheckUserUrl}?email={email}&mobile={mobile}");
            }
            catch
            {
                throw;
            }
        }

        public async Task<RegistrationResponse> Register(RegistrationInput registrationInput)
        {
            try
            {
                var result = await _restService.POSTRequest<RegistrationResponse>(Constants.RegistrationUrl, registrationInput);
                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<VerificationResponseDTO> Verify(VerificationInputDTO verificationInput)
        {
            try
            {
                return await _restService.POSTRequest<VerificationResponseDTO>($"{Constants.VerificationUrl}?mobile={verificationInput.PhoneNumber}&verification_code={verificationInput.VerificationCode}");
            }
            catch
            {
                throw;
            }
        }

        public async Task<ResendVerificationResponseDTO> ResendVerification(ResendVerificationInputDTO resendVerificationInput)
        {
            try
            {
                return await _restService.POSTRequest<ResendVerificationResponseDTO>($"{Constants.ResendVerificationUrl}?mobile={resendVerificationInput.PhoneNumber}");
            }
            catch
            {
                throw;
            }
        }

        public async Task<LoginResponse> RequestPasswordChangeCode(string email)
        {
            try
            {
                return await _restService.GETRequest<LoginResponse>($"{Constants.RequestCodeUrl}?email={email}");
            }
            catch
            {
                throw;
            }
        }

        public async Task<UserUpdateResponseDTO> ConfirmPassword(PasswordRequestDTO passwordRequest)
        {
            try
            {
                return await _restService.POSTRequest<UserUpdateResponseDTO>(Constants.ChangePasswordUrl, passwordRequest);
            }
            catch
            {
                throw;
            }
        }
    }
}
