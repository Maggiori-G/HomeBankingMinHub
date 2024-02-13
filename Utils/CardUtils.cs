using System.Text;

namespace HomeBankingMinHub.Utils
{
	public class CardUtils
	{
		public static string GenerateRandomNumber(int lenght)
		{
			Random random = new Random();
			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < lenght; i++)
			{
				sb.Append(random.Next(0, 9).ToString());
			}
			return sb.ToString();
		}


	}


}
