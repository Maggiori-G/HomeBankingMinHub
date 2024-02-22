using HomeBankingMinHub.Utils;

namespace HomeBankingMinHub.Models
{
	public class Account
	{
		public long Id { get; set; }
        public string Number { get; set; }
        public DateTime CreationDate { get; set; }
        public double Balance { get; set; }
        public Client Client { get; set; }
		public long ClientId { get; set; }
		public List<Transaction> Transactions { get; set; }

        public Account()
        {

        }
        public Account(long clientId, DateTime creationDate, string number, double balance)
        {
            Id = clientId;
            Number = number;
            CreationDate = creationDate;
            Balance = balance;
        }
        
        public Account(long id)
        {
            Number = ClientUtils.GeneradorDeNumerosParaCuentas();
			CreationDate = DateTime.Now;
			Balance = 0;
			ClientId = id;
        }
    }
}