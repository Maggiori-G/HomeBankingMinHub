using HomeBankingMinHub.Models;

namespace HomeBankingMinHub.Repositories
{
	public interface ILoanRepository
	{
		public IEnumerable<Loan> GetAll();

		public Loan FindById(long id);
	}
}
