using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ttnm.Domain.Data.DataService;
using ttnm.Domain.Data.Entities;
using ttnm.Infrastructure.Services.Collector;
using ttnm.Messages;
using ttnm.Models;
using ttnm.Services.DataSync;

namespace ttnm.ViewModels
{
    [INotifyPropertyChanged]
    public partial class CollectionPickupViewModel
    {
        [ObservableProperty]
        private bool isBusyPending;

        [ObservableProperty]
        private bool isBusyAccepted;

        [ObservableProperty]
        private IEnumerable<AcceptedCollectionRequest> acceptedCollectionRequests;

        [ObservableProperty]
        private IEnumerable<PendingCollectionRequest> pendingCollectionRequests;

        [ObservableProperty]
        private bool showAccepted;

        [ObservableProperty]
        private bool showPending;


        private readonly ICollectionRequestService _collectionRequestService;
        private readonly IDataService _dataService;
        private readonly IPullDataService _pullDataService;
        public CollectionPickupViewModel(ICollectionRequestService collectionRequestService, IDataService dataService, IPullDataService pullDataService)
        {
            _collectionRequestService = collectionRequestService;
            _dataService = dataService;
            _pullDataService = pullDataService;
            StrongReferenceMessenger.Default.Register<UpdateAcceptedRequests>(this, async (sender, message) =>
            {
                await GetAcceptedRequests();
            });
            StrongReferenceMessenger.Default.Register<UpdatePendingRequests>(this, async (sender, message) =>
            {
                await GetPendingRequests();
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

        private async Task Initialize()
        {
            var user = App.UserContext;

            if (user != null)
            {
                if (user.role == "Collector")
                {
                    ShowPending = true;
                    ShowAccepted = false;
                    await GetPendingRequests();
                    await GetAcceptedRequests();
                }
            }
        }

        private async Task GetAcceptedRequests()
        {
            IsBusyAccepted = true;
            var user = App.UserContext;
            if (user != null)
            {
                var requests = await _dataService.GetAllItemsAsync<AcceptedRequests>();
                if (requests.Any())
                {
                    var acceptedRequests = new List<AcceptedCollectionRequest>();

                    foreach (var request in requests)
                    {
                        acceptedRequests.Add(new AcceptedCollectionRequest
                        {
                            Item_id = (int)request.item_id,
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
                            Waste_type = request.Waste_type
                        });
                    }

                    if (acceptedRequests.Count == 1 && string.IsNullOrWhiteSpace(acceptedRequests.FirstOrDefault().Contact_person))
                    {
                        acceptedRequests.Remove(acceptedRequests.First());
                    }

                    acceptedRequests.Reverse();
                    AcceptedCollectionRequests = (IEnumerable<AcceptedCollectionRequest>)acceptedRequests;
                }
            }

            IsBusyAccepted = false;
        }

        private async Task GetPendingRequests()
        {
            IsBusyPending = true;
            var user = App.UserContext;
            if (user != null)
            {
                var requests = await _dataService.GetAllItemsAsync<PendingRequests>();
                if (requests.Any())
                {
                    var pendingRequests = new List<PendingCollectionRequest>();

                    foreach (var request in requests)
                    {
                        pendingRequests.Add(new PendingCollectionRequest
                        {
                            Item_id = (int)request.Item_id,
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
                        });
                    }

                    if (pendingRequests.Count == 1 && string.IsNullOrWhiteSpace(pendingRequests.FirstOrDefault().Contact_person))
                    {
                        pendingRequests.Remove(pendingRequests.First());
                    }

                    pendingRequests.Reverse();
                    PendingCollectionRequests = (IEnumerable<PendingCollectionRequest>)pendingRequests;
                }
            }

            IsBusyPending = false;
        }

        [RelayCommand]
        private async void RefreshAcceptedCollection()
        {
            await _pullDataService.UpdateAcceptedCollections();
        }

        [RelayCommand]
        private async void RefreshPendingCollection()
        {
            await _pullDataService.UpdatePendingCollections();
        }


        [RelayCommand]
        private void PendingVisibility()
        {
            if (!ShowPending)
            {
                ShowAccepted = false;
                ShowPending = true;
            }
        }

        [RelayCommand]
        private void AcceptedVisibility()
        {
            if (!ShowAccepted)
            {
                ShowPending = false;
                ShowAccepted = true;
            }
        }
    }
}
