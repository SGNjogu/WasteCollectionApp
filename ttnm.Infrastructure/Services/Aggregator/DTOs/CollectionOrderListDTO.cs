namespace ttnm.Infrastructure.Services.Aggregator.DTOs
{
    public class CollectionOrderListDTO
    {
        public int aggregatorId { get; set; }
        public int collectorId { get; set; }
        public double weightInKg { get; set; }
        public int orderAmount { get; set; }
        public string? wasteType { get; set; }
        public int collectionRequestId { get; set; }
    }
}
