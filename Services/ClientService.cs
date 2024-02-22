using HomeBankingMindHub.dtos;
using HomeBankingMinHub.DTOs;
using HomeBankingMinHub.Models;
using HomeBankingMinHub.Repositories;
using HomeBankingMinHub.Utils;

namespace HomeBankingMinHub.Services
{
	public class ClientService:IClientService
	{

		private IClientRepository _clientRepository;
		private IAccountRepository _accountRepository;
		private ICardRepository _cardRepository;

		public ClientService(IClientRepository clientRepository,IAccountRepository accountRepository,ICardRepository cardRepository)
		{
			_clientRepository=clientRepository;
			_accountRepository=accountRepository;
			_cardRepository=cardRepository;
		}

		public bool CreateCard(ClientDTO client,CreateCardDTO createCardDTO,out Card createdCard)
		{
			if(client==null||createCardDTO==null)
			{
				createdCard=null;
				return false;
			}
			foreach(CardDTO card in client.Cards)
			{
				if(createCardDTO.Type==card.Type.ToString()&&createCardDTO.Color==card.Color.ToString())
				{
					createdCard=null;
					return false;
				}
			}
			var newCard = new Card(client,createCardDTO);
			_cardRepository.Save(newCard);
			createdCard=newCard;
			return true;

		}

		public bool CreateAccount(string email,out Account ac)
		{

			Client client = _clientRepository.FindByEmail(email);

			if(client==null)
			{
				ac=null;
				return false;
			}

			if(client.Accounts.Count>3)
			{
				ac=null;
				return false;
			}

			Account account = new Account(client.Id);
			if(account==null)
			{
				ac=null;
				return false;
			}

			_accountRepository.Save(account);
			ac=account;
			return true;
		}

		public bool GetCurrentAccounts(string email,out List<AccountDTO> clientAccounts)
		{
			Client client = _clientRepository.FindByEmail(email);
			if(client==null)
			{
				clientAccounts=null;
				return false;
			}

			var accounts = _accountRepository.GetAccountsByClient(client.Id);
			if(accounts==null)
			{
				clientAccounts=null;
				return false;
			}

			var accountsDTOs = new List<AccountDTO>();
			foreach(Account ac in accounts)
			{
				accountsDTOs.Add(new AccountDTO(ac));
			}

			clientAccounts=accountsDTOs;
			return true;
		}

		public List<ClientDTO> GetAllClients()
		{
			var clients = _clientRepository.GetAllClients();
			var clientsDTO = new List<ClientDTO>();

			foreach(Client client in clients)
			{
				clientsDTO.Add(new ClientDTO(client));
			}
			return clientsDTO;
		}

		public bool CreateClient(Client client,out Client createdClient)
		{
			if(String.IsNullOrEmpty(client.Email)||String.IsNullOrEmpty(client.Password)||String.IsNullOrEmpty(client.FirstName)||String.IsNullOrEmpty(client.LastName))
			{
				createdClient=null;
				return false;
			}

			Client user = _clientRepository.FindByEmail(client.Email);
			if(user==null)
			{
				createdClient=null;
				return false;
			}

			Client newClient = new Client(client);
			createdClient=newClient;
			_clientRepository.Save(newClient);
			return true;
		}

		public ClientDTO GetClientById(long id)
		{
			var client = _clientRepository.FindById(id);
			if(client==null)
			{
				throw new Exception("Cliente no encontrado");
			}
			return new ClientDTO(client);
		}


		public ClientDTO GetCurrent(string email)
		{
			Client client = _clientRepository.FindByEmail(email);
    
            return new ClientDTO(client);
		}
	}
}
