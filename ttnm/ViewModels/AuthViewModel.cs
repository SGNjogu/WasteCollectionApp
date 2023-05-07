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
using static ttnm.Services.Settings.SettingsService;

namespace ttnm.ViewModels
{
    [INotifyPropertyChanged]
    public partial class AuthViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly IAuthService _loginService;
        private readonly ISettingsService _settingsService;

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

        [ObservableProperty]
        private bool isPassword = true;

        [ObservableProperty]
        private bool iconVisible = true;

        [ObservableProperty]
        private string forgetEmail;

        [ObservableProperty]
        private string forgetMobile;

        private int _forgetUserId;

        [ObservableProperty]
        private string forgetCode;

        [ObservableProperty]
        private string newPassword;

        [ObservableProperty]
        private string newConfirmPassword;

        [ObservableProperty]
        private bool showForgotPassword;

        [ObservableProperty]
        private bool showVerifyCode;

        [ObservableProperty]
        private bool showConfirmPassword;

        [ObservableProperty]
        private bool saveCredentials;

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

        private string gender;
        private string role;

        [ObservableProperty]
        private string zone;

        public AuthViewModel
            (
            IDialogService dialogService,
            IAuthService loginService,
            ISettingsService settingsService
            )
        {
            _dialogService = dialogService;
            _loginService = loginService;
            _settingsService = settingsService;

            gender = "Male"; //Default value, since this value is not required, and should be optional

            _ = GetUserCredentialsFromStorage();
            ShowForgotPasswordGroup();
        }

        private bool ValidLoginDetails()
        {
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

            return true;
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

        private void ShowForgotPasswordGroup()
        {
            ShowForgotPassword = true;
            ShowVerifyCode = false;
            ShowConfirmPassword = false;
        }

        private void ShowVerifyCodeGroup()
        {
            ShowVerifyCode = true;
            ShowForgotPassword = false;
            ShowConfirmPassword = false;
        }

        private void ShowConfirmPasswordGroup()
        {
            ShowConfirmPassword = true;
            ShowForgotPassword = false;
            ShowVerifyCode = false;
        }

        public async Task GetUserCredentialsFromStorage()
        {
            var savedSetting = _settingsService.GetSetting(Setting.RememberLogin);
            if (!string.IsNullOrEmpty(savedSetting))
            {
                var storedPassword = await _settingsService.GetSecureSetting(SecureSetting.UserPassword);
                var storedEmail = await _settingsService.GetSecureSetting(SecureSetting.UserEmail);
                if (!string.IsNullOrEmpty(storedPassword) && !string.IsNullOrEmpty(storedEmail))
                {

                    Email = storedEmail;
                    Password = storedPassword;
                    SaveCredentials = true;
                }
            }
            else
            {
                Email = null;
                Password = null;
                SaveCredentials = false;
                _settingsService.RemoveSetting(Setting.RememberLogin);
            }
        }

        private async Task SaveUserCredentials(string email, string password)
        {
            await _settingsService.AddSecureSetting(SecureSetting.UserPassword, password);
            await _settingsService.AddSecureSetting(SecureSetting.UserEmail, email);
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

        [RelayCommand]
        private async Task Login()
        {
            try
            {
                if (!ValidLoginDetails())
                    return;

                _dialogService.ShowActivityIndicator();

                var response = await _loginService.Login(email, password);

                if (response.user != null && response.user.role.ToLower() != "household")
                {
                    var userJsonString = await JsonConverter.ReturnJsonStringFromObject(response.user);
                    await _settingsService.AddSecureSetting(SettingsService.SecureSetting.UserObject, userJsonString);
                    _settingsService.AddSetting(SettingsService.Setting.IsLoggedIn, true.ToString());
                    App.UserContext = response.user;
                    StrongReferenceMessenger.Default.Send(new UserContextMessage { LoadUser = true });
                    Application.Current.MainPage = new AppShell();
                    await Application.Current.MainPage.Navigation.PopToRootAsync();
                    _dialogService.HideActivityIndicator();

                    await Task.Delay(3000);

                    //Send message to update Dashboard
                    StrongReferenceMessenger.Default.Send(new UIMessage { UpdateDashboard = true });
                    if (SaveCredentials)
                    {
                        SaveCredentials = true;
                        _settingsService.AddSetting(Setting.RememberLogin, true.ToString());
                        await SaveUserCredentials(Email, Password);
                    }
                    else
                    {
                        SaveCredentials = false;
                        Email = null;
                        Password = null;
                        _settingsService.RemoveSetting(Setting.RememberLogin);
                    }
                }
                else
                {
                    _dialogService.HideActivityIndicator();

                    if (response.user == null)
                    {
                        _dialogService.ShowSnackBar(DialogService.DialogMessage.Defined, response.msg);
                    }
                    else
                    {
                        _dialogService.ShowSnackBar(DialogService.DialogMessage.Defined, "Invalid credentials");
                    }
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
        private async Task SignUp()
        {
            email = null;
            password = null;
            await Application.Current.MainPage.Navigation.PushAsync(new RegistrationPage { BindingContext = this });
        }

        [RelayCommand]
        private async Task SignIn()
        {
            email = null;
            password = null;
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        [RelayCommand]
        private async Task ForgotPassword()
        {
            email = null;
            password = null;
            forgetEmail = null;
            await Application.Current.MainPage.Navigation.PushAsync(new ForgotPasswordPage { BindingContext = this });
        }

        [RelayCommand]
        private async Task RequestCode()
        {
            try
            {
                _dialogService.ShowActivityIndicator();

                if (!EmailValidator.IsValidEmail(ForgetEmail))
                {
                    _dialogService.HideActivityIndicator();
                    _dialogService.ShowSnackBar(DialogService.DialogMessage.Defined, "Enter valid email!");
                    return;
                }

                var response = await _loginService.RequestPasswordChangeCode(ForgetEmail);

                if (response.user != null)
                {
                    _dialogService.ShowSnackBar(DialogService.DialogMessage.Defined, "Verification code sent.");
                    ForgetMobile = response.user.phone;
                    _forgetUserId = response.user.id;
                    ShowVerifyCodeGroup();
                }
                else
                {
                    ForgetCode = string.Empty;
                    ForgetEmail = string.Empty;
                    ForgetMobile = string.Empty;
                    NewPassword = string.Empty;
                    NewConfirmPassword = string.Empty;
                    _dialogService.ShowSnackBar(DialogService.DialogMessage.Defined, "Sending verification code failed. Please try again.");
                    ShowForgotPasswordGroup();
                    await Application.Current.MainPage.Navigation.PopAsync();
                }

                _dialogService.HideActivityIndicator();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                _dialogService.HideActivityIndicator();
                _dialogService.ShowSnackBar(DialogService.DialogMessage.UndefinedError);
                await Application.Current.MainPage.Navigation.PopAsync();
            }
        }

        [RelayCommand]
        private async Task VerifyCode()
        {
            try
            {
                _dialogService.ShowActivityIndicator();

                var response = await _loginService.Verify(new VerificationInputDTO { PhoneNumber = ForgetMobile, VerificationCode = ForgetCode });

                if (response.status == "success")
                {
                    _dialogService.ShowSnackBar(DialogService.DialogMessage.Defined, "Verification Successful.");
                    ShowConfirmPasswordGroup();
                }
                else
                {
                    _dialogService.ShowSnackBar(DialogService.DialogMessage.Defined, "Verification Failed. Try again.");
                    ForgetCode = string.Empty;
                    ForgetEmail = string.Empty;
                    ForgetMobile = string.Empty;
                    NewPassword = string.Empty;
                    NewConfirmPassword = string.Empty;
                    ShowForgotPasswordGroup();
                    await Application.Current.MainPage.Navigation.PopAsync();
                }

                _dialogService.HideActivityIndicator();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                _dialogService.HideActivityIndicator();
                _dialogService.ShowSnackBar(DialogService.DialogMessage.UndefinedError);
                await Application.Current.MainPage.Navigation.PopAsync();
            }
        }

        [RelayCommand]
        private async Task ConfirmPassword()
        {
            try
            {
                _dialogService.ShowActivityIndicator();

                if (NewPassword != NewConfirmPassword)
                {
                    _dialogService.ShowSnackBar(DialogService.DialogMessage.Defined, "Passwords do not match. Please try again.");
                    NewPassword = string.Empty;
                    NewConfirmPassword = string.Empty;
                    _dialogService.HideActivityIndicator();
                    return;
                }

                var response = await _loginService.ConfirmPassword(new PasswordRequestDTO { id = _forgetUserId, password = NewConfirmPassword });

                if (response.msg == "Pasword successfully changed!")
                {
                    _dialogService.ShowSnackBar(DialogService.DialogMessage.Defined, "Password successfully changed!");
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    _dialogService.ShowSnackBar(DialogService.DialogMessage.Defined, "Password change failed. Please try again.");
                    ForgetCode = string.Empty;
                    ForgetEmail = string.Empty;
                    ForgetMobile = string.Empty;
                    NewPassword = string.Empty;
                    NewConfirmPassword = string.Empty;
                    ShowForgotPasswordGroup();
                    await Application.Current.MainPage.Navigation.PopAsync();
                }

                _dialogService.HideActivityIndicator();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                _dialogService.HideActivityIndicator();
                _dialogService.ShowSnackBar(DialogService.DialogMessage.UndefinedError);
                await Application.Current.MainPage.Navigation.PopAsync();
            }
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
                        // login user to get token

                        var loginResponse = await _loginService.Login(email, password);

                        if (loginResponse.user != null)
                        {
                            var userJsonString = await JsonConverter.ReturnJsonStringFromObject(loginResponse.user);
                            await _settingsService.AddSecureSetting(SettingsService.SecureSetting.UserObject, userJsonString);
                            _settingsService.AddSetting(SettingsService.Setting.IsLoggedIn, true.ToString());
                            App.UserContext = response.user;
                            StrongReferenceMessenger.Default.Send(new UserContextMessage { LoadUser = true });
                            Application.Current.MainPage = new AppShell();
                            await Application.Current.MainPage.Navigation.PopToRootAsync();
                            _dialogService.HideActivityIndicator();
                        }
                        else
                        {
                            _dialogService.HideActivityIndicator();
                            _dialogService.ShowSnackBar(DialogService.DialogMessage.Defined, loginResponse.msg);
                        }
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
    }
}
