using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics;
using ttnm.Helpers;
using ttnm.Infrastructure.Services.Auth;
using ttnm.Infrastructure.Services.Auth.DTOs;
using ttnm.Messages;
using ttnm.Services.Dialogs;
using ttnm.Services.Settings;
using ttnm.Views.Login;

namespace ttnm.ViewModels
{
    [INotifyPropertyChanged]
    public partial class RegisterCollectorViewModel
    {
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string mobile;

        [ObservableProperty]
        private bool isMale;
        partial void OnIsMaleChanged(bool value)
        {
            if (value) gender = "Male";
        }

        [ObservableProperty]
        private bool isFemale;
        partial void OnIsFemaleChanged(bool value)
        {
            if (value) gender = "Female";
        }

        [ObservableProperty]
        private bool isHousehold;
        partial void OnIsHouseholdChanged(bool value)
        {
            if (value) role = "Household";
        }

        [ObservableProperty]
        private bool isCollector;
        partial void OnIsCollectorChanged(bool value)
        {
            if (value) role = "Collector";
        }

        [ObservableProperty]
        private bool isAggregator;
        partial void OnIsAggregatorChanged(bool value)
        {
            if (value) role = "Aggregator";
        }

        [ObservableProperty]
        private bool isPassword = true;
        [ObservableProperty]
        private bool iconVisible = true;

        private string gender;
        private string role;

        [ObservableProperty]
        private string zone;

        private readonly IDialogService _dialogService;
        private readonly IAuthService _loginService;
        private readonly ISettingsService _settingsService;

        public RegisterCollectorViewModel(IDialogService dialogService, IAuthService loginService, ISettingsService settingsService)
        {
            _dialogService = dialogService;
            _loginService = loginService;
            _settingsService = settingsService;
        }

        private bool ValidRegistrationDetails()
        {
            if (string.IsNullOrEmpty(name))
            {
                _dialogService.ShowSnackBar(DialogService.DialogMessage.InputError, "Name");
                return false;
            }

            if (string.IsNullOrEmpty(email))
            {
                _dialogService.ShowSnackBar(DialogService.DialogMessage.InputError, "Email");
                return false;
            }

            if (!EmailValidator.IsValidEmail(email))
            {
                _dialogService.ShowSnackBar(DialogService.DialogMessage.Defined, "The email entered is invalid");
                return false;
            }

            if (string.IsNullOrEmpty(password))
            {
                _dialogService.ShowSnackBar(DialogService.DialogMessage.InputError, "Password");
                return false;
            }

            if (string.IsNullOrEmpty(mobile))
            {
                _dialogService.ShowSnackBar(DialogService.DialogMessage.InputError, "Mobile number");
                return false;
            }

            if (string.IsNullOrEmpty(mobile))
            {
                _dialogService.ShowSnackBar(DialogService.DialogMessage.InputError, "Mobile number");
                return false;
            }

            if (!PhoneNumberValidator.IsPhoneNumberValid(mobile))
            {
                _dialogService.ShowSnackBar(DialogService.DialogMessage.Defined, "Mobile number must be in the format 0712345678");
                return false;
            }

            if (string.IsNullOrEmpty(gender))
            {
                _dialogService.ShowSnackBar(DialogService.DialogMessage.InputError, "Gender");
                return false;
            }

            if (string.IsNullOrEmpty(role))
            {
                _dialogService.ShowSnackBar(DialogService.DialogMessage.InputError, "Role");
                return false;
            }

            if (string.IsNullOrEmpty(zone))
            {
                _dialogService.ShowSnackBar(DialogService.DialogMessage.InputError, "Zone");
                return false;
            }

            return true;
        }

        [RelayCommand]
        private async Task Register()
        {
            try
            {
                if (!ValidRegistrationDetails())
                    return;

                _dialogService.ShowActivityIndicator();

                // check is user exists

                var response = await _loginService.CheckUser(email, mobile);

                if (response.user == null)
                {
                    // if does not exist register user
                    var registrationResponse = await _loginService.Register(new RegistrationInput
                    {
                        name = name,
                        email = email,
                        password = password,
                        mobile = mobile,
                        gender = gender,
                        role = role,
                        zone = zone
                    });

                    if (registrationResponse != null)
                    {
                        var userJsonString = await JsonConverter.ReturnJsonStringFromObject(registrationResponse);

                        var newVerificationInstance = new VerifyCollectorViewModel(_dialogService, _loginService);
                        await Application.Current.MainPage.Navigation.PushAsync(new VerifyCollectorPage(newVerificationInstance));

                        StrongReferenceMessenger.Default.Send(new VerificationMessage { PhoneNumber = registrationResponse.mobile });

                        _dialogService.HideActivityIndicator();
                    }
                    else
                    {
                        _dialogService.HideActivityIndicator();
                        _dialogService.ShowSnackBar(DialogService.DialogMessage.UndefinedError);
                    }
                }
                else
                {
                    _dialogService.HideActivityIndicator();
                    _dialogService.ShowSnackBar(DialogService.DialogMessage.Defined, response.msg);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                _dialogService.HideActivityIndicator();
                _dialogService.ShowSnackBar(DialogService.DialogMessage.UndefinedError);
            }
        }

        [RelayCommand]
        private void TogglePassword()
        {
            if (IsPassword)
            {
                IsPassword = false;
                IconVisible = false;
                return;
            }
            if (!IsPassword)
            {
                IsPassword = true;
                IconVisible = true;
            }
        }
    }
}
