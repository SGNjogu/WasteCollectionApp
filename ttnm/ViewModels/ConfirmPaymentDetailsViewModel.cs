using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics;
using ttnm.Infrastructure.Services.Aggregator;
using ttnm.Infrastructure.Services.Aggregator.DTOs;
using ttnm.Messages;
using ttnm.Models;
using ttnm.Services.Dialogs;
using ttnm.Views.NewTransaction;

namespace ttnm.ViewModels
{
    [INotifyPropertyChanged]
    public partial class ConfirmPaymentDetailsViewModel
    {
        [ObservableProperty]
        private double totalPrice;
        [ObservableProperty]
        private string collectorName;
        [ObservableProperty]
        private string collectorNumber;
        public Collector Collector { get; set; }

        public List<WasteCollection> WasteCollections { get; set; }

        private readonly ICollectionOrdersListService _collectionOrdersListService;
        private readonly IDialogService _dialogService;
        public ConfirmPaymentDetailsViewModel(ICollectionOrdersListService collectionOrdersListService, IDialogService dialogService)
        {
            _collectionOrdersListService = collectionOrdersListService;
            _dialogService = dialogService;

            StrongReferenceMessenger.Default.Register<WasteCollectionMessage>(this, (s, m) =>
            {
                LoadDetails(m);
            });
            _dialogService = dialogService;
        }

        private void LoadDetails(WasteCollectionMessage message)
        {
            if (message == null)
                return;

            if (message.ClearData)
            {
                CollectorName = string.Empty;
                TotalPrice = 0;
                CollectorNumber = string.Empty;
                Collector = new Collector();
                WasteCollections = new List<WasteCollection>();
            }
            else
            {
                CollectorName = message.Collector?.Name;
                TotalPrice = message.TotalPrice;
                CollectorNumber = message.Collector?.Telephone;
                Collector = message.Collector;
                WasteCollections = message.WasteCollections;
            }
        }

        private async Task<bool> UploadCollection()
        {
            _dialogService.ShowActivityIndicator();
            try
            {
                var user = App.UserContext;

                List<CollectionOrderListDTO> currentCollection = new List<CollectionOrderListDTO>();
                foreach (var item in WasteCollections)
                {
                    currentCollection.Add(new CollectionOrderListDTO
                    {
                        orderAmount = (int)(Convert.ToDouble(item.Weight) * Convert.ToDouble(item.PricePerKg)),
                        wasteType = item.WasteType,
                        weightInKg = Convert.ToDouble(item.Weight),
                        collectorId = Collector.Id,
                        aggregatorId = (int)user.aggregator_id,
                        collectionRequestId = 0
                    });
                }

                if (currentCollection.Count > 0)
                {
                    var result = await _collectionOrdersListService.UploadCollectionOrders(currentCollection);
                    if (result.FirstOrDefault().orderAmount > 0)
                    {
                        StrongReferenceMessenger.Default.Send(new AggHistoryMessage { Fetch = true });
                        return true;
                    }
                    else
                    {
                        _dialogService.ShowSnackBar(DialogService.DialogMessage.UndefinedError);
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                _dialogService.ShowSnackBar(DialogService.DialogMessage.UndefinedError);
                return false;
            }
            finally
            {
                _dialogService.HideActivityIndicator();
            }
        }

        [RelayCommand]
        private async void ShowPaymentSuccess()
        {
            var success = await UploadCollection().ConfigureAwait(true);
            if (!success)
                return;

            await Application.Current.MainPage.Navigation.PushAsync(new NewTransactionSuccessPage());
            StrongReferenceMessenger.Default.Send(new WasteCollectionMessage
            {
                Collector = Collector,
                TotalPrice = TotalPrice
            });
        }

        [RelayCommand]
        private async void GoBack()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}
