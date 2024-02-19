﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HomeBankingMindHub.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using HomeBankingMinHub.Repositories;
using HomeBankingMinHub.Models;
using HomeBankingMinHub.DTOs;
using HomeBankingMindHub.dtos;
using HomeBankingMinHub.Utils;

namespace HomeBankingMinHub.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class ClientsController:ControllerBase
	{
		private IClientRepository _clientRepository;
        private IAccountRepository _accountRepository;
        private ICardRepository _cardRepository;

        public ClientsController(IClientRepository clientRepository, IAccountRepository accountRepository, ICardRepository cardRepository)
        {
			_clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _cardRepository = cardRepository;
        }

        [HttpPost("current/cards")]
        public IActionResult CreateCard([FromBody] CreateCardDTO createCardDTO)
        {
            try
            {
                Client client = _clientRepository.FindByEmail(User.FindFirst("Client").Value);
                if (client == null)
                {
                    return StatusCode(404, "Cliente no encontrado");
                }

                foreach(Card card in client.Cards)
                {
                    if(createCardDTO.Type == card.Type.ToString() && createCardDTO.Color == card.Color.ToString())
                    {
                        return StatusCode(404, $"No permitido, ya posee una tarjeta de {createCardDTO.Type} y de color {createCardDTO.Color}");
                    }
                }

                Card newCard = new Card
                {
                    CardHolder = $"{client.LastName} {client.FirstName}",
                    ClientId = client.Id,
                    FromDate = DateTime.Now,
                    ThruDate = DateTime.Now.AddYears(6),
                    Type = (CardType)Enum.Parse(typeof(CardType), createCardDTO.Type),
                    Color = (CardColor)Enum.Parse(typeof(CardColor), createCardDTO.Color),
                    Number=CardUtils.GenerateRandomNumber(16),
                    Cvv=int.Parse(CardUtils.GenerateRandomNumber(3))
                };

                _cardRepository.Save(newCard);

                return StatusCode(200, newCard);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("current/accounts")]
		public IActionResult CreateAccount()
		{
			try
			{
				string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty) 
                { 
                    return StatusCode(401, "Email vacio");
                }

				Client client = _clientRepository.FindByEmail(email);

                if (client == null)
                {
                    return StatusCode(404, "Cliente no encontrado");
                }

				if(client.Accounts.Count > 3)
				{
					return StatusCode(403, "Maximo de cuentas alcanzado");
				}

				Account account = new Account
				{
					Number = ClientUtils.GeneradorDeNumerosParaCuentas(),
					CreationDate = DateTime.Now,
					Balance = 0,
					ClientId = client.Id,
				};

				_accountRepository.Save(account);
                //client.Accounts.Add(account);
                return StatusCode(201, "Cuenta creada con exito");
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

        [HttpGet("current/accounts")]
        public IActionResult GetCurrentAccount()
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty) 
                { 
                    return StatusCode(401, "Email vacio");
                }

				Client client = _clientRepository.FindByEmail(email);

                if (client == null)
                {
                    return StatusCode(404, "Cliente no encontrado");
                }

                var accounts = _accountRepository.GetAccountsByClient(client.Id);
                if (accounts == null)
                {
                    return StatusCode(404, "No se encontraron las cuentas de los clientes");
                }

                var accountsDTOs = new List<AccountDTO>();
                foreach (Account ac in accounts)
                {
                    accountsDTOs.Add(new AccountDTO()
                    {
                        Balance = ac.Balance,
                        CreationDate = ac.CreationDate,
                        Id = ac.Id,
                        Number = ac.Number
                    });
                }

                return Ok(accountsDTOs);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }


		[HttpGet]
		public IActionResult Get()
		{
			try
			{
				var clients = _clientRepository.GetAllClients();
				var clientsDTO = new List<ClientDTO>();

				foreach (Client client in clients)
				{
					var newClientDTO = new ClientDTO
					{
						Id = client.Id,
						Email = client.Email,
						FirstName = client.FirstName,
						LastName = client.LastName,
						Accounts = client.Accounts.Select(ac=> new AccountDTO
                        {
                            Id = ac.Id,
                            Balance = ac.Balance,
                            CreationDate = ac.CreationDate,
                            Number = ac.Number
                        }).ToList(),
						Credits = client.ClientLoans.Select(cl => new ClientLoanDTO
						{
							Id = cl.Id,
							LoanId = cl.LoanId,
							Name = cl.Loan.Name,
							Amount = cl.Amount,
							Payments = int.Parse(cl.Payments)
						}).ToList()

                    };
					clientsDTO.Add(newClientDTO);
				}
				return Ok(clientsDTO);
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
				var client = _clientRepository.FindById(id);
                if (client == null)
                {
                    return Forbid();
                }

				var clientDTO = new ClientDTO
                {
                    Id = client.Id,
                    Email = client.Email,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    Accounts = client.Accounts.Select(ac => new AccountDTO
                    {
                        Id = ac.Id,
                        Balance = ac.Balance,
                        CreationDate = ac.CreationDate,
                        Number = ac.Number
                    }).ToList(),
					Credits = client.ClientLoans.Select(cl => new ClientLoanDTO
                    {
                        Id = cl.Id,
                        LoanId = cl.LoanId,
                        Name = cl.Loan.Name,
                        Amount = cl.Amount,
                        Payments = int.Parse(cl.Payments)
                    }).ToList(),
					Cards = client.Cards.Select(c => new CardDTO
                    {
                        Id = c.Id,
                        CardHolder= c.CardHolder,
                        Color= c.Color.ToString(),
                        Cvv= c.Cvv,
                        FromDate= c.FromDate,
                        Number= c.Number,
                        ThruDate= c.ThruDate,
                        Type = c.Type.ToString()
                    }).ToList()
                };
				return Ok(clientDTO);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}


		[HttpGet("current")]
        public IActionResult GetCurrent()
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty) 
                { 
                    return Forbid();
                }

                Client client = _clientRepository.FindByEmail(email);

                if (client == null)
                {
                    return StatusCode(404, "Cliente no encontrado");
                }

                var clientDTO = new ClientDTO
                {
                    Id = client.Id,
                    Email = client.Email,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    Accounts = client.Accounts.Select(ac => new AccountDTO
                    {
                        Id = ac.Id,
                        Balance = ac.Balance,
                        CreationDate = ac.CreationDate,
                        Number = ac.Number
                    }).ToList(),
                    Credits = client.ClientLoans.Select(cl => new ClientLoanDTO
                    {
                        Id = cl.Id,
                        LoanId = cl.LoanId,
                        Name = cl.Loan.Name,
                        Amount = cl.Amount,
                        Payments = int.Parse(cl.Payments)
                    }).ToList(),
                    Cards = client.Cards.Select(c => new CardDTO
                    {
                        Id = c.Id,
                        CardHolder = c.CardHolder,
                        Color = c.Color.ToString(),
                        Cvv = c.Cvv,
                        FromDate = c.FromDate,
                        Number = c.Number,
                        ThruDate = c.ThruDate,
                        Type = c.Type.ToString()
                    }).ToList()
                };

                return Ok(clientDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("manager")]
        public IActionResult Post([FromBody] Client client)
        {
            try
            {
                if (String.IsNullOrEmpty(client.Email) || String.IsNullOrEmpty(client.Password) || String.IsNullOrEmpty(client.FirstName) ||String.IsNullOrEmpty(client.LastName))
                {
                    return StatusCode(403, "datos inválidos");
                }
                Client user = _clientRepository.FindByEmail(client.Email);
                if (user != null)
                {
                    return StatusCode(403, "Email está en uso");
                }

                Client newClient = new Client
                {
                    Email = client.Email,
                    Password = ClientUtils.HashPassword(client.Password),
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                };
                _clientRepository.Save(newClient);
                return Created("", newClient);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
	}
}