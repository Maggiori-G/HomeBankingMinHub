﻿using HomeBankingMindHub.Repositories;
using HomeBankingMinHub.DTOs;
using HomeBankingMinHub.Models;
using HomeBankingMinHub.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeBankingMinHub.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LoansController:ControllerBase
	{
		private IClientRepository _clientRepository;
        private IAccountRepository _accountRepository;
        private ITransactionRepository _transactionRepository;
		private ILoanRepository _loanRepository;
		private IClientRepository _clientLoanRepository;

		public LoansController(IClientRepository clientRepository,IAccountRepository accountRepository,ITransactionRepository transactionRepository,ILoanRepository loanRepository,IClientRepository clientLoanRepository)
		{
			_clientRepository=clientRepository;
			_accountRepository=accountRepository;
			_transactionRepository=transactionRepository;
			_loanRepository=loanRepository;
			_clientLoanRepository=clientLoanRepository;
		}

		[HttpGet]
        public IActionResult Get()
        {
            try
            {
                var loans = _loanRepository.GetAll();
                var loansDTO = new List<LoanDTO>();

                foreach (Loan loan in loans)
                {
                    loansDTO.Add(new LoanDTO(loan));
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
                //verifica que exista el cliente
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return StatusCode(401, "Email vacío");
                }

                Client client = _clientRepository.FindByEmail(email);
                if (client == null)
                {
                    return StatusCode(401, "No existe el cliente");
                }
                //verifica si existe el prestamo
                var loan = _loanRepository.FindById(loanApplicationDTO.LoanId);
                if (loan == null)
                {
                    return StatusCode(403, "Prestamo no encontrado");
                }
                //valida la cantidad minima y maxima del prestamo
                if(loanApplicationDTO.Amount <= 100 || loanApplicationDTO.Amount > loan.MaxAmount)
                {
                    return StatusCode(403, $"Error, el monto minimo es de 100$ y no puede sobrepasar {loan.MaxAmount}");
                }
                //valida que payments no venga vacio
                if(string.IsNullOrEmpty(loanApplicationDTO.Payments))
                {
                    return StatusCode(403, "Debe indicar la cantidad de cuotas");
                }
                //valida que la cuenta de destino exista
                var account = _accountRepository.FindByNumber(loanApplicationDTO.ToAccountNumber);

                if (account == null)
                {
                    return StatusCode(403, "La cuenta de destino no existe");
                }
                //verifica que la cuenta de destino sea del cliente autenticado
                if(account.ClientId != client.Id)
                {
                    return StatusCode(403, "La cuenta no pertenece al cliente");
                }


                return Ok(loanApplicationDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



	}
}