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

namespace HomeBankingMinHub.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class ClientsController:ControllerBase
	{
		private IClientRepository _clientRepository;

        public ClientsController(IClientRepository clientRepository)
        {
			_clientRepository = clientRepository;
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
                        Color= c.Color,
                        Cvv= c.Cvv,
                        FromDate= c.FromDate,
                        Number= c.Number,
                        ThruDate= c.ThruDate,
                        Type = c.Type
                    }).ToList()
                };
				return Ok(clientDTO);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

        /*
            Este http post fue hecho en una tarea anterior y quedo invalidado
        */

		//[HttpPost]
		//public IActionResult Post([FromBody] FormClientDTO formClientDTO)
		//{
		//	try
		//	{
		//		if(_clientRepository.FindByEmail(formClientDTO.Email) == null)
		//		{
		//			return Forbid();
		//		}

		//		var client = new Client
		//		{
		//			Email = formClientDTO.Email,
		//			FirstName = formClientDTO.FirstName,
		//			LastName = formClientDTO.LastName,
		//			Password = formClientDTO.Email + "pass" + formClientDTO.FirstName,
		//		};
		//		_clientRepository.Save(client);
		//		return Ok(client);
		//	}
		//	catch (Exception ex)
		//	{
		//		return StatusCode(500, ex.Message);
		//	}
		//}

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
                        CardHolder = c.CardHolder,
                        Color = c.Color,
                        Cvv = c.Cvv,
                        FromDate = c.FromDate,
                        Number = c.Number,
                        ThruDate = c.ThruDate,
                        Type = c.Type
                    }).ToList()
                };

                return Ok(clientDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
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
                    Password = client.Password,
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