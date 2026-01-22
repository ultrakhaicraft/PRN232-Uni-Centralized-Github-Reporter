using GithubReporterRepository.EntityModel;
using GithubReporterRepository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubReporterRepository.Core
{
	public class AccountRepository : IAccountRepository
	{
		private InMemoryUserStoreTemp _userStore;

		public AccountRepository(InMemoryUserStoreTemp userStore)
		{
			_userStore = userStore;
		}

		public async Task<Account?> GetByEmailMockAsync(string email)
		{
			await Task.CompletedTask;
			Account user = _userStore.GetUserByEmail(email);
			if (user == null)
			{
				throw new Exception("User not found");
			}
			
			return user;
		}
	}
}
