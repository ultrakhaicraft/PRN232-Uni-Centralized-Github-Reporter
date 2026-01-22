using GithubReporterRepository.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubReporterRepository
{
	public class InMemoryUserStoreTemp
	{
		private static List<Account> _accounts = new List<Account>
		{
			new Account { Email = "test@example.com", PasswordHash = "$2a$12$Yi5aB7iX5j9l5u0YFwki5u2TM7C8TRESY5Qb.LREl.yjg8lv1IFIq" }, // Pass: "based123"
			new Account { Email = "admin@example.com", PasswordHash = "$2a$12$Ipas1Zx/3rDsnqpH.4vhsuZcny64qrDlOcOgxWOefZMS57jjOeWES" } // Pass: "Epic832"
		};

		public Account? GetUserByEmail(string email)
		{
			return _accounts.FirstOrDefault(u => u.Email == email);
		}
	}
}
