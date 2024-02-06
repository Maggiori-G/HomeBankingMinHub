namespace HomeBankingMinHub.Models
{
	public class DBInitializer
    {
        public static void Initialize(HomeBankingContext context)
        {
            if (!context.Clients.Any())
            {
                var clients = new Client[]
                {
                    new Client { Email = "vcoronado@gmail.com", FirstName="Victor", LastName="Coronado", Password="123456"},
                    new Client { Email = "tanito@ejemplo.com", FirstName="Gianni", LastName="Maggiori", Password="@12345"},
                    new Client { Email = "ejemplo2@otroejemplo.com", FirstName="404NotFound", LastName="Non Existent", Password="@!123"},
                    new Client { Email = "elchompi@hotmail.com", FirstName="El Chompiras", LastName="Esposito", Password="elchavo"}
                };
                context.Clients.AddRange(clients);
                context.SaveChanges();
            }

            if (!context.Accounts.Any())
            {
                var accountVictor = context.Clients.FirstOrDefault(c => c.Email == "vcoronado@gmail.com");
                if (accountVictor != null)
                {
                    var accounts = new Account[]
                    {
                        new Account {ClientId = accountVictor.Id, CreationDate = DateTime.Now, Number = string.Empty, Balance = 0 }
                    };
                    context.Accounts.AddRange(accounts);
                    context.SaveChanges();
                }
            }

            if(!context.Transactions.Any())
            {
                var account = context.Accounts.FirstOrDefault(c => c.Number == "VIN001");

                if(account !=null)
                {
                    var transaction = new Transaction[]
                    {
                        new Transaction { AccountId= account.Id, Ammount = 10000, Date= DateTime.Now.AddHours(-5), Description = "Transferencia reccibida", Type = TransactionType.CREDIT.ToString() },
                        new Transaction { AccountId= account.Id, Ammount = -2000, Date= DateTime.Now.AddHours(-6), Description = "Compra en tienda mercado libre", Type = TransactionType.DEBIT.ToString() },
                        new Transaction { AccountId= account.Id, Ammount = -3000, Date= DateTime.Now.AddHours(-7), Description = "Compra en tienda xxxx", Type = TransactionType.DEBIT.ToString() },

                    };

                    context.Transactions.AddRange(transaction);
                    context.SaveChanges();
                }
            }
        }
    }
}
