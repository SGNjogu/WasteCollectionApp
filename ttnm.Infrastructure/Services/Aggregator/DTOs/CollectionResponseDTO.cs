using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ttnm.Infrastructure.Services.Aggregator.DTOs
{
    public class CollectionResponseDTO
    {
        public class CollectionOrder
        {
            public int? id { get; set; }
            public int? collectionRequestId { get; set; }
            public int? aggregatorId { get; set; }
            public int? collectorId { get; set; }
            public string? orderNumber { get; set; }
            public string? wasteType { get; set; }
            public string? subWasteType { get; set; }
            public string? status { get; set; }
            public double? weightInKg { get; set; }
            public double? orderAmount { get; set; }
            public string? deletedAt { get; set; }
            public string? createdAt { get; set; }
            public string? updatedAt { get; set; }
            public string? remarks { get; set; }
            public string? aggregator { get; set; }
            public string? collectionRequest { get; set; }
            public string? collector { get; set; }
            public List<string> collectionPayments { get; set; }
        }

        public class Collector
        {
            public int? id { get; set; }
            public int? userId { get; set; }
            public int? zoneId { get; set; }
            public string? name { get; set; }
            public string? roleInCompany { get; set; }
            public string? companyName { get; set; }
            public string? address { get; set; }
            public string? email { get; set; }
            public string? telephone { get; set; }
            public string? yearOfRegistration { get; set; }
            public string? createdAt { get; set; }
            public string? updatedAt { get; set; }
            public string? nhifRegistered { get; set; }
            public string? saccoRegistered { get; set; }
            public string? saccoName { get; set; }
            public string? user { get; set; }
            public string? zone { get; set; }
            public List<CollectionOrder> collectionOrders { get; set; }
            public List<string?> collectionPayments { get; set; }
            public List<string?> collectionRequests { get; set; }
            public List<string?> collectorMemberships { get; set; }
        }

        public class AggregatorCollection
        {
            public int? id { get; set; }
            public int? collectionRequestId { get; set; }
            public int? aggregatorId { get; set; }
            public int? collectorId { get; set; }
            public string? orderNumber { get; set; }
            public string? wasteType { get; set; }
            public string? subWasteType { get; set; }
            public string? status { get; set; }
            public double? weightInKg { get; set; }
            public double? orderAmount { get; set; }
            public string? deletedAt { get; set; }
            public string? createdAt { get; set; }
            public string? updatedAt { get; set; }
            public string? remarks { get; set; }
            public string? aggregator { get; set; }
            public string? collectionRequest { get; set; }
            public Collector? collector { get; set; }
            public List<string>? collectionPayments { get; set; }
        }
    }
}
