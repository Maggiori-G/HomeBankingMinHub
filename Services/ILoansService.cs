using HomeBankingMinHub.DTOs;

namespace HomeBankingMinHub.Services
{
	public interface ILoansService
	{
		public IEnumerable<LoanDTO> GetAllLoans();
		public string CreateLoanApplication(LoanApplicationDTO loanApplicationDTO, string email);
	}
}
