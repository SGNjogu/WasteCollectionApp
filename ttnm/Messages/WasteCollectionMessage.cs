using ttnm.Models;

namespace ttnm.Messages
{
    public class WasteCollectionMessage
    {
        public double TotalPrice { get; set; }
        public Collector Collector { get; set; }
        public List<WasteCollection> WasteCollections { get; set; }
        public bool ClearData { get; set; } = false;
    }
}
