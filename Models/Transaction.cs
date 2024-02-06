namespace HomeBankingMinHub.Models
{
	public class Transaction
	{
        public long Id { get; set; }
        public string Type { get; set; }
        public double Ammount { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public long AccountId { get; set; }
    }
}
