using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ttnm.Messages;
using ttnm.Models;
using ttnm.Views.NewTransaction;

namespace ttnm.ViewModels
{
    [INotifyPropertyChanged]
    public partial class EnterPaymentDetailsViewModel
    {
        [ObservableProperty]
        private double totalPrice;
        [ObservableProperty]
        private string collectorName;

        public List<WasteCollection> WasteCollections { get; set; }
        public Collector Collector { get; set; }

        private readonly ConfirmPaymentPage _confirmPaymentPage;

        public EnterPaymentDetailsViewModel(ConfirmPaymentPage confirmPaymentPage)
        {
            _confirmPaymentPage = confirmPaymentPage;
            StrongReferenceMessenger.Default.Register<WasteCollectionMessage>(this, (s, m) =>
            {
                LoadDetails(m);
            });
        }

        private void LoadDetails(WasteCollectionMessage message)
        {
            if (message == null)
                return;

            if (message.ClearData)
            {
                CollectorName = string.Empty;
                TotalPrice = 0;
                Collector = new Collector();
                WasteCollections = new List<WasteCollection>();
            }
            else
            {
                CollectorName = message.Collector?.Name;
                TotalPrice = message.TotalPrice;
                Collector = message.Collector;
                WasteCollections = message.WasteCollections;
            }
        }

        [RelayCommand]
        private async void ConfirmPayment()
        {
            await Application.Current.MainPage.Navigation.PushAsync(_confirmPaymentPage);
            StrongReferenceMessenger.Default.Send(new WasteCollectionMessage
            {
                Collector = Collector,
                TotalPrice = TotalPrice,
                WasteCollections = WasteCollections
            });
        }

        [RelayCommand]
        private async void GoBack()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}
