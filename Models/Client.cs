using HomeBankingMinHub.Utils;
using System.ComponentModel.DataAnnotations;

namespace HomeBankingMinHub.Models
{
	public class Client
	{
		public long Id { get; set; }
        public string FirstName { get; set;}
        public string LastName { get; set;}
        public string Email { get; set;}
        public string Password { get; set;}
		public ICollection<Account> Accounts { get; set; }
        public ICollection<ClientLoans> ClientLoans { get; set; }
        public ICollection<Card> Cards { get; set; }

        public Client()
        {
            
        }
        public Client(string email, string firstName, string lastname, string password)
        {
            Email = email;
            FirstName = firstName;
            LastName = lastname;
            Password = password;
        }

        public Client(Client client)
        {
            Email = client.Email;
            Password = ClientUtils.HashPassword(client.Password);
            FirstName = client.FirstName;
            LastName = client.LastName;
        }
    }
}
