using HomeBankingMinHub.DTOs;
using HomeBankingMinHub.Models;

namespace HomeBankingMinHub.Services
{
	public interface IClientService
	{
		public string GenerarInfoPDF(string email);
		public string CreateClientCard(string email, CreateCardDTO createCardDTO);
		public string CreateAccount(string email);
		public IEnumerable<AccountDTO> GetCurrentAccounts(string email);
		public IEnumerable<ClientDTO> GetAllClients();
		public ClientDTO GetById(long id);
		public ClientDTO GetCurrentClient(string email);
		public string CreateClient(Client client);
	}
}
