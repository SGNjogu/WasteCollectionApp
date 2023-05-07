using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ttnm.Domain.Data.DataService;
using ttnm.Helpers;
using ttnm.Messages;
using ttnm.Services.DataSync;
using ttnm.Services.Dialogs;
using ttnm.Services.Settings;
using ttnm.Views.Dashboard;

namespace ttnm.ViewModels
{
    [INotifyPropertyChanged]
    public partial class DashboardViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly IDataService _dataService;

        [ObservableProperty]
        private bool isAggregator;

        [ObservableProperty]
        private string userName;

        [ObservableProperty]
        private bool isCollector = true;

        private string _collector = EnumsConverter.ConvertToString(SettingsService.UserRole.Collector).ToLower();
        private string _aggregator = EnumsConverter.ConvertToString(SettingsService.UserRole.Aggregator).ToLower();

        public DashboardViewModel(IDialogService dialogService, IDataService dataService)
        {
            _dialogService = dialogService;
            _dataService = dataService;
           

            StrongReferenceMessenger.Default.Register<UserContextMessage>(this, (r, m) =>
            {
                if (m.LoadUser)
                    LoadDashboard();
            });

            StrongReferenceMessenger.Default.Register<UIMessage>(this, (sender, message) =>
            {
                LoadDashboard();
            });

            LoadDashboard();
        }

        private void LoadDashboard()
        {
            if (App.UserContext != null)
            {
                UserName = App.UserContext.name;

                if (App.UserContext.role.ToLower() == _collector)
                {
                    IsCollector = true;
                    IsAggregator = false;
                }
                if (App.UserContext.role.ToLower() == _aggregator)
                {
                    IsCollector = false;
                    IsAggregator = true;
                }
            }
            else
            {
                IsCollector = true;
                IsAggregator = false;
            }
        }

       
        [RelayCommand]
        private async Task GoToCollectionPickUpPage()
        {
            await Shell.Current.GoToAsync(nameof(CollectionPickupPage), false);
        }

        [RelayCommand]
        private async Task GoToNewTransactionPage()
        {
            await Shell.Current.GoToAsync(nameof(NewTransactionPage), false);
        }

        [RelayCommand]
        private async Task GoToPickUpSchedulePage()
        {
            await Shell.Current.GoToAsync(nameof(PickupSchedulePage), false);
        }

        [RelayCommand]
        private async Task GoToRegisterCollectorPage()
        {
            await Shell.Current.GoToAsync(nameof(RegisterCollectorPage), false);
        }

        [RelayCommand]
        private async Task GoToCollectionHistoryPage()
        {
            await Shell.Current.GoToAsync(nameof(CollectionHistoryPage), false);
        }

        [RelayCommand]
        private async Task GoToSupportPage()
        {
            await Shell.Current.GoToAsync(nameof(SupportPage), false);
        }

        [RelayCommand]
        private void OpenDialer()
        {
            _dialogService.OpenPhoneDialer(AppConstants.HelplineNumber);
        }
    }
}
