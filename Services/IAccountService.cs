using HomeBankingMinHub.DTOs;
using HomeBankingMinHub.Models;

namespace HomeBankingMinHub.Services
{
	public interface IAccountService
	{
		public IEnumerable<AccountDTO> GetAllAccounts();
		public AccountDTO GetAccountById(long id);
	}
}
