namespace ttnm.Domain.Data.Entities
{
    public class Aggregator : BaseModel
    {
        public int AggregatorId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Mobile { get; set; }
        public string? WasteType { get; set; }
        public string? Price { get; set; }
    }
}
