using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics;
using ttnm.Models;
using ttnm.Services.DataSync;
using ttnm.Services.Dialogs;
using ttnm.Services.Maps;

namespace ttnm.ViewModels
{
    [INotifyPropertyChanged]
    public partial class AcceptedCollectionDetailsViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly IPushDataService _pushDataService;

        [ObservableProperty]
        private AcceptedCollectionRequest selectedCollection;

        [ObservableProperty]
        private MapService mapService;

        public AcceptedCollectionDetailsViewModel(IMapService mapService, IDialogService dialogService, IPushDataService pushDataService)
        {
            this.mapService = (MapService)mapService;
            _dialogService = dialogService;
            _pushDataService = pushDataService;

            StrongReferenceMessenger.Default.Register<AcceptedCollectionRequest>(this, (sender, message) =>
            {
                LoadSelectedCollection(message);
            });
        }

        private void LoadSelectedCollection(AcceptedCollectionRequest acceptedCollection)
        {
            if (acceptedCollection != null)
            {
                SelectedCollection = acceptedCollection;
                LoadMap();
            }
        }

        private void LoadMap()
        {
            if (SelectedCollection != null)
            {
                if (!string.IsNullOrWhiteSpace(SelectedCollection.Pickup_latitude) && !string.IsNullOrWhiteSpace(SelectedCollection.Pickup_longitude))
                {
                    mapService.UpdateMapLocation(Convert.ToDouble(SelectedCollection.Pickup_latitude), Convert.ToDouble(SelectedCollection.Pickup_longitude));
                    mapService.AddPinToCurrentLocation();
                }
                else
                {
                    mapService.SetDefaultLocation();
                }
            }
        }

        [RelayCommand]
        private void OpenDialer()
        {
            _dialogService.OpenPhoneDialer(SelectedCollection.Contact_phone);
        }

        [RelayCommand]

        private async Task Collect()
        {
            try
            {
                _dialogService.ShowActivityIndicator();
                if (SelectedCollection != null)
                {
                    var isCollected = await _pushDataService.SyncCollectedRequests(SelectedCollection.Item_id);
                    if (isCollected)
                    {
                        _dialogService.HideActivityIndicator();
                        await App.Current.MainPage.Navigation.PopAsync();
                        _dialogService.ShowSnackBar(DialogService.DialogMessage.Defined, "Collection request COLLECTED!");
                        return;
                    }
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

        private async Task CancelCollection()
        {
            try
            {
                _dialogService.ShowActivityIndicator();
                if (SelectedCollection != null)
                {
                    bool isCanceled;
                    isCanceled = await _pushDataService.SyncCanceledRequests(SelectedCollection.Item_id);
                    if (isCanceled)
                    {
                        _dialogService.HideActivityIndicator();
                        await App.Current.MainPage.Navigation.PopAsync();
                        _dialogService.ShowSnackBar(DialogService.DialogMessage.Defined, "Collection request CANCELED!");
                        return;
                    }
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
    }
}
