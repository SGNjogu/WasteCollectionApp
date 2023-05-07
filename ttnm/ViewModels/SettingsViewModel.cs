using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics;
using ttnm.Domain.Data.DataService;
using ttnm.Domain.Data.Entities;
using ttnm.Helpers;
using ttnm.Infrastructure.Services.Profile;
using ttnm.Infrastructure.Services.Support;
using ttnm.Messages;
using ttnm.Services.Dialogs;
using ttnm.Services.Settings;
using ttnm.Views.Login;
using ttnm.Views.Settings;

namespace ttnm.ViewModels
{
    [INotifyPropertyChanged]
    public partial class SettingsViewModel
    {
        private readonly ISettingsService _settingsService;
        private readonly IDialogService _dialogService;
        private readonly IProfileService _profileService;
        private readonly IDataService _dataService;
        private readonly ISupportService _supportService;
        private readonly LoginPage _loginPage;

        private string _collector = EnumsConverter.ConvertToString(SettingsService.UserRole.Collector).ToLower();
        private string _aggregator = EnumsConverter.ConvertToString(SettingsService.UserRole.Aggregator).ToLower();

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string mobile;

        [ObservableProperty]
        private string zone;

        partial void OnZoneChanged(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                Address = "Select Zone";
            }

        }

        [ObservableProperty]
        private string address;

        [ObservableProperty]
        private bool isCollector;

        private int userID;

        partial void OnAddressChanged(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                Address = "Update your default address for regular collection pickups";
            }
        }

        [ObservableProperty]
        private string saccoName;
        partial void OnSaccoNameChanged(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                SaccoName = "Provide the name of your sacco";
            }
        }

        [ObservableProperty]
        private int isSaccoRegistered;

        partial void OnIsSaccoRegisteredChanged(int value)
        {
            if (value == 1)
            {
                SaccoNameAvailability = true;
            }

        }

        [ObservableProperty]
        private bool saccoNameAvailability;

        [ObservableProperty]
        private int isNhifRegistered;

        [ObservableProperty]
        private int isPWD;

        public SettingsViewModel(ISettingsService settingsService, IDialogService dialogService, IProfileService profileService, LoginPage loginPage, IDataService dataService, ISupportService supportService)
        {
            _settingsService = settingsService;
            _dialogService = dialogService;
            _profileService = profileService;
            _loginPage = loginPage;
            _dataService = dataService;
            _supportService = supportService;

            StrongReferenceMessenger.Default.Register<UserContextMessage>(this, (r, m) =>
            {
                if (m.LoadUser)
                    LoadUser();
            });
            LoadUser();
        }

        private void LoadUser()
        {
            if (App.UserContext != null)
            {
                if (App.UserContext.role.ToLower() == _collector)
                {
                    IsCollector = true;
                }
                if (App.UserContext.role.ToLower() == _aggregator)
                {
                    IsCollector = false;
                }
             
                userID = App.UserContext.id;
                Name = App.UserContext.name;
                Mobile = App.UserContext.phone;
                Email = App.UserContext.email;
                Zone = App.UserContext.zone ?? "Select Zone";
                Address = App.UserContext.address;
                SaccoName = App.UserContext.sacco_name;
                IsSaccoRegistered = (int)App.UserContext.sacco_registered;
                isPWD = (int)App.UserContext.is_pwd;
                IsNhifRegistered = (int?)App.UserContext.nhif_registered ?? 0;
            }
        }

        private async Task UpdateAppUserContext()
        {
            var userJsonString = await JsonConverter.ReturnJsonStringFromObject(App.UserContext);
            await _settingsService.AddSecureSetting(SettingsService.SecureSetting.UserObject, userJsonString);
        }


        [RelayCommand]
        private async Task UpdateSacco()
        {
            try
            {
                var newSacco = await _dialogService.OpenTextInput("Sacco Name", SaccoName);
                if (newSacco != null)
                {
                    _dialogService.ShowActivityIndicator();
                    if (!string.IsNullOrEmpty(newSacco) && newSacco != SaccoName)
                    {
                        var response = await _profileService.UpdateSacco(userID, 1.ToString(), newSacco);
                        if (response != null && response.msg == "SACCO successfully updated!")
                        {
                            SaccoName = newSacco;
                            App.UserContext.sacco_name = newSacco;
                            await UpdateAppUserContext();
                            _dialogService.HideActivityIndicator();
                            _dialogService.ShowSnackBar(DialogService.DialogMessage.Defined, response.msg);
                            return;
                        }
                    }
                    _dialogService.HideActivityIndicator();
                    _dialogService.ShowSnackBar(DialogService.DialogMessage.UndefinedError);
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

        private async Task UpdateNhif()
        {
            try
            {
                var boolValue = isNhifRegistered;
                _dialogService.ShowActivityIndicator();
                var response = await _profileService.UpdateNhif(userID, boolValue.ToString());
                if (response != null && response.msg == "NHIF successfully updated!")
                {
                    App.UserContext.nhif_registered = boolValue;
                    await UpdateAppUserContext();
                    _dialogService.HideActivityIndicator();
                    _dialogService.ShowSnackBar(DialogService.DialogMessage.Defined, response.msg);
                    return;
                }
                _dialogService.HideActivityIndicator();
                _dialogService.ShowSnackBar(DialogService.DialogMessage.UndefinedError);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                _dialogService.HideActivityIndicator();
                _dialogService.ShowSnackBar(DialogService.DialogMessage.UndefinedError);
            }

        }

        [RelayCommand]

        private async Task UpdatePWD()
        {
            try
            {
                var boolValue = isPWD;
                _dialogService.ShowActivityIndicator();
                var response = await _profileService.UpdatePWD(userID, boolValue.ToString());
                if (response != null && response.msg == "Is PWD successfully updated!")
                {
                    App.UserContext.is_pwd = boolValue;
                    await UpdateAppUserContext();
                    _dialogService.HideActivityIndicator();
                    _dialogService.ShowSnackBar(DialogService.DialogMessage.Defined, response.msg);
                    return;
                }
                _dialogService.HideActivityIndicator();
                _dialogService.ShowSnackBar(DialogService.DialogMessage.UndefinedError);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                _dialogService.HideActivityIndicator();
                _dialogService.ShowSnackBar(DialogService.DialogMessage.UndefinedError);
            }

        }

        [RelayCommand]

        private void SaccoNameVisibility()
        {
            if (IsSaccoRegistered == 1)
            {
                SaccoNameAvailability = true;
            }
            else
            {
                SaccoNameAvailability = false;
            }
        }

        [RelayCommand]

        private async Task ZoneSelection()
        {
            try
            {
                var selected = await _dialogService.OpenZoneinput();
                if (selected == null || selected == "Cancel")
                {
                    return;
                }
                else
                {
                    _dialogService.ShowActivityIndicator();
                    var response = await _profileService.UpdateZone(userID, selected);
                    if (response != null && response.msg == "Zone successfully updated!")
                    {
                        Zone = selected;
                        App.UserContext.zone = selected;
                        await UpdateAppUserContext();
                        _dialogService.HideActivityIndicator();
                        _dialogService.ShowSnackBar(DialogService.DialogMessage.Defined, response.msg);
                        return;
                    }
                    _dialogService.HideActivityIndicator();
                    _dialogService.ShowSnackBar(DialogService.DialogMessage.UndefinedError);
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

        private async Task UpdateName()
        {
            try
            {
                var newName = await _dialogService.OpenTextInput("Full Name", Name);
                if (newName != null)
                {
                    _dialogService.ShowActivityIndicator();
                    if (!string.IsNullOrEmpty(newName) && newName != SaccoName)
                    {
                        var response = await _profileService.UpdateName(userID, newName);
                        if (response != null && response.msg == "Name successfully updated!")
                        {
                            Name = newName;
                            App.UserContext.name = newName;
                            await UpdateAppUserContext();
                            _dialogService.HideActivityIndicator();
                            _dialogService.ShowSnackBar(DialogService.DialogMessage.Defined, response.msg);

                            //Send message to update Dashboard
                            StrongReferenceMessenger.Default.Send(new UIMessage { UpdateDashboard = true });
                            return;
                        }
                    }
                    _dialogService.HideActivityIndicator();
                    _dialogService.ShowSnackBar(DialogService.DialogMessage.UndefinedError);
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

        private async Task NavigateToMap()
        {
            await Shell.Current.GoToAsync(nameof(MapPage), false);
        }

        [RelayCommand]

        private async Task LogOut()
        {
            try
            {
                var response = await _dialogService.OpenAlert("Sign Out", "Are you sure you want to sign out?", "Yes", "Cancel");
                if (response)
                {
                    App.UserContext = null;
                    _settingsService.ClearSettings();
                    _settingsService.RemoveAllSecureSettings();
                    Application.Current.MainPage = new NavigationPage(_loginPage);
                    await Application.Current.MainPage.Navigation.PopToRootAsync();

                    //Clear all data
                    await _dataService.DeleteAllItemsAsync<CollectedRequests>();
                    await _dataService.DeleteAllItemsAsync<AggregatorHistory>();
                    await _dataService.DeleteAllItemsAsync<AcceptedRequests>();
                    await _dataService.DeleteAllItemsAsync<PendingRequests>();
                    await _dataService.DeleteAllItemsAsync<Aggregator>();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        [RelayCommand]

        private async Task DeleteAccount()
        {
            try
            {
                var promptResponse = await _dialogService.OpenAlert("Delete Account", "This will send a request to TakaTaka Ni Mali, requesting that your account and all associated data is deleted. This will take some time to reflect on the app and portal. An adminitrator will send you a notification once your request has been processed. Are you sure you want to delete your account?", "Yes", "Cancel");
                if (promptResponse)
                {
                    //Send en email to have the account deleted
                    _dialogService.ShowActivityIndicator();
                    var response = await _supportService.SubmitQuery(App.UserContext.phone, App.UserContext.email, $"ACCOUNT DELETION: {App.UserContext.phone}", $"{App.UserContext.phone} - {App.UserContext.email} : Process Application Deletion Request", App.UserContext.name);
                    if (response.msg == "Message sent successfully!")
                    {
                        _dialogService.HideActivityIndicator();
                        _dialogService.ShowSnackBar(DialogService.DialogMessage.Defined, "Account Deletion request sent!");
                    }
                    else
                    {
                        _dialogService.HideActivityIndicator();
                        _dialogService.ShowSnackBar(DialogService.DialogMessage.UndefinedError);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
