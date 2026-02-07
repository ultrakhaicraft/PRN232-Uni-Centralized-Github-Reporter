using GithubReporterService.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubReporterService.Utilities
{
	public static class PageConverter<T> where T : class
	{
		public async static Task<PagedResult<T>> ToPagedResult(
			int page, int pageSize, int totalItems, IQueryable<T> queryableData)
		{
			var pagedItems = await queryableData.Skip((page - 1) * pageSize)
							   .Take(pageSize)
							   .ToListAsync();

			int totalPage = (int)Math.Ceiling((double)totalItems / pageSize);


			return new PagedResult<T>(page, pageSize, totalPage, totalItems, pagedItems);

		}
	}
}
