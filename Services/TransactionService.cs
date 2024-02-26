using HomeBankingMindHub.dtos;
using HomeBankingMindHub.Repositories;
using HomeBankingMinHub.DTOs;
using HomeBankingMinHub.Models;
using HomeBankingMinHub.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace HomeBankingMinHub.Services
{
	public class TransactionService:ITransactionService
	{
		private IClientRepository _clientRepository;
        private IAccountRepository _accountRepository;
        private ITransactionRepository _transactionRepository;

        public TransactionService(IClientRepository clientRepository, IAccountRepository accountRepository, ITransactionRepository transactionRepository)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
        }

		public string GenerarTransaccion(TransferDTO transferDTO)
		{
			if (transferDTO.FromAccountNumber.IsNullOrEmpty() || transferDTO.ToAccountNumber.IsNullOrEmpty())
            {
                return "Cuenta de origen o cuenta de destino no proporcionada.";
            }

            //Client client = _clientRepository.FindByEmail(email);

                //if (client == null)
                //{
                //    return Forbid("No existe el cliente");
                //}

            if (transferDTO.FromAccountNumber == transferDTO.ToAccountNumber)
            {
                return "No se permite la transferencia a la misma cuenta.";
            }

            if (transferDTO.Amount < 0 || transferDTO.Description == string.Empty)
            {
                return "Monto o descripción no proporcionados.";
            }

            Account fromAccount = _accountRepository.FindByNumber(transferDTO.FromAccountNumber);
            if (fromAccount == null)
            {
                return "Cuenta de origen no existe";
            }

            Account toAccount = _accountRepository.FindByNumber(transferDTO.ToAccountNumber);
            if (toAccount == null)
            {
                return "Cuenta de destino no existe";
            }

            if (fromAccount.Balance < transferDTO.Amount)
            {
                return "Fondos insuficientes";
            }

            _transactionRepository.Save(new Transaction
            {
                Type = TransactionType.DEBIT,
                Amount = transferDTO.Amount * -1,
                Description= transferDTO.Description + " " + toAccount.Number,
                AccountId = fromAccount.Id,
                Date= DateTime.Now,
            });

            _transactionRepository.Save(new Transaction
            {
                Type= TransactionType.CREDIT,
                Amount = transferDTO.Amount,
                Description= transferDTO.Description + " " + fromAccount.Number,
                AccountId = toAccount.Id,
                Date= DateTime.Now,
            });

            fromAccount.Balance = fromAccount.Balance - transferDTO.Amount;
            _accountRepository.Save(fromAccount);

            toAccount.Balance = toAccount.Balance + transferDTO.Amount;
            _accountRepository.Save(toAccount);

			return "Transferencia Realizada con éxito";
		}
	}
}
