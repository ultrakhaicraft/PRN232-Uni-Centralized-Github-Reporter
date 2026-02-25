using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubReporterService.DTO
{
	public record PagedRequest(
		int Page = 1,
		int PageSize = 20,
		string? SearchKeyword = null,
		bool? IsAscending = null,
		string? Fields = null
	);
	

	public record ProjectPagedRequest : PagedRequest
	{
		

	}

	public record AccountPagedRequest : PagedRequest
	{
		public int? Role { get; set; }
		public int? Status { get; set; }
	}

	public record GradePagedRequest : PagedRequest
	{
		public Guid? StudentId { get; set; }

		public Guid? ProjectId { get; set; }

	
	}

	public record GroupTeamPagedRequest : PagedRequest
	{
		public Guid? AccountId { get; set; }
		
	}


	public record PagedResult<T>(
		int Page,
		int PageSize,
		int TotalPage,
		int TotalItems,
		IReadOnlyList<T> Items
	);
}
