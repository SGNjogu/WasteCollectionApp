using CommunityToolkit.Mvvm.Messaging;
using System.ComponentModel;
using System.Diagnostics;
using ttnm.Domain.Data.DataService;
using ttnm.Domain.Data.Entities;
using ttnm.Helpers;
using ttnm.Infrastructure.Services.Aggregator;
using ttnm.Infrastructure.Services.Collector;
using ttnm.Messages;
using ttnm.Services.Settings;

namespace ttnm.Services.DataSync
{
    public class PullDataService : IPullDataService
    {
        private readonly IDataService _dataService;
        private readonly ISettingsService _settingsService;
        private readonly ICollectionRequestService _collectionRequestService;
        private readonly IAggregatorService _aggregatorService;
        private readonly ICollectorsService _collectorsService;

        private BackgroundWorker BackgroundWorkerClient;

        public PullDataService(IDataService dataService, ISettingsService settingsService, ICollectionRequestService collectionRequestService, IAggregatorService aggregatorService, ICollectorsService collectorsService)
        {
            _dataService = dataService;
            _settingsService = settingsService;
            _collectionRequestService = collectionRequestService;
            _aggregatorService = aggregatorService;
            _collectorsService = collectorsService;

            BackgroundWorkerClient = new BackgroundWorker();
            BackgroundWorkerClient.DoWork += DoWork;
            BackgroundWorkerClient.RunWorkerAsync();
            BackgroundWorkerClient.RunWorkerCompleted += RunWorkerCompleted;
        }

        private async void DoWork(object sender, DoWorkEventArgs e)
        {
            await BeginDataSync();
        }

        public void CancelDataSync()
        {
            try
            {
                BackgroundWorkerClient.CancelAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                Debug.WriteLine("DATAPULL WAS CANCELLED");
            }
            else
            {
                Debug.WriteLine("DATAPULLUP TO DATE");
            }
        }

        public async Task BeginDataSync()
        {
            await UpdateCollectedRequests();
            await UpdateAggregatorHistory();
            await UpdateCollectorsList();
            await UpdatePendingCollections();
            await UpdateAcceptedCollections();
            await UpdateAggregatorsList();
        }

        public async Task UpdateCollectedRequests()
        {
            try
            {
                var user = App.UserContext;

                if (user == null) return;

                //Check local
                var availableRequests = await _dataService.GetAllItemsAsync<CollectedRequests>();

                var requests = await _collectionRequestService.GetCollectedCollectionRequests((int)user.collector_id).ConfigureAwait(true);

                if (requests.Any())
                {
                    if (availableRequests.Any())
                        await _dataService.DeleteAllItemsAsync<CollectedRequests>().ConfigureAwait(true);

                    foreach (var item in requests)
                    {
                        await _dataService.InsertItemAsync<CollectedRequests>(new CollectedRequests
                        {
                            Household_remarks = item.household_remarks,
                            Collector_remarks = item.collector_remarks,
                            Status = item.status,
                            Description = item.description,
                            Request_date = item.request_date,
                            Collected_date = item.collected_date,
                            Delivered_date = item.delivered_date,
                            Points = item.points,
                            Total_weight = item.total_weight,
                            Confirmed_weight = item.confirmed_weight,
                            Pickup_address = item.pickup_address,
                            Pickup_latitude = item.pickup_latitude,
                            Pickup_longitude = item.pickup_longitude,
                            Extra_comments = item.extra_comments,
                            Contact_person = item.contact_person,
                            Contact_phone = item.contact_phone,
                            Pickup_time = item.pickup_time,
                            Waste_type = item.waste_type,
                            CollectionId = item.id
                        });
                    }
                }

                //Send message to update list
                StrongReferenceMessenger.Default.Send(new UpdateCollectedRequests { UpdateCollected = true });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task UpdateAggregatorsList()
        {
            try
            {
                var user = App.UserContext;

                var localAggregators = await _dataService.GetAllItemsAsync<Aggregator>();

                List<Aggregator> aggregators = new List<Aggregator>();

                var paperAggregators = await _aggregatorService.GetAggregators("paper");

                if (paperAggregators.Count > 0)
                {
                    foreach (var item in paperAggregators)
                    {
                        aggregators.Add(new Aggregator
                        {
                            Address = item.address,
                            AggregatorId = item.id,
                            Mobile = item.mobile,
                            Name = item.name,
                            Price = item.price,
                            WasteType = item.waste_type
                        });
                    }
                }

                var plasticAggregators = await _aggregatorService.GetAggregators("plastic");
                if (plasticAggregators.Count > 0)
                {
                    foreach (var item in plasticAggregators)
                    {
                        aggregators.Add(new Aggregator
                        {
                            Address = item.address,
                            AggregatorId = item.id,
                            Mobile = item.mobile,
                            Name = item.name,
                            Price = item.price,
                            WasteType = item.waste_type
                        });
                    }
                }

                var glassAggregators = await _aggregatorService.GetAggregators("glass");
                if (glassAggregators.Count > 0)
                {
                    foreach (var item in glassAggregators)
                    {
                        aggregators.Add(new Aggregator
                        {
                            Address = item.address,
                            AggregatorId = item.id,
                            Mobile = item.mobile,
                            Name = item.name,
                            Price = item.price,
                            WasteType = item.waste_type
                        });
                    }
                }

                var metalAggregators = await _aggregatorService.GetAggregators("metal");
                if (metalAggregators.Count > 0)
                {
                    foreach (var item in metalAggregators)
                    {
                        aggregators.Add(new Aggregator
                        {
                            Address = item.address,
                            AggregatorId = item.id,
                            Mobile = item.mobile,
                            Name = item.name,
                            Price = item.price,
                            WasteType = item.waste_type
                        });
                    }
                }

                var mixedAggregators = await _aggregatorService.GetAggregators("mixed");
                if (mixedAggregators.Count > 0)
                {
                    foreach (var item in mixedAggregators)
                    {
                        aggregators.Add(new Aggregator
                        {
                            Address = item.address,
                            AggregatorId = item.id,
                            Mobile = item.mobile,
                            Name = item.name,
                            Price = item.price,
                            WasteType = item.waste_type
                        });
                    }
                }

                if (aggregators.Count > 0)
                {
                    await _dataService.DeleteAllItemsAsync<Aggregator>();

                    await _dataService.InsertAllItemsAsync(aggregators);

                    StrongReferenceMessenger.Default.Send(new UpdateAggregators { UpdateLists = true });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateAggregatorHistory()
        {
            try
            {
                var user = App.UserContext;

                if (user == null || user.role != "Aggregator") return;

                var localHistory = await _dataService.GetAllItemsAsync<AggregatorHistory>();

                var remoteHistory = await _aggregatorService.GetCollectionHistory((int)user.aggregator_id).ConfigureAwait(true);

                if (remoteHistory.Any())
                {
                    if (localHistory.Any())
                        await _dataService.DeleteAllItemsAsync<AggregatorHistory>().ConfigureAwait(true);

                    foreach (var item in remoteHistory)
                    {
                        await _dataService.InsertItemAsync<AggregatorHistory>(new AggregatorHistory
                        {
                            Collector_name = item.collector.name,
                            Created_date = item.createdAt,
                            Weight = item.weightInKg,
                            Waste_type = item.wasteType,
                            Order_amount = item.orderAmount
                        });
                    }
                    StrongReferenceMessenger.Default.Send(new AggHistoryMessage { UpdateHistory = true });
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task UpdateAcceptedCollections()
        {
            try
            {
                var user = App.UserContext;

                if (user == null || user.role != "Collector") return;

                var localAcceptedRequests = await _dataService.GetAllItemsAsync<AcceptedRequests>();

                var remoteAcceptedRequests = await _collectionRequestService.GetAcceptedCollectionRequests((int)user.collector_id).ConfigureAwait(true);

                if (remoteAcceptedRequests.Any())
                {
                    if (localAcceptedRequests.Any())
                        await _dataService.DeleteAllItemsAsync<AcceptedRequests>().ConfigureAwait(true);

                    foreach (var item in remoteAcceptedRequests)
                    {
                        await _dataService.InsertItemAsync<AcceptedRequests>(new AcceptedRequests
                        {
                            item_id = item.id,
                            Household_remarks = item.household_remarks,
                            Collector_remarks = item.collector_remarks,
                            Status = item.status,
                            Description = item.description,
                            Request_date = item.request_date,
                            Collected_date = item.collected_date,
                            Delivered_date = item.delivered_date,
                            Points = item.points,
                            Total_weight = item.total_weight,
                            Confirmed_weight = item.confirmed_weight,
                            Pickup_address = item.pickup_address,
                            Pickup_latitude = item.pickup_latitude,
                            Pickup_longitude = item.pickup_longitude,
                            Extra_comments = item.extra_comments,
                            Contact_person = item.contact_person,
                            Contact_phone = item.contact_phone,
                            Pickup_time = item.pickup_time,
                            Waste_type = item.waste_type
                        });
                    }
                    StrongReferenceMessenger.Default.Send(new UpdateAcceptedRequests { UpdateAccepted = true });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task UpdatePendingCollections()
        {
            try
            {
                var user = App.UserContext;

                if (user == null || user.role != "Collector") return;

                var localPendingRequests = await _dataService.GetAllItemsAsync<PendingRequests>();

                var remotePendingRequests = await _collectionRequestService.GetPendingCollectionRequests((int)user.collector_id).ConfigureAwait(true);

                if (remotePendingRequests.Any())
                {
                    if (localPendingRequests.Any())
                        await _dataService.DeleteAllItemsAsync<PendingRequests>().ConfigureAwait(true);

                    foreach (var item in remotePendingRequests)
                    {
                        await _dataService.InsertItemAsync<PendingRequests>(new PendingRequests
                        {
                            Item_id = item.id,
                            Household_remarks = item.household_remarks,
                            Collector_remarks = item.collector_remarks,
                            Status = item.status,
                            Description = item.description,
                            Request_date = item.request_date,
                            Collected_date = item.collected_date,
                            Delivered_date = item.delivered_date,
                            Points = item.points,
                            Total_weight = item.total_weight,
                            Confirmed_weight = item.confirmed_weight,
                            Pickup_address = item.pickup_address,
                            Pickup_latitude = item.pickup_latitude,
                            Pickup_longitude = item.pickup_longitude,
                            Extra_comments = item.extra_comments,
                            Contact_person = item.contact_person,
                            Contact_phone = item.contact_phone,
                            Pickup_time = item.pickup_time,
                            Waste_type = item.waste_type
                        });
                    }

                    StrongReferenceMessenger.Default.Send(new UpdatePendingRequests { UpdatePending = true });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task UpdateCollectorsList()
        {
            try
            {
                var update = await _collectorsService.FetchCollectors();
                if (update != null)
                {
                    CollectorsHelper.Collectors = new List<Models.Collector>();
                    foreach (var item in update)
                    {
                        CollectorsHelper.Collectors.Add(new Models.Collector
                        {
                            Id = (int)item.Id,
                            Name = item.Name,
                            Telephone = item.Telephone,
                            NameTelephone = item.Name + " " + item.Telephone
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

    }

}
