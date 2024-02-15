using HomeBankingMinHub.Models;

namespace HomeBankingMindHub.Repositories
{
    public interface ITransactionRepository
    {
        void Save(Transaction transaction);
        Transaction FindByNumber(long id);
    }
}