using HomeBankingMinHub.DTOs;
using HomeBankingMinHub.Models;
using HomeBankingMinHub.Repositories;

namespace HomeBankingMinHub.Services
{
	public class AccountService:IAccountService
	{
		private IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public AccountDTO GetAccountById(long id)
		{
			var account = _accountRepository.FindById(id);
            if (account == null)
            {
                return null;
            }

			var accountDTO = new AccountDTO
            {
                Id = account.Id,
                Number = account.Number,
                CreationDate = account.CreationDate,
                Balance = account.Balance,
                Transactions = account.Transactions.Select(ac => new TransactionDTO
                {
                    Id = ac.Id,
                    Type = ac.Type.ToString(),
                    Amount = ac.Amount,
                    Description = ac.Description,
					Date = ac.Date,
					Account = ac.Account,
                }).ToList()
            };
			return accountDTO;
		}

		public IEnumerable<AccountDTO> GetAllAccounts()
		{
			var accounts = _accountRepository.GetAllAccounts();
			var accountsDTO = new List<AccountDTO>();
			if(accounts !=null)
			{
				foreach (Account account in accounts)
				{
					var newAccountDTO = new AccountDTO
					{
						Id = account.Id,
						Number = account.Number,
						CreationDate = account.CreationDate,
						Balance = account.Balance,
						Transactions = account.Transactions.Select(transaction=> new TransactionDTO
						{
							Id = transaction.Id,
							Type = transaction.Type.ToString(),
							Amount = transaction.Amount,
							Description = transaction.Description,
							Date = transaction.Date,
							Account = transaction.Account,
						}).ToList()
					};
					accountsDTO.Add(newAccountDTO);
				}
				return accountsDTO;
			}
			return null;
		}
	}
}
