using ttnm.Infrastructure.Services.Aggregator.DTOs;

namespace ttnm.Infrastructure.Services.Aggregator
{
    public interface ICollectorsService
    {
        Task<List<CollectorDTO>> FetchCollectors();
    }
}