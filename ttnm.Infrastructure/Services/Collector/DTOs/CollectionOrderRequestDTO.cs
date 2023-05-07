namespace ttnm.Infrastructure.Services.Collector.DTOs
{
    public class CollectionOrderRequestDTO
    {
        public int id { get; set; }
        public int agg_id { get; set; }
        public int collector_id { get; set; }
        public int weight { get; set; }
        public int amount { get; set; }
        public string? waste_type { get; set; }
        public string? created_at { get; set; } = null;

    }
}
