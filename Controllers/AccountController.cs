using HomeBankingMindHub.Repositories;
using HomeBankingMinHub.DTOs;
using HomeBankingMinHub.Models;
using HomeBankingMinHub.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HomeBankingMinHub.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController:ControllerBase
	{
		private IAccountRepository _accountRepository;

		public AccountController(IAccountRepository accountRepository)
        {
			_accountRepository = accountRepository;
        }

		[HttpGet]
		public IActionResult Get()
		{
			try
			{
				var accounts = _accountRepository.GetAllAccounts();
				var accountsDTO = new List<AccountDTO>();

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
                            Type = transaction.Type,
                            Ammount = transaction.Ammount,
                            Description = transaction.Description,
							Date = transaction.Date,
							Account = transaction.Account,
                        }).ToList()
                    };
					accountsDTO.Add(newAccountDTO);
				}
				return Ok(accountsDTO);
			}	
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("{id}")]
		public IActionResult Get(long id)
		{
			try
			{
				var account = _accountRepository.FindById(id);
                if (account == null)
                {
                    return Forbid();
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
                        Type = ac.Type,
                        Ammount = ac.Ammount,
                        Description = ac.Description,
						Date = ac.Date,
						Account = ac.Account,
                    }).ToList()
                };
				return Ok(accountDTO);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
	}
}
