using HomeBankingMinHub.Intefaces;
using HomeBankingMinHub.Models;

namespace HomeBankingMinHub.Repositories
{
	public class ClientLoanRepository:RepositoryBase<ClientLoans>, IClientLoanRepository
	{
		public ClientLoanRepository(HomeBankingContext repositoryContext) : base(repositoryContext)
        {
        }

		public void Save(ClientLoans clientLoan)
		{
			Create(clientLoan);
			SaveChanges();
		}
	}
}
