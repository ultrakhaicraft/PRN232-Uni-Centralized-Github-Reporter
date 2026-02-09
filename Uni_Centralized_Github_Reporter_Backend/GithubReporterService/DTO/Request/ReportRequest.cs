using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubReporterService.DTO.Request
{
	public class CommitPagedRequest : PagedRequest
	{
		[Required]
		public string RepositoryUrl { get; set; } = null!;

		public CommitPagedRequest() { }

	}

	public class PullRequestPagedRequest : PagedRequest
	{
		[Required]
		public string RepositoryUrl { get; set; } = null!;
		public PullRequestPagedRequest() { }
	}

	public class IssuePagedRequest : PagedRequest
	{
		[Required]
		public string RepositoryUrl { get; set; } = null!;
		public IssuePagedRequest() { }
	}

	
}
