using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ttnm.Domain.Data.DataService;
using ttnm.Infrastructure.Services.Collector;
using ttnm.Models;
using ttnm.Services.Dialogs;
using ttnm.Services.Maps;
using ttnm.Views.CollectorHistory;

namespace ttnm.ViewModels
{
    [INotifyPropertyChanged]
    public partial class CollectedCollectionDetailsViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly IDataService _dataService;
        private readonly ICollectionRequestService _collectionRequestService;

        [ObservableProperty]
        private CollectedCollectionRequest selectedCollection;

        [ObservableProperty]
        private MapService mapService;

        public CollectedCollectionDetailsViewModel(IMapService mapService, ICollectionRequestService collectionRequestService, IDialogService dialogService, IDataService dataService)
        {
            this.mapService = (MapService)mapService;
            _dialogService = dialogService;
            _dataService = dataService;
            _collectionRequestService = collectionRequestService;

            StrongReferenceMessenger.Default.Register<CollectedCollectionRequest>(this, (sender, message) =>
            {
                LoadSelectedCollection(message);
            });
        }

        private void LoadSelectedCollection(CollectedCollectionRequest collectedCollection)
        {
            if (collectedCollection != null)
            {
                SelectedCollection = collectedCollection;
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
        private async void Deliver()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new CollectionDeliveryPage(_dataService, _collectionRequestService, _dialogService));
            StrongReferenceMessenger.Default.Send(SelectedCollection);
        }
    }
}
