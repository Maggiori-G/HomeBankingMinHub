﻿using HomeBankingMindHub.dtos;
using HomeBankingMinHub.DTOs;
using HomeBankingMinHub.Models;
using HomeBankingMinHub.Repositories;
using HomeBankingMinHub.Utils;

namespace HomeBankingMinHub.Services
{
	public class ClientService:IClientService
	{
		private readonly IClientRepository _clientRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ICardRepository _cardRepository;

        public ClientService(IClientRepository clientRepository, IAccountRepository accountRepository, ICardRepository cardRepository)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _cardRepository = cardRepository;
        }
        
		public string CreateClientCard(string email, CreateCardDTO createCardDTO)
		{
			Client client = _clientRepository.FindByEmail(email);
            if (client == null)
            {
                return "Cliente no encontrado";
            }

            foreach(Card card in client.Cards)
            {
                if(createCardDTO.Type == card.Type.ToString() && createCardDTO.Color == card.Color.ToString())
                {
                    return $"No permitido, ya posee una tarjeta de {createCardDTO.Type} y de color {createCardDTO.Color}";
                }
            }

            Card newCard = new Card
            {
                CardHolder = $"{client.FirstName} {client.LastName}",
                ClientId = client.Id,
                FromDate = DateTime.Now,
                ThruDate = DateTime.Now.AddYears(6),
                Type = (CardType)Enum.Parse(typeof(CardType), createCardDTO.Type),
                Color = (CardColor)Enum.Parse(typeof(CardColor), createCardDTO.Color),
                Number=CardUtils.GenerateRandomNumber(16),
                Cvv=int.Parse(CardUtils.GenerateRandomNumber(3))
            };

            _cardRepository.Save(newCard);

            return "Tarjeta creada con éxito";
		}
        
		public string CreateAccount(string email)
		{
			
			Client client = _clientRepository.FindByEmail(email);

            if (client == null)
            {
                return "Cliente no encontrado";
            }

			if(client.Accounts.Count > 3)
			{
				return "Maximo de cuentas alcanzado";
			}

			Account account = new Account
			{
				Number = ClientUtils.GeneradorDeNumerosParaCuentas(),
				CreationDate = DateTime.Now,
				Balance = 0,
				ClientId = client.Id,
			};
			_accountRepository.Save(account);
            return "Cuenta creada con éxito";
		}
        
		public IEnumerable<AccountDTO> GetCurrentAccounts(string email)
		{
			Client client = _clientRepository.FindByEmail(email);

            if (client == null)
            {
                return null;
            }

            var accounts = _accountRepository.GetAccountsByClient(client.Id);
            if (accounts == null)
            {
                return null;
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
            return accountsDTOs;
		}

		public IEnumerable<ClientDTO> GetAllClients()
		{
			var clients = _clientRepository.GetAllClients();
            if(clients == null)
            {
                return null;
            }

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
            return clientsDTO;

		}

        public ClientDTO GetById(long id)
		{
			var client = _clientRepository.FindById(id);
            if (client == null)
            {
                return null;
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
            return clientDTO;
		}

		public ClientDTO GetCurrentClient(string email)
		{
            Client client = _clientRepository.FindByEmail(email);

            if(client==null)
            {
                return null;
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

            return clientDTO;
		}

		public string CreateClient(Client client)
		{
			if (String.IsNullOrEmpty(client.Email) || String.IsNullOrEmpty(client.Password) || String.IsNullOrEmpty(client.FirstName) ||String.IsNullOrEmpty(client.LastName))
            {
                return "Datos inválidos";
            }
            Client user = _clientRepository.FindByEmail(client.Email);
            if (user != null)
            {
                return "Email está en uso";
            }

            Client newClient = new Client
            {
                Email = client.Email,
                Password = ClientUtils.HashPassword(client.Password),
                FirstName = client.FirstName,
                LastName = client.LastName,
            };
            _clientRepository.Save(newClient);

            return "Cliente creado con éxito";
		}
	}
}