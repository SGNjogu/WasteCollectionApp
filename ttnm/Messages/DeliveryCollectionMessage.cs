namespace ttnm.Messages
{
    public class DeliveryCollectionMessage
    {
        public string WasteType { get; set; }
        public int Amount { get; set; }
        public int Weight { get; set; }
        public int PricePerKg { get; set; }
    }
}
