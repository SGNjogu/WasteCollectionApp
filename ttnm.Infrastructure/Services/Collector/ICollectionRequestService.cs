using ttnm.Infrastructure.Services.Collector.DTOs;

namespace ttnm.Infrastructure.Services.Collector
{
    public interface ICollectionRequestService
    {
        Task<CollectionRequestUpdate> AcceptCollectionRequest(int collectionId, int collectorId);
        Task<CollectionRequestUpdate> CancelCollectionRequest(int collectionId);
        Task<CollectionRequestUpdate> CollectRequest(int collectionId);
        Task<List<CollectionRequestDTO>> GetAcceptedCollectionRequests(int collectorId);
        Task<List<CollectionRequestDTO>> GetCollectedCollectionRequests(int collectorId);
        Task<List<CollectionRequestDTO>> GetPendingCollectionRequests(int collectorId);
        Task<CollectionRequestUpdate> CreateNewCollection(CollectionOrderRequestDTO collectionOrder);
    }
}