using HomeBankingMindHub.Repositories;
using HomeBankingMinHub.DTOs;
using HomeBankingMinHub.Intefaces;
using HomeBankingMinHub.Models;
using HomeBankingMinHub.Repositories;

namespace HomeBankingMinHub.Services
{
	public class LoanService:ILoansService
	{
		private IClientRepository _clientRepository;
        private IAccountRepository _accountRepository;
        private ITransactionRepository _transactionRepository;
		private ILoanRepository _loanRepository;
		private IClientLoanRepository _clientLoanRepository;

        public LoanService(IClientRepository clientRepository,IAccountRepository accountRepository,ITransactionRepository transactionRepository,ILoanRepository loanRepository,IClientLoanRepository clientLoanRepository)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository; 
            _transactionRepository = transactionRepository;
            _loanRepository = loanRepository;
            _clientLoanRepository = clientLoanRepository;
        }

		public IEnumerable<LoanDTO> GetAllLoans()
		{
			var loans = _loanRepository.GetAll();
            var loansDTO = new List<LoanDTO>();
            if(loans !=null)
            {
                foreach (Loan loan in loans)
                {
                    loansDTO.Add(new LoanDTO(loan));
                }
                return loansDTO;
            }
            return null;
		}

		public string CreateLoanApplication(LoanApplicationDTO loanApplicationDTO, string email)
		{
            Client client = _clientRepository.FindByEmail(email);
            if (client == null)
            {
                return "No existe el cliente";
            }

            var account = _accountRepository.FindByNumber(loanApplicationDTO.ToAccountNumber);

            if (account == null)
            {
                return "La cuenta de destino no existe";
            }

            var loan = _loanRepository.FindById(loanApplicationDTO.LoanId);
            if (loan == null)
            {
                return "Prestamo no encontrado";
            }

            if(loanApplicationDTO.Amount <= 100 || loanApplicationDTO.Amount > loan.MaxAmount)
            {
                return $"Error, el monto minimo es de 100$ y no puede sobrepasar {loan.MaxAmount}";
            }

            if(string.IsNullOrEmpty(loanApplicationDTO.Payments))
            {
                return "Debe indicar la cantidad de cuotas";
            }
                
            if(account.ClientId != client.Id)
            {
                return "La cuenta no pertenece al cliente";
            }

            var loans = _loanRepository.GetAll();
            if (loans != null) 
            {
                foreach(Loan loanDB in loans)
                {
                    if(loanDB.Id == loan.Id)
                    {
                        if(!loanDB.Payments.Split(',').Contains(loanApplicationDTO.Payments))
                        {
                            return "Cantidad de cuotas no permitidas";
                        }
                    }
                }
            }

            var clientLoan = new ClientLoans()
            {
                Amount = loanApplicationDTO.Amount * 1.20,
                Payments = loanApplicationDTO.Payments,
                ClientId = client.Id,
                LoanId = loanApplicationDTO.LoanId,
            };

            _clientLoanRepository.Save(clientLoan);

            var transaction = new Transaction()
            {
                Type = TransactionType.CREDIT,
                Amount = loanApplicationDTO.Amount,
                Description = $"{loan.Name} Loan Approved",
                Date = DateTime.Now,
                AccountId = account.Id,
            };

            _transactionRepository.Save(transaction);

            account.Balance = account.Balance + loanApplicationDTO.Amount;
            _accountRepository.Save(account);

            return "Prestamo Aprobado";
		}
	}
}
