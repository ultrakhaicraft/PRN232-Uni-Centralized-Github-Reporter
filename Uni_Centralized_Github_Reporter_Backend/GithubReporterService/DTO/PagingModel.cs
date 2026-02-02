using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubReporterService.DTO
{
	public class PagedRequest
	{
		public PagedRequest(
		int Page = 1,
		int PageSize = 20,
		string? SearchKeyword = null,
		bool? IsAscending = null,
		string? Fields = null)
		{
			this.Page = Page;
			this.PageSize = PageSize;
			this.SearchKeyword = SearchKeyword;
			this.IsAscending = IsAscending;
			this.Fields = Fields;
		}

		// Properties
		public int Page { get; set; }
		public int PageSize { get; set; }
		public string? SearchKeyword { get; set; }
		public bool? IsAscending { get; set; }
		public string? Fields { get; set; }
	}

	public class ProjectPagedRequest : PagedRequest
	{
		public ProjectPagedRequest(
			int Page = 1,
			int PageSize = 20,
			string? SearchKeyword = null,
			bool? IsAscending = null,
			string? Fields = null
		) : base(Page, PageSize, SearchKeyword, IsAscending, Fields)
		{
		}

	}


	public record PagedResult<T>(
		int Page,
		int PageSize,
		int TotalPage,
		int TotalItems,
		IReadOnlyList<T> Items
	);
}
