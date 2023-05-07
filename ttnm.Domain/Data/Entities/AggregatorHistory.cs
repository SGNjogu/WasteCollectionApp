using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ttnm.Domain.Data.Entities
{
    public class AggregatorHistory
    {
       
        public string? Collector_name { get; set; }

       
        public string? Created_date { get; set; }


        public double? Weight { get; set; }


        public string? Waste_type { get; set; }


        public double? Order_amount { get; set; }
    }
}
