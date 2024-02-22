using HomeBankingMinHub.DTOs;
using HomeBankingMinHub.Models;

namespace HomeBankingMinHub.Services
{
	public interface IClientService
	{
		public bool CreateCard(ClientDTO client, CreateCardDTO createCardDTO, out Card createdCard);
		public bool CreateAccount(string email, out Account account);
		public List<ClientDTO> GetAllClients();
		public ClientDTO GetClientById(long id);
		public ClientDTO GetCurrent(string email);
		public bool CreateClient(Client client, out Client createdClient);
		//public bool GetCurrentAccounts(string email, out List<AccountDTO> clientAccounts);


	}
}
