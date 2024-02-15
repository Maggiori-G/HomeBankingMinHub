using HomeBankingMinHub.Models;

namespace HomeBankingMinHub.Repositories
{
	public interface IAccountRepository
	{
		IEnumerable<Account> GetAllAccounts();
        void Save(Account account);
        Account FindById(long id);
		IEnumerable<Account> GetAccountsByClient(long clientId);
		Account FinByNumber(string number);
	}
}