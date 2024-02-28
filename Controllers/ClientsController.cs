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
using Microsoft.CodeAnalysis.Elfie.Serialization;
using System.Web;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System.IO;
using Azure;


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

        [HttpGet("current/downloads")]
        public IActionResult GenerarPDF()
        {
            string email = User.FindFirst("Client")!=null ? User.FindFirst("Client").Value : string.Empty;
            if(email==string.Empty)
            {
                return StatusCode(401,"Email vacio");
            }
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),"resumen.pdf");

            string contenido = _clientService.GenerarInfoPDF(email);
            if(string.IsNullOrEmpty(contenido))
            {
                return StatusCode(401,"No se pudo generar el resumen de cuenta");
			}
			using (PdfWriter writer = new PdfWriter(filePath))
            {
                using (PdfDocument doc = new PdfDocument(writer))
                {
                    Document document = new Document(doc);
                    document.Add(new Paragraph(contenido));
                }
            }
            return StatusCode(200, File(filePath,"application/pdf","resumen.pdf"));
        }

        [HttpPost("current/cards")]
        public IActionResult CreateCard([FromBody] CreateCardDTO createCardDTO)
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty) 
                { 
                    return StatusCode(401, "Email vacio");
                }

                string response = _clientService.CreateClientCard(email, createCardDTO);
                if(response.Equals("Tarjeta creada con éxito"))
                {
                    return StatusCode(200, response);
                }
                else
                {
                    return StatusCode(401, response);
                }
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

                string response = _clientService.CreateAccount(email);
                if(response.Equals("Cuenta creada con éxito"))
                {
                    return StatusCode(200, response);
                }
                else
                {
                    return StatusCode(401, response);
                }
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

				var accountsDTO = _clientService.GetCurrentAccounts(email);
                if(accountsDTO == null)
                {
                    return null;
                }
                return Ok(accountsDTO);
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
				var clientsDTO = _clientService.GetAllClients();
                if (clientsDTO ==null)
                {
                    return StatusCode(404, "No se encontro la lista de clientes");
                }
				return StatusCode(200, clientsDTO);
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
                    return StatusCode(200, _clientService.GetById(id));
                }
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

                var clientDTO = _clientService.GetCurrentClient(email);
                if(clientDTO == null)
                {
                    return StatusCode(404, "Cliente no encontrado");
                }

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
                string response = _clientService.CreateClient(client);
                if(response.Equals("Cliente creado con éxito"))
                {
                    return StatusCode(200, response);
                }
                else
                {
                    return StatusCode(400, response);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
	}
}