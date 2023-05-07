using ttnm.Infrastructure.Services.APIService;
using ttnm.Infrastructure.Services.Collector.DTOs;

namespace ttnm.Infrastructure.Services.Collector
{
    public class CollectionRequestService : ICollectionRequestService
    {
        private readonly IRestService _restService;

        public CollectionRequestService(IRestService restService)
        {
            _restService = restService;
        }

        public async Task<List<CollectionRequestDTO>> GetAcceptedCollectionRequests(int collectorId)
        {
            try
            {
                string queryParameters = $"?filter=Accepted&household={collectorId}&role=Collector";
                return await _restService.GETRequest<List<CollectionRequestDTO>>(Constants.GetCollectionRequestsUrl + queryParameters);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<CollectionRequestDTO>> GetPendingCollectionRequests(int collectorId)
        {
            try
            {
                string queryParameters = $"?filter=Scheduled&household={collectorId}&role=Collector";
                return await _restService.GETRequest<List<CollectionRequestDTO>>(Constants.GetCollectionRequestsUrl + queryParameters);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<CollectionRequestDTO>> GetCollectedCollectionRequests(int collectorId)
        {
            try
            {
                string queryParameters = $"?filter=Collected&household={collectorId}&role=Collector";
                var result = await _restService.GETRequest<List<CollectionRequestDTO>>(Constants.GetCollectionRequestsUrl + queryParameters);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CollectionRequestUpdate> AcceptCollectionRequest(int collectionId, int collectorId)
        {
            try
            {
                string queryParameters = $"?id={collectionId}&user_id={collectorId}";
                var result = await _restService.GETRequest<CollectionRequestUpdate>(Constants.AcceptCollectionRequestsUrl + queryParameters);
                return result;
            }
            catch (Exception)
            {
                return new CollectionRequestUpdate { message = "Collection request ACCEPTED!", status = "200" };
                //TODO Update to throw exception when backend issues are fixed for this endpoint
                //throw;
            }
        }

        public async Task<CollectionRequestUpdate> CollectRequest(int collectionId)
        {
            try
            {
                string queryParameters = $"?id={collectionId}";
                return await _restService.GETRequest<CollectionRequestUpdate>(Constants.CollectRequestUrl + queryParameters);
            }
            catch (Exception)
            {
                return new CollectionRequestUpdate { message = "Collection request ACCEPTED!", status = "200" };
                //TODO Update to throw exception when backend issues are fixed for this endpoint
                //throw;
            }
        }

        public async Task<CollectionRequestUpdate> CancelCollectionRequest(int collectionId)
        {
            try
            {
                string queryParameters = $"?id={collectionId}";
                return await _restService.GETRequest<CollectionRequestUpdate>(Constants.CancelCollectionRequestsUrl + queryParameters);
            }
            catch (Exception)
            {
                return new CollectionRequestUpdate { message = "Collection request CANCELLED!", status = "200" };
                //TODO Update to throw exception when backend issues are fixed for this endpoint
                //throw;
            }
        }

        public async Task<CollectionRequestUpdate> CreateNewCollection(CollectionOrderRequestDTO collectionOrder)
        {
            try
            {
                var result = await _restService.POSTRequest<CollectionRequestUpdate>(Constants.NewCollectionOrderUrl, collectionOrder);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
