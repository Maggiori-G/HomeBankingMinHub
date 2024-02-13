using System.Security.Cryptography;
using System.Text;

namespace HomeBankingMinHub.Utils
{
	public class ClientUtils
	{
		public static string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2")); // x2 formatea a hexadecimal
                }
                return builder.ToString();
            }
        }

        public static string GeneradorDeNumerosParaCuentas()
        {
            string vin = "VIN-";
            Random random = new Random();

            long numeroDeCuenta = random.Next(0,99999999);

            return vin + numeroDeCuenta.ToString();
        }
	}

}
