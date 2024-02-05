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

                //guardamos
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
        }
    }
}
