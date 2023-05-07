using ttnm.Infrastructure.Services.Aggregator.DTOs;
using ttnm.Infrastructure.Services.APIService;

namespace ttnm.Infrastructure.Services.Aggregator
{
    public class AggregatorService : IAggregatorService
    {
        private readonly IRestService _restService;

        public AggregatorService(IRestService restService)
        {
            _restService = restService;
        }

        public async Task<List<CollectionResponseDTO.AggregatorCollection>> GetCollectionHistory(int aggregatorID)
        {
            try
            {
                return await _restService.GETRequest<List<CollectionResponseDTO.AggregatorCollection>>($"{Constants.AggCollectionHistoryUrl}?aggregatorid={aggregatorID}");
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<AggregatorDTO>> GetAggregators(string wasteType)
        {
            try
            {
                var result = await _restService.GETRequest<List<AggregatorDTO>>($"{Constants.GetAggregatorsListUrl}?waste_type={wasteType}");
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
