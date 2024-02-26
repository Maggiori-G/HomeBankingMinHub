using HomeBankingMinHub.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HomeBankingMinHub.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountsController:ControllerBase
	{
		private IAccountService _accountService;
		
		public AccountsController(IAccountService accountService)
        {
			_accountService = accountService;
        }

		[HttpGet]
		public IActionResult Get()
		{
			try
			{
				var accountsDTO = _accountService.GetAllAccounts();
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
				if(id <= 0)
				{
					return StatusCode(400, "ID invalido");
				}
				else
				{
					var accountDTO = _accountService.GetAccountById(id);
					if(accountDTO ==null)
					{
						return StatusCode(404, "Cuenta no encontrada");
					}
					else
					{
						return Ok(accountDTO);
					}
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
	}
}
