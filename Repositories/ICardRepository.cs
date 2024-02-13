using HomeBankingMinHub.Models;

namespace HomeBankingMinHub.Repositories
{
	public interface ICardRepository
	{
		public Card FindById(long id);
		public IEnumerable<Card> GetAllCards();
		public void Save(Card card);
	}
}
