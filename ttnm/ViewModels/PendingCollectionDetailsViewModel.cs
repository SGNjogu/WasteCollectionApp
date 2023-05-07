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
    public partial class PendingCollectionDetailsViewModel
    {

        private readonly IDialogService _dialogService;
        private readonly IPushDataService _pushDataService;


        [ObservableProperty]
        private PendingCollectionRequest selectedCollection;

        [ObservableProperty]
        private MapService mapService;

        public PendingCollectionDetailsViewModel(IMapService mapService, IDialogService dialogService, IPushDataService pushDataService)
        {
            this.mapService = (MapService)mapService;
            _dialogService = dialogService;
            _pushDataService = pushDataService;

            StrongReferenceMessenger.Default.Register<PendingCollectionRequest>(this, (sender, message) =>
            {
                LoadSelectedCollection(message);
            });
        }

        private void LoadSelectedCollection(PendingCollectionRequest pendingCollection)
        {
            if (pendingCollection != null)
            {
                SelectedCollection = pendingCollection;
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

        private async Task AcceptCollection()
        {
            try
            {
                _dialogService.ShowActivityIndicator();
                if (SelectedCollection != null)
                {
                    var isAccepted = await _pushDataService.SyncAcceptedRequests(SelectedCollection.Item_id);
                    if (isAccepted)
                    {
                        _dialogService.HideActivityIndicator();
                        _dialogService.ShowSnackBar(DialogService.DialogMessage.Defined, "Collection request ACCEPTED!");
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
