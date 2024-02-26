using HomeBankingMindHub.dtos;
using HomeBankingMinHub.DTOs;

namespace HomeBankingMinHub.Services
{
	public interface ITransactionService
	{
		public string GenerarTransaccion(TransferDTO transferDTO);
	}
}
