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
				if (lenght > 4 && i % 4 == 0 && i != 0)
				{
					sb.Append("-");
				}
				sb.Append(random.Next(1, 9).ToString());
			}
			return sb.ToString();
		}
	}
}
