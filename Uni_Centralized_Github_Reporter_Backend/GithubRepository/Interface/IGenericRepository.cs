using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GithubReporterRepository.Interface
{
	public interface IGenericRepository<T> where T : class
	{
		// Get operations
		Task<T?> GetByIdAsync(int id);
		Task<T?> GetByIdAsync(string id);
		Task<IEnumerable<T>> GetAllAsync();
		Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
		Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

		// Add operations
		Task<T> AddAsync(T entity);
		Task AddRangeAsync(IEnumerable<T> entities);

		// Update operations
		void Update(T entity);
		void UpdateRange(IEnumerable<T> entities);

		// Delete operations
		void Delete(T entity);
		void DeleteRange(IEnumerable<T> entities);

		// Query operations
		Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
		Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);

		// Save changes
		Task<int> SaveChangesAsync();
	}
}
