using ttnm.Infrastructure.Services.Aggregator.DTOs;
using ttnm.Infrastructure.Services.APIService;

namespace ttnm.Infrastructure.Services.Aggregator
{
    public class CollectionOrdersListService : ICollectionOrdersListService
    {
        private readonly IRestService _restService;

        public CollectionOrdersListService(IRestService restService)
        {
            _restService = restService;
        }
        public async Task<List<CollectionOrderListDTO>> UploadCollectionOrders(List<CollectionOrderListDTO> collections)
        {
            try
            {
                var result = await _restService.POSTRequest<List<CollectionOrderListDTO>>(Constants.AddCollectionOrdersListUrl, collections);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
