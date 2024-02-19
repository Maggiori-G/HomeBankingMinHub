using HomeBankingMinHub.Models;

namespace HomeBankingMinHub.DTOs
{
	public class LoanDTO
	{
        public long Id { get; set; }
        public string Name { get; set; }
        public double MaxAmount { get; set; }
        public string Payments { get; set; }

        public LoanDTO(Loan loan)
        {
            this.Id = loan.Id;
            this.Name = loan.Name;
            this.MaxAmount = loan.MaxAmount;
            this.Payments = loan.Payments;
        }
    }
}
