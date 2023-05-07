using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ttnm.Messages;
using ttnm.Models;
using ttnm.Views.NewTransaction;

namespace ttnm.ViewModels
{
    [INotifyPropertyChanged]
    public partial class ConfirmDetailsViewModel
    {
        [ObservableProperty]
        private List<WasteCollection> wasteCollections;
        [ObservableProperty]
        private string collectorName;
        public Collector Collector { get; set; }
        [ObservableProperty]
        private double totalPrice;

        private readonly EnterPaymentDetailsPage _enterPaymentDetailsPage;
        public ConfirmDetailsViewModel(EnterPaymentDetailsPage enterPaymentDetailsPage)
        {
            _enterPaymentDetailsPage = enterPaymentDetailsPage;
            StrongReferenceMessenger.Default.Register<WasteCollectionMessage>(this, (s, m) =>
            {
                LoadCollections(m);
            });

            WasteCollections = new List<WasteCollection>();
        }

        private void LoadCollections(WasteCollectionMessage message)
        {
            if (message == null)
                return;

            if (message.ClearData)
            {
                Collector = new Collector();
                CollectorName = string.Empty;
                WasteCollections = new List<WasteCollection>();
                TotalPrice = 0;
            }
            else
            {
                Collector = message.Collector;
                CollectorName = message.Collector?.Name;
                WasteCollections = message.WasteCollections;
                TotalPrice = message.TotalPrice;
            }
        }

        [RelayCommand]
        private async void EnterPaymentDetails()
        {
            await Application.Current.MainPage.Navigation.PushAsync(_enterPaymentDetailsPage);
            StrongReferenceMessenger.Default.Send(new WasteCollectionMessage
            {
                Collector = Collector,
                WasteCollections = WasteCollections,
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
