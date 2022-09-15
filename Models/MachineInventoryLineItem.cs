namespace vmapi.Models
{
    public class MachineInventoryLineItem
    {
        public long Id { get; set; }
        public long MachineInventoryId { get; set; }
        public long ItemId { get; set; }
        public int Quantity { get; set; }
    }
}
