using ttnm.Infrastructure.Services.Auth.DTOs;

namespace ttnm.Infrastructure.Services.Auth
{
    public interface IAuthService
    {
        Task<LoginResponse> Login(string email, string password);
        Task<LoginResponse> CheckUser(string email, string mobile);
        Task<RegistrationResponse> Register(RegistrationInput registrationInput);
        Task<VerificationResponseDTO> Verify(VerificationInputDTO verificationInput);
        Task<ResendVerificationResponseDTO> ResendVerification(ResendVerificationInputDTO resendVerificationInput);
        Task<LoginResponse> RequestPasswordChangeCode(string email);
        Task<UserUpdateResponseDTO> ConfirmPassword(PasswordRequestDTO passwordRequest);
    }
}