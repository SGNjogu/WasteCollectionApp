using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics;
using ttnm.Infrastructure.Services.Auth;
using ttnm.Infrastructure.Services.Auth.DTOs;
using ttnm.Messages;
using ttnm.Services.Dialogs;

namespace ttnm.ViewModels
{
    [INotifyPropertyChanged]
    public partial class VerifyCollectorViewModel
    {
        [ObservableProperty]
        private string phoneNumber;

        [ObservableProperty]
        private string verificationCode;

        private readonly IDialogService _dialogService;
        private readonly IAuthService _authService;
        public VerifyCollectorViewModel(IDialogService dialogService, IAuthService authService)
        {
            _dialogService = dialogService;
            _authService = authService;

            StrongReferenceMessenger.Default.Register<VerificationMessage>(this, (sender, message) =>
            {
                UpdatePhoneNumber(message);
            });
        }

        private void UpdatePhoneNumber(VerificationMessage message)
        {
            if (!string.IsNullOrWhiteSpace(message.PhoneNumber))
            {
                PhoneNumber = message.PhoneNumber;
            }
        }

        private async Task VerifyCode()
        {
            try
            {
                _dialogService.ShowActivityIndicator();

                var response = await _authService.Verify(new VerificationInputDTO { PhoneNumber = PhoneNumber, VerificationCode = VerificationCode });
                if (response != null || response.status == "success")
                {
                    _dialogService.ShowSnackBar(DialogService.DialogMessage.Defined, "Verification Successful.");

                    await Application.Current.MainPage.Navigation.PopToRootAsync();
                }
                else
                {
                    _dialogService.ShowSnackBar(DialogService.DialogMessage.UndefinedError);
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowSnackBar(DialogService.DialogMessage.UndefinedError);
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                _dialogService.HideActivityIndicator();
            }
        }

        private async Task ResendCodeVerification()
        {
            try
            {
                _dialogService.ShowActivityIndicator();

                var response = await _authService.ResendVerification(new ResendVerificationInputDTO { PhoneNumber = PhoneNumber });
                if (response != null || response.status == "successfull")
                {
                    _dialogService.ShowSnackBar(DialogService.DialogMessage.Defined, "Verification code sent.");
                }
                else
                {
                    _dialogService.ShowSnackBar(DialogService.DialogMessage.UndefinedError);
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowSnackBar(DialogService.DialogMessage.UndefinedError);
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                _dialogService.HideActivityIndicator();
            }
        }

        [RelayCommand]
        private async void Verify()
        {
            if (string.IsNullOrWhiteSpace(PhoneNumber) || string.IsNullOrWhiteSpace(VerificationCode))
            {
                _dialogService.ShowSnackBar(DialogService.DialogMessage.InputError, "Verification Code");
            }
            else
            {
                await VerifyCode();
            }
        }

        [RelayCommand]
        private async void ResendVerification()
        {
            if (string.IsNullOrWhiteSpace(PhoneNumber))
            {
                _dialogService.ShowSnackBar(DialogService.DialogMessage.InputError, "Phone Number");
            }
            else
            {
                await ResendCodeVerification();
            }
        }
    }
}
