namespace vmapi.Models
{
    public class Transaction
    {
        public long Id { get; set; }
        public IEnumerable<TransactionLineItem> TransactionLineItem { get; set; }
        public decimal AmountTendered { get; set; }

    }
}
