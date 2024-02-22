using HomeBankingMinHub.Models;

namespace HomeBankingMindHub.dtos
{
    public class ClientLoanDTO
    {
        public long Id { get; set; }
        public long LoanId { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public int Payments { get; set; }

        public ClientLoanDTO(ClientLoans clientLoan)
        {
            Id = clientLoan.Id;
			LoanId = clientLoan.LoanId;
			Name = clientLoan.Loan.Name;
			Amount = clientLoan.Amount;
			Payments = int.Parse(clientLoan.Payments);
        }
    }
}