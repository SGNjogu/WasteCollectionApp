using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using ttnm.Helpers;
using ttnm.Infrastructure.Services.Profile;
using ttnm.Services.Dialogs;
using ttnm.Services.Maps;
using ttnm.Services.Settings;
using ttnm.Views.Settings;

namespace ttnm.ViewModels
{
    [INotifyPropertyChanged]
    public partial class MapViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly IProfileService _profileService;
        private readonly ISettingsService _settingsService;

        [ObservableProperty]
        private MapService mapService;

        public MapViewModel(IMapService mapService, IDialogService dialogService, IProfileService profileService, ISettingsService settingsService)
        {
            _dialogService = dialogService;
            _profileService = profileService;
            _settingsService = settingsService;
            this.mapService = (MapService)mapService;

            _ = LoadCurrentLocation();
        }

        private async Task LoadCurrentLocation()
        {
            MapService.ZoomMap(1.0);
            await this.mapService.SetUsersLocation();
            this.mapService.AddPinToCurrentLocation();

        }

        [RelayCommand]

        private async Task SaveCurrentLocation()
        {
            try
            {
                var currentLocation = await this.mapService.GetCurrentLocation();

                if (currentLocation != null)
                {
                    var latitude = currentLocation.Latitude;
                    var longitude = currentLocation.Longitude;

                    _dialogService.ShowActivityIndicator();
                    var response = await _profileService.UpdateAddress(App.UserContext.id, "Address", latitude.ToString(), longitude.ToString());
                    if (response != null && response.msg == "Address successfully updated!")
                    {
                        App.UserContext.latitude = latitude;
                        App.UserContext.longitude = longitude;
                        var userJsonString = await JsonConverter.ReturnJsonStringFromObject(App.UserContext);
                        await _settingsService.AddSecureSetting(SettingsService.SecureSetting.UserObject, userJsonString);
                        _dialogService.HideActivityIndicator();
                        _dialogService.ShowSnackBar(DialogService.DialogMessage.Defined, response.msg);
                        await Shell.Current.GoToAsync(nameof(SettingsPage), false);
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
    }
}
