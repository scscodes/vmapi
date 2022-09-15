namespace vmapi.Models
{
    public class TransactionLineItem
    {
        public long Id { get; set; }
        public long TransactionId { get; set; }
        public long MachineId { get; set; }
        public long ItemId { get; set; }
        public int Quantity { get; set; }
    }
}
