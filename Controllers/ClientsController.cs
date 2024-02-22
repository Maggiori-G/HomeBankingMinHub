using Microsoft.AspNetCore.Http;
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
using HomeBankingMinHub.Services;

namespace HomeBankingMinHub.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class ClientsController:ControllerBase
	{
		private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpPost("current/cards")]
        public IActionResult CreateCard([FromBody] CreateCardDTO createCardDTO)
        {
            try
            {
                ClientDTO client = _clientService.GetCurrent(User.FindFirst("Client").Value);
                if (client == null)
                {
                    return StatusCode(404, "Cliente no encontrado");
                }             

                Card newCard = null;
                if(!_clientService.CreateCard(client, createCardDTO, out newCard))
                {
                    newCard = null;
                    return StatusCode(401, "Surgio un problema al crear la nueva tarjeta, comuniquese con atencion al cliente o intente de nuevo");
                }
                
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
                
                Account account = null;
                if(!_clientService.CreateAccount(email, out account))
                {
                    return StatusCode(400, "Hubo un error al crear la cuenta");
                }
                return StatusCode(200, account);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

        //[HttpGet("current/accounts")]
        //public IActionResult GetCurrentAccounts()
        //{
        //    try
        //    {
        //        string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
        //        if (email == string.Empty) 
        //        { 
        //            return StatusCode(401, "Email vacio");
        //        }
        //        List<AccountDTO> clientAccounts = null;
        //        if(!_clientService.GetCurrentAccounts(email,out clientAccounts))
        //        {
        //            return StatusCode(404,"Cliente no encontrado o cuentas inexistentes");
        //        }

        //        return StatusCode(200, clientAccounts);
        //    }
        //    catch(Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }

        //}

		[HttpGet]
		public IActionResult GetAllClients()
		{
			try
			{
                var clients = _clientService.GetAllClients();
                if(clients == null)
                {
                    return StatusCode(404, "Lista de clientes no encontrada");
                }
				return Ok(clients);
			}	
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("{id}")]
		public IActionResult GetClientById(long id)
		{
			try
			{
				var client = _clientService.GetClientById(id);
                if (client == null)
                {
                    return StatusCode(404, "Cliente no encontrado");
                }
				return Ok(client);
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
                    return StatusCode(401, email);
                }

                ClientDTO currentClient = _clientService.GetCurrent(email);
                if (currentClient == null)
                {
                    return StatusCode(404, "Cliente no encontrado");
                }
                return StatusCode(200, currentClient);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [HttpPost("manager")]
        public IActionResult CreateClient([FromBody] Client client)
        {
            try
            {
                if (String.IsNullOrEmpty(client.Email) || String.IsNullOrEmpty(client.Password) || String.IsNullOrEmpty(client.FirstName) ||String.IsNullOrEmpty(client.LastName))
                {
                    return StatusCode(403, "Datos inválidos");
                }
                Client newClient = null;
                if(!_clientService.CreateClient(client, out newClient))
                {
                    return StatusCode(401, "Datos invalidos o email en uso, por favor chequee");
                }
                return StatusCode(200, newClient);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
	}
}