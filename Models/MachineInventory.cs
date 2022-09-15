namespace vmapi.Models
{
    public class MachineInventory
    {
        public long Id { get; set; }
        public IEnumerable<MachineInventoryLineItem> Items { get; set; }
    }
}
