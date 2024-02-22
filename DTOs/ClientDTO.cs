using HomeBankingMindHub.dtos;
using HomeBankingMinHub.Models;
using System.Collections.Generic;
using System.Text.Json.Serialization;
namespace HomeBankingMinHub.DTOs
{
    public class ClientDTO
    {
        [JsonIgnore]
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public ICollection<AccountDTO> Accounts { get; set; }
        public ICollection<ClientLoanDTO> Credits { get; set; }
        public ICollection<CardDTO> Cards { get; set; }

        public ClientDTO()
        {
            
        }

        public ClientDTO(Client client)
        {
            Id = client.Id;
            Email = client.Email;
            FirstName = client.FirstName;
            LastName = client.LastName;
            Accounts = client.Accounts.Select(ac => new AccountDTO(ac)).ToList();
            Credits = client.ClientLoans.Select(cl => new ClientLoanDTO(cl)).ToList();
            Cards = client.Cards.Select(c => new CardDTO(c)).ToList();
        }
    }
}
