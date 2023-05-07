using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ttnm.Infrastructure.Services.Collector.DTOs
{
    public class CollectionRequestDTO
    {
        public int id { get; set; }
        public string? household_remarks { get; set; }
        public string? collector_remarks { get; set; }
        public string? status { get; set; }
        public string? description { get; set; }
        public DateTime? request_date { get; set; }
        public string? collected_date { get; set; }
        public string? delivered_date { get; set; }
        public string? points { get; set; }
        public string? total_weight { get; set; }
        public string? confirmed_weight { get; set; }
        public string? pickup_address { get; set; }
        public string? pickup_latitude { get; set; }
        public string? pickup_longitude { get; set; }
        public string? extra_comments { get; set; }
        public string? contact_person { get; set; }
        public string? contact_phone { get; set; }
        public string? pickup_time { get; set; }
        public string? waste_type { get; set; }
    }
}
