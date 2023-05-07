using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.ComponentModel;
using System.Diagnostics;
using ttnm.Helpers;
using ttnm.Messages;
using ttnm.Models;
using ttnm.Services.Dialogs;
using ttnm.Views.NewTransaction;

namespace ttnm.ViewModels
{
    [INotifyPropertyChanged]
    public partial class NewTransactionViewModel
    {
        private List<WasteCollection> _wasteCollectionsList;

        [ObservableProperty]
        private List<WasteCollection> wasteCollections;
        [ObservableProperty]
        private string wasteType;
        [ObservableProperty]
        private string weight;
        [ObservableProperty]
        private string pricePerKg;
        [ObservableProperty]
        private string searchText;
        [ObservableProperty]
        private List<Collector> collectors;
        [ObservableProperty]
        private List<Collector> collectorsSearchResults;

        [ObservableProperty]
        private Collector selectedCollector;

        private Collector tappedCollector;
        public Collector TappedCollector
        {
            get => tappedCollector;
            set
            {
                SetProperty(ref tappedCollector, value);
                UpdateCollector();
            }
        }
        [ObservableProperty]
        private bool isVisibleSearchResults;

        private readonly IDialogService _dialogService;
        private readonly ConfirmDetailsPage _confirmDetailsPage;

        public NewTransactionViewModel(IDialogService dialogService, ConfirmDetailsPage confirmDetailsPage)
        {
            _dialogService = dialogService;
            _confirmDetailsPage = confirmDetailsPage;

            StrongReferenceMessenger.Default.Register<WasteCollectionMessage>(this, (r, m) =>
            {
                ClearData(m);
            });

            WasteCollections = new List<WasteCollection>();
            Collectors = new List<Collector>();
            IsVisibleSearchResults = false;

            LoadCollectors();
        }

        private void ClearData(WasteCollectionMessage message)
        {
            if (message.ClearData)
            {
                WasteCollections = null;
                Collectors = CollectorsHelper.Collectors;
                IsVisibleSearchResults = false;
                SearchText = string.Empty;
                CancelSearch();
            }
        }

        private void UpdateCollector()
        {
            if (TappedCollector != null)
            {
                SelectedCollector = TappedCollector;
                SearchText = SelectedCollector?.NameTelephone;
                IsVisibleSearchResults = false;
                CollectorsSearchResults = Collectors;
                TappedCollector = null;
            }
        }

        public void LoadCollectors()
        {
            if (!Collectors.Any())
            {
                Collectors = CollectorsHelper.Collectors;
            }
        }

        [RelayCommand]
        public void Search()
        {
            try
            {
                Collectors = CollectorsHelper.Collectors;
                if (Collectors != null)
                {
                    var results = Collectors.FindAll(c => c.NameTelephone.ToLower().Contains(SearchText.ToLower()));
                    if (results.Any())
                    {
                        CollectorsSearchResults = new List<Collector>(results);
                        IsVisibleSearchResults = true;
                    }
                    else
                    {
                        _dialogService.ShowSnackBar(DialogService.DialogMessage.Defined, "No results found. Please try again.");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }

        }

        public void CancelSearch()
        {
            SearchText = string.Empty;
            IsVisibleSearchResults = false;
        }

        public void DeleteItem(WasteCollection wasteCollection)
        {
            var currentList = WasteCollections;
            if (currentList.Contains(wasteCollection))
            {
                currentList.Remove(wasteCollection);
            }

            WasteCollections = new List<WasteCollection>(currentList);
            _wasteCollectionsList = WasteCollections;
        }

        [RelayCommand]
        private void AddWasteEntry()
        {
            if (string.IsNullOrWhiteSpace(WasteType))
            {
                _dialogService.ShowSnackBar(DialogService.DialogMessage.Defined, "Enter valid Waste Type");
                return;
            }

            var validWeight = double.TryParse(Weight, out double weightResult);
            if (validWeight && weightResult > 0)
            {
                validWeight = true;
            }
            else
            {
                validWeight = false;
            }

            if (string.IsNullOrWhiteSpace(Weight) || !validWeight)
            {
                _dialogService.ShowSnackBar(DialogService.DialogMessage.Defined, "Enter valid Weight");
                return;
            }

            var validPrice = double.TryParse(PricePerKg, out double priceResult);
            if (validPrice && priceResult > 0)
            {
                validPrice = true;
            }
            else
            {
                validPrice = false;
            }

            if (string.IsNullOrWhiteSpace(PricePerKg) || !validPrice)
            {
                _dialogService.ShowSnackBar(DialogService.DialogMessage.Defined, "Enter valid Price per Kg");
                return;
            }

            if (WasteCollections == null)
            {
                _wasteCollectionsList = new List<WasteCollection>();
            }
            else
            {
                _wasteCollectionsList = WasteCollections;
            }

            _wasteCollectionsList.Add(new WasteCollection
            {
                WasteType = WasteType,
                Weight = Weight,
                PricePerKg = PricePerKg
            });

            WasteType = string.Empty;
            Weight = string.Empty;
            PricePerKg = string.Empty;

            _wasteCollectionsList.Reverse();

            WasteCollections = new List<WasteCollection>(_wasteCollectionsList);
        }

        [RelayCommand]
        private async void ConfirmDetails()
        {
            if (SelectedCollector == null || string.IsNullOrWhiteSpace(SearchText) || !WasteCollections.Any() || WasteCollections.Count < 1)
            {
                _dialogService.ShowSnackBar(DialogService.DialogMessage.Defined, "Please check your entries and try again.");
                return;
            }

            await Application.Current.MainPage.Navigation.PushAsync(_confirmDetailsPage);
            List<double> prices = new List<double>();

            foreach (var item in WasteCollections)
            {
                prices.Add(Convert.ToDouble(item.PricePerKg) * Convert.ToDouble(item.Weight));
            }

            StrongReferenceMessenger.Default.Send(new WasteCollectionMessage
            {
                Collector = SelectedCollector,
                WasteCollections = WasteCollections,
                TotalPrice = Math.Round(prices.Sum(), 2)
            });

            CollectorsSearchResults = Collectors;
            SelectedCollector = new Collector();
        }

        [RelayCommand]
        private async void GoToDashboard()
        {
            await Application.Current.MainPage.Navigation.PopToRootAsync();
        }
    }
}
