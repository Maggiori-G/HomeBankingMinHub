using HomeBankingMinHub.DTOs;
using HomeBankingMinHub.Utils;

namespace HomeBankingMinHub.Models
{
	public class Card
	{
		public int Id { get; set; }
		public string CardHolder { get; set; }
		public CardType Type { get; set; }
		public CardColor Color { get; set; }
		public string Number { get; set; }
		public int Cvv { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ThruDate { get; set; }
        public long ClientId { get; set; }

        public Card()
        {
            
        }

        public Card(long id, string firstName, string lastName, CardType type, CardColor color, string number, int cvv)
        {
            ClientId = id;
            CardHolder = firstName + " " + lastName; 
            Type = CardType.DEBIT;
            Color = CardColor.GOLD;
            Number = "3325-6745-7876-4445";
            Cvv = 990;
            FromDate= DateTime.Now;
            ThruDate= DateTime.Now.AddYears(4);
        }

        public Card(ClientDTO client, CreateCardDTO createCardDTO)
        {
            CardHolder = $"{client.LastName} {client.FirstName}";
            ClientId = client.Id;
            FromDate = DateTime.Now;
            ThruDate = DateTime.Now.AddYears(6);
            Type = (CardType)Enum.Parse(typeof(CardType), createCardDTO.Type);
            Color = (CardColor)Enum.Parse(typeof(CardColor), createCardDTO.Color);
            Number=CardUtils.GenerateRandomNumber(16);
            Cvv=int.Parse(CardUtils.GenerateRandomNumber(3));
        }
    }
}
