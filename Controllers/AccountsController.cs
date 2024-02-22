using HomeBankingMindHub.Repositories;
using HomeBankingMinHub.DTOs;
using HomeBankingMinHub.Models;
using HomeBankingMinHub.Repositories;
using HomeBankingMinHub.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HomeBankingMinHub.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountsController:ControllerBase
	{
		private IAccountRepository _accountRepository;
		

		public AccountsController(IAccountRepository accountRepository)
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
					var newAccountDTO = new AccountDTO(account)
					{
						Id = account.Id,
						Number = account.Number,
						CreationDate = account.CreationDate,
						Balance = account.Balance,
						Transactions = account.Transactions.Select(transaction=> new TransactionDTO(transaction)).ToList(),
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

				var accountDTO = new AccountDTO(account);
				return Ok(accountDTO);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
	}
}
