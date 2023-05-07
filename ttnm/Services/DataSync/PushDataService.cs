using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics;
using ttnm.Domain.Data.DataService;
using ttnm.Domain.Data.Entities;
using ttnm.Infrastructure.Services.Collector;
using ttnm.Messages;

namespace ttnm.Services.DataSync
{
    public class PushDataService : IPushDataService
    {

        private readonly ICollectionRequestService _collectionRequestService;
        private readonly IDataService _dataService;
        public PushDataService(IDataService dataService, ICollectionRequestService collectionRequestService)
        {
            _dataService = dataService;
            _collectionRequestService = collectionRequestService;
        }


        public async Task<bool> SyncAcceptedRequests(int collectitonId)
        {
            try
            {
                var user = App.UserContext;
                if (user == null || user.role != "Collector") return false;
                var pendingRequests = await _dataService.GetAllItemsAsync<PendingRequests>();
                var selectedRequest = pendingRequests.FirstOrDefault(x => x.Item_id == collectitonId);

                var response = await _collectionRequestService.AcceptCollectionRequest(collectitonId, (int)user.id).ConfigureAwait(true);
                if (response != null && response.message == "Collection request ACCEPTED!")
                {
                    await _dataService.DeleteItemAsync<PendingRequests>(selectedRequest);
                    var acceptedItem = selectedRequest;

                    await _dataService.InsertItemAsync<AcceptedRequests>(new AcceptedRequests
                    {
                        item_id = acceptedItem.Item_id,
                        Household_remarks = acceptedItem.Household_remarks,
                        Collector_remarks = acceptedItem.Collector_remarks,
                        Status = acceptedItem.Status,
                        Description = acceptedItem.Description,
                        Request_date = acceptedItem.Request_date,
                        Collected_date = acceptedItem.Collected_date,
                        Delivered_date = acceptedItem.Delivered_date,
                        Points = acceptedItem.Points,
                        Total_weight = acceptedItem.Total_weight,
                        Confirmed_weight = acceptedItem.Confirmed_weight,
                        Pickup_address = acceptedItem.Pickup_address,
                        Pickup_latitude = acceptedItem.Pickup_latitude,
                        Pickup_longitude = acceptedItem.Pickup_longitude,
                        Extra_comments = acceptedItem.Extra_comments,
                        Contact_person = acceptedItem.Contact_person,
                        Contact_phone = acceptedItem.Contact_phone,
                        Pickup_time = acceptedItem.Pickup_time,
                        Waste_type = acceptedItem.Waste_type
                    });
                    StrongReferenceMessenger.Default.Send(new UpdateAcceptedRequests { UpdateAccepted = true });
                    StrongReferenceMessenger.Default.Send(new UpdatePendingRequests { UpdatePending = true });
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<bool> SyncCollectedRequests(int collectionId)
        {
            try
            {
                var user = App.UserContext;
                if (user == null || user.role != "Collector") return false;
                var acceptedRequests = await _dataService.GetAllItemsAsync<AcceptedRequests>();
                var selectedRequest = acceptedRequests.Where(x => x.item_id == collectionId);

                var response = await _collectionRequestService.CollectRequest(collectionId).ConfigureAwait(true);
                if (response != null && response.message == "Collection request Collected!")
                {
                    var acceptedItem = selectedRequest.FirstOrDefault();

                    await _dataService.InsertItemAsync<CollectedRequests>(new CollectedRequests
                    {
                        Household_remarks = acceptedItem.Household_remarks,
                        Collector_remarks = acceptedItem.Collector_remarks,
                        Status = acceptedItem.Status,
                        Description = acceptedItem.Description,
                        Request_date = acceptedItem.Request_date,
                        Collected_date = acceptedItem.Collected_date,
                        Delivered_date = acceptedItem.Delivered_date,
                        Points = acceptedItem.Points,
                        Total_weight = acceptedItem.Total_weight,
                        Confirmed_weight = acceptedItem.Confirmed_weight,
                        Pickup_address = acceptedItem.Pickup_address,
                        Pickup_latitude = acceptedItem.Pickup_latitude,
                        Pickup_longitude = acceptedItem.Pickup_longitude,
                        Extra_comments = acceptedItem.Extra_comments,
                        Contact_person = acceptedItem.Contact_person,
                        Contact_phone = acceptedItem.Contact_phone,
                        Pickup_time = acceptedItem.Pickup_time,
                        Waste_type = acceptedItem.Waste_type,
                    });
                    await _dataService.DeleteItemAsync<AcceptedRequests>(selectedRequest.FirstOrDefault());
                    StrongReferenceMessenger.Default.Send(new UpdateAcceptedRequests { UpdateAccepted = true });
                    StrongReferenceMessenger.Default.Send(new UpdateCollectedRequests { UpdateCollected = true });
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<bool> SyncCanceledRequests(int collectionId)
        {
            try
            {
                var user = App.UserContext;
                if (user == null || user.role != "Collector") return false;
                var acceptedRequests = await _dataService.GetAllItemsAsync<AcceptedRequests>().ConfigureAwait(true);
                var selectedRequest = acceptedRequests.FirstOrDefault(x => x.item_id == collectionId);

                var response = await _collectionRequestService.CancelCollectionRequest(collectionId).ConfigureAwait(true);
                if (response != null && response.message == "Collection request CANCELLED!" && selectedRequest != null)
                {

                    var acceptedItem = selectedRequest;

                    await _dataService.InsertItemAsync<PendingRequests>(new PendingRequests
                    {
                        Household_remarks = acceptedItem.Household_remarks,
                        Collector_remarks = acceptedItem.Collector_remarks,
                        Status = acceptedItem.Status,
                        Description = acceptedItem.Description,
                        Request_date = acceptedItem.Request_date,
                        Collected_date = acceptedItem.Collected_date,
                        Delivered_date = acceptedItem.Delivered_date,
                        Points = acceptedItem.Points,
                        Total_weight = acceptedItem.Total_weight,
                        Confirmed_weight = acceptedItem.Confirmed_weight,
                        Pickup_address = acceptedItem.Pickup_address,
                        Pickup_latitude = acceptedItem.Pickup_latitude,
                        Pickup_longitude = acceptedItem.Pickup_longitude,
                        Extra_comments = acceptedItem.Extra_comments,
                        Contact_person = acceptedItem.Contact_person,
                        Contact_phone = acceptedItem.Contact_phone,
                        Pickup_time = acceptedItem.Pickup_time,
                        Waste_type = acceptedItem.Waste_type,
                        Item_id = acceptedItem.item_id
                    }).ConfigureAwait(true);

                    await _dataService.DeleteItemAsync<AcceptedRequests>(selectedRequest).ConfigureAwait(true);
                    StrongReferenceMessenger.Default.Send(new UpdateAcceptedRequests { UpdateAccepted = true });
                    StrongReferenceMessenger.Default.Send(new UpdateCollectedRequests { UpdateCollected = true });
                    StrongReferenceMessenger.Default.Send(new UpdatePendingRequests { UpdatePending = true });
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
