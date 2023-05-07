using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ttnm.Messages;

namespace ttnm.ViewModels
{
    [INotifyPropertyChanged]
    public partial class NewTransactionSuccessViewModel
    {
        [ObservableProperty]
        private double totalPrice;
        [ObservableProperty]
        private string collectorName;
        [ObservableProperty]
        private string timeStamp;
        public NewTransactionSuccessViewModel()
        {
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
                TimeStamp = string.Empty;
            }
            else
            {
                CollectorName = message.Collector?.Name;
                TotalPrice = message.TotalPrice;
                TimeStamp = DateTime.Now.ToString("f");
            }
        }

        [RelayCommand]
        private async void GoToDashboard()
        {
            CollectorName = string.Empty;
            TotalPrice = 0;
            TimeStamp = string.Empty;
            await Application.Current.MainPage.Navigation.PopToRootAsync();

            StrongReferenceMessenger.Default.Send(new WasteCollectionMessage { ClearData = true });
            StrongReferenceMessenger.Default.Send(new AggHistoryMessage { Fetch = true });
        }
    }
}
