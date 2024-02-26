using HomeBankingMinHub.DTOs;
using HomeBankingMinHub.Intefaces;
using HomeBankingMinHub.Models;
using HomeBankingMinHub.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace HomeBankingMinHub.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LoansController:ControllerBase
	{
        private readonly ILoansService _loansService;
		public LoansController(ILoansService loansService)
		{
			_loansService = loansService;
		}

		[HttpGet]
        public IActionResult Get()
        {
            try
            {
                
                var loansDTO = _loansService.GetAllLoans();
                if(loansDTO == null)
                {
                    return StatusCode(404, "No se encontraron los prestamos");
                }
                return Ok(loansDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] LoanApplicationDTO loanApplicationDTO)
        {
            try
            {
                string email = User.FindFirst("Client").Value;
                if (!email.IsNullOrEmpty())
                {
                    string response = _loansService.CreateLoanApplication(loanApplicationDTO, email);
                    if (response.Equals("Prestamo Aprobado"))
                    {
                        return StatusCode(200, response);
                    }
                    else
                    {
                        return StatusCode(403, response);
                    }
                }
                else
                {
                    return StatusCode(401, "No se pudo procesar el prestamo");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



	}
}
