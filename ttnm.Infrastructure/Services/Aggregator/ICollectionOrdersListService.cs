using ttnm.Infrastructure.Services.Aggregator.DTOs;

namespace ttnm.Infrastructure.Services.Aggregator
{
    public interface ICollectionOrdersListService
    {
        Task<List<CollectionOrderListDTO>> UploadCollectionOrders(List<CollectionOrderListDTO> collections);
    }
}