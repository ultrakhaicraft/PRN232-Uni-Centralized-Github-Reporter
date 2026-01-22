using GithubReporterRepository.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubReporterRepository.Interface
{
	public interface IAccountRepository
	{
		public Task<Account?> GetByEmailMockAsync(string email);
	}
}
