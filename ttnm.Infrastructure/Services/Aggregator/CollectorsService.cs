using ttnm.Infrastructure.Services.Aggregator.DTOs;
using ttnm.Infrastructure.Services.APIService;

namespace ttnm.Infrastructure.Services.Aggregator
{
    public class CollectorsService : ICollectorsService
    {
        private readonly IRestService _restService;

        public CollectorsService(IRestService restService)
        {
            _restService = restService;
        }

        public async Task<List<CollectorDTO>> FetchCollectors()
        {
            try
            {
                var result = await _restService.GETRequest<List<CollectorDTO>>(Constants.FetchCollectorsUrl);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
