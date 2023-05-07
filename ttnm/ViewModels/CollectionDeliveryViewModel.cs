using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics;
using ttnm.Domain.Data.DataService;
using ttnm.Infrastructure.Services.Collector;
using ttnm.Messages;
using ttnm.Models;
using ttnm.Services.Dialogs;

namespace ttnm.ViewModels
{
    [INotifyPropertyChanged]
    public partial class CollectionDeliveryViewModel
    {
        [ObservableProperty]
        private List<Aggregator> aggregatorsList;

        [ObservableProperty]
        private int pricePerKg;

        [ObservableProperty]
        private int weight;

        [ObservableProperty]
        private int amount;

        [ObservableProperty]
        private string wasteType;

        [ObservableProperty]
        private Aggregator selectedAggregator;

        private string SelectedWasteType { get; set; }
        private int SelectedCollectionId { get; set; }

        private readonly IDataService _dataService;
        private readonly ICollectionRequestService _collectionRequestService;
        private readonly IDialogService _dialogService;

        public CollectionDeliveryViewModel(IDataService dataService, ICollectionRequestService collectionRequestService, IDialogService dialogService)
        {
            _dataService = dataService;
            _collectionRequestService = collectionRequestService;
            _dialogService = dialogService;

            StrongReferenceMessenger.Default.Register<UpdateAggregators>(this, (sender, message) =>
            {
                LoadAggregators();
            });

            StrongReferenceMessenger.Default.Register<CollectedCollectionRequest>(this, (sender, message) =>
            {
                LoadSelectedCollection(message);
            });
            LoadAggregators();
        }

        private void LoadSelectedCollection(CollectedCollectionRequest request)
        {
            try
            {
                if (request != null)
                {
                    WasteType = request.Waste_type;
                    Weight = Convert.ToInt32(request.Total_weight);
                    SelectedCollectionId = request.CollectionId;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async void LoadAggregators()
        {
            try
            {
                var aggregators = await _dataService.GetAllItemsAsync<Domain.Data.Entities.Aggregator>();
                var newList = new List<Aggregator>();

                if (aggregators.Count > 0)
                {
                    foreach (var item in aggregators)
                    {
                        newList.Add(new Aggregator
                        {
                            Name = item.Name,
                            AggregatorId = item.AggregatorId,
                            WasteType = item.WasteType
                        });
                    }

                    AggregatorsList = new List<Aggregator>(newList);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task SubmitCollectionDelivery()
        {
            _dialogService.ShowActivityIndicator();
            var user = App.UserContext;
            try
            {
                var result = await _collectionRequestService.CreateNewCollection(new Infrastructure.Services.Collector.DTOs.CollectionOrderRequestDTO
                {
                    id = SelectedCollectionId,
                    agg_id = SelectedAggregator.AggregatorId,
                    collector_id = (int)user.collector_id,
                    weight = Weight,
                    amount = Amount,
                    waste_type = WasteType
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                _dialogService.ShowSnackBar(DialogService.DialogMessage.UndefinedError);
            }
            finally
            {
                _dialogService.HideActivityIndicator();
            }
        }

        [RelayCommand]
        private void SubmitDelivery()
        {
            //if (
            //    SelectedAggregator == null ||
            //    string.IsNullOrWhiteSpace(WasteType) ||
            //    PricePerKg == 0 ||
            //    Weight == 0 ||
            //    Amount == 0)
            //{
            //    _dialogService.ShowSnackBar(DialogService.DialogMessage.Defined, "Please enter valid entries and try again.");
            //}
            //else
            //{
            //    await SubmitCollectionDelivery();
            //}
        }
    }
}
