using ttnm.Infrastructure.Services.Aggregator.DTOs;

namespace ttnm.Infrastructure.Services.Aggregator
{
    public interface IAggregatorService
    {
        Task<List<CollectionResponseDTO.AggregatorCollection>> GetCollectionHistory(int aggregatorID);
        Task<List<AggregatorDTO>> GetAggregators(string wasteType);
    }
}
