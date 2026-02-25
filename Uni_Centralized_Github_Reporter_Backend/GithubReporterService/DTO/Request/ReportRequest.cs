using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubReporterService.DTO.Request
{
	public record CommitPagedRequest : PagedRequest
	{
		[Required]
		public string RepositoryUrl { get; set; } = null!;

		public CommitPagedRequest() { }

	}

	public record PullRequestPagedRequest : PagedRequest
	{
		[Required]
		public string RepositoryUrl { get; set; } = null!;
		public PullRequestPagedRequest() { }
	}

	public record IssuePagedRequest : PagedRequest
	{
		[Required]
		public string RepositoryUrl { get; set; } = null!;
		public IssuePagedRequest() { }
	}

	
}
