using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubReporterRepository.Interface
{
	public interface IUnitOfWork : IDisposable
	{
		IGenericRepository<T> Repository<T>() where T : class;

		Task<IDbContextTransaction> BeginTransactionAsync();
		Task CommitAsync();
		Task RollbackAsync();

		Task<int> SaveChangesAsync();
		int SaveChanges();

		void DetachAllEntries();
	}
}
