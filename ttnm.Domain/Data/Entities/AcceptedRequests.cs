namespace ttnm.Domain.Data.Entities
{
    public class AcceptedRequests : BaseModel
    {
        public int? item_id { get; set; }
        public string? Household_remarks { get; set; }

        public string? Collector_remarks { get; set; }

        public string? Status { get; set; }

        public string? Description { get; set; }

        public DateTime? Request_date { get; set; }

        public string? Collected_date { get; set; }

        public string? Delivered_date { get; set; }

        public string? Points { get; set; }

        public string? Total_weight { get; set; }
        public string? Confirmed_weight { get; set; }

        public string? Pickup_address { get; set; }

        public string? Pickup_latitude { get; set; }

        public string? Pickup_longitude { get; set; }

        public string? Extra_comments { get; set; }

        public string? Contact_person { get; set; }

        public string? Contact_phone { get; set; }

        public string? Pickup_time { get; set; }

        public string? Waste_type { get; set; }
    }
}

