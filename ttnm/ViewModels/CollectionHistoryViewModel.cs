using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ttnm.Domain.Data.DataService;
using ttnm.Domain.Data.Entities;
using ttnm.Infrastructure.Services.Aggregator;
using ttnm.Infrastructure.Services.Collector;
using ttnm.Messages;
using ttnm.Models;
using ttnm.Services.DataSync;
using ttnm.Services.Settings;

namespace ttnm.ViewModels
{
    [INotifyPropertyChanged]
    public partial class CollectionHistoryViewModel
    {
        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private bool isCollector = true;

        [ObservableProperty]
        private bool isAggregator = false;

        [ObservableProperty]
        private IEnumerable<CollectedCollectionRequest> collectedCollectionRequests;

        [ObservableProperty]
        private List<AggCollectionHistory> aggregatorCollectionHistory;

        private readonly ICollectionRequestService _collectionRequestService;
        private readonly ISettingsService _settingsService;
        private readonly IDataService _dataService;
        private readonly IPullDataService _pullDataService;
        private readonly IAggregatorService _aggregatorService;

        public CollectionHistoryViewModel(ICollectionRequestService collectionRequestService, ISettingsService settingsService, IDataService dataService, IPullDataService pullDataService, IAggregatorService aggregatorService)
        {
            _collectionRequestService = collectionRequestService;
            _settingsService = settingsService;
            _dataService = dataService;
            _pullDataService = pullDataService;
            _aggregatorService = aggregatorService;
            StrongReferenceMessenger.Default.Register<UpdateCollectedRequests>(this, async (sender, message) =>
            {
                await GetCollectedRequests();
            });
            StrongReferenceMessenger.Default.Register<AggHistoryMessage>(this, async (sender, message) =>
            {
                await HandleMessage(message);
            });
            StrongReferenceMessenger.Default.Register<UserContextMessage>(this, async (sender, message) =>
            {
                await HandleMessage(message);
            });

            _ = Initialize();
        }

        private async Task HandleMessage(UserContextMessage message)
        {
            if (message.LoadUser)
            {
                await Initialize();
            }
        }

        private async Task HandleMessage(AggHistoryMessage message)
        {
            if (message.UpdateHistory)
            {
                await GetAggCollectionHistory();
            }
            if (message.Fetch)
            {
                await _pullDataService.UpdateAggregatorHistory();
                await GetAggCollectionHistory();
            }
        }

        private async Task Initialize()
        {
            var user = App.UserContext;

            if (user != null)
            {
                if (user.role == "Aggregator")
                {
                    IsCollector = false;
                    IsAggregator = true;
                    await GetAggCollectionHistory();
                }
                else
                {
                    IsCollector = true;
                    IsAggregator = false;
                    await GetCollectedRequests();
                }
            }
        }

        #region Collector Methods

        private async Task GetCollectedRequests()
        {
            IsBusy = true;
            var user = App.UserContext;
            if (user != null)
            {
                var requests = await _dataService.GetAllItemsAsync<CollectedRequests>();
                if (requests.Any())
                {
                    var collectionRequests = new List<CollectedCollectionRequest>();

                    foreach (var request in requests)
                    {
                        collectionRequests.Add(new CollectedCollectionRequest
                        {
                            Household_remarks = request.Household_remarks,
                            Collector_remarks = request.Collector_remarks,
                            Status = request.Status,
                            Description = request.Description,
                            Request_date = request.Request_date,
                            Collected_date = request.Collected_date,
                            Delivered_date = request.Delivered_date,
                            Points = request.Points,
                            Total_weight = request.Total_weight,
                            Confirmed_weight = request.Confirmed_weight,
                            Pickup_address = request.Pickup_address,
                            Pickup_latitude = request.Pickup_latitude,
                            Pickup_longitude = request.Pickup_longitude,
                            Extra_comments = request.Extra_comments,
                            Contact_person = request.Contact_person,
                            Contact_phone = request.Contact_phone,
                            Pickup_time = request.Pickup_time,
                            Waste_type = request.Waste_type,
                            CollectionId = request.CollectionId
                        });
                    }

                    if (collectionRequests.Count == 1 && string.IsNullOrWhiteSpace(collectionRequests.FirstOrDefault().Contact_person))
                    {
                        collectionRequests.Remove(collectionRequests.First());
                    }
                    collectionRequests.Reverse();
                    CollectedCollectionRequests = (IEnumerable<CollectedCollectionRequest>)collectionRequests;
                }
            }

            IsBusy = false;
        }

        [RelayCommand]
        private async void Refresh()
        {
            if (IsAggregator)
            {
                await _pullDataService.UpdateAggregatorHistory();
                await GetAggCollectionHistory();
                return;
            }
            await GetCollectedRequests();
            await _pullDataService.UpdateCollectedRequests();
        }

        #endregion

        #region Aggregator Methods

        private async Task GetAggCollectionHistory()
        {
            IsBusy = true;
            var user = App.UserContext;
            if (user != null && user.role == "Aggregator")
            {
                var aggHistory = await _dataService.GetAllItemsAsync<AggregatorHistory>();
                if (aggHistory.Any())
                {
                    var collectionHistory = new List<AggCollectionHistory>();

                    foreach (var collection in aggHistory)
                    {
                        collectionHistory.Add(new AggCollectionHistory
                        {
                            Created_date = collection.Created_date,
                            Order_amount = (double)collection.Order_amount,
                            Collector_name = collection.Collector_name,
                            Waste_type = collection.Waste_type,
                            Weight = (double)collection.Weight
                        });

                    }
                    collectionHistory.Reverse();

                    AggregatorCollectionHistory = (List<AggCollectionHistory>)collectionHistory;
                }
            }

            IsBusy = false;
        }
        #endregion
    }
}
