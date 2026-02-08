using GithubReporterService.DTO;
using GithubReporterService.DTO.Request;
using GithubReporterService.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubReporterService.Interface
{
	public interface IReporterService
	{
		public Task<CommitReportDto> GenerateCommitReportAsync(string repositoryUrl);
		public Task<PagedResult<CommitDto>> GetCommitsAsync(CommitPagedRequest request);
		public Task<PagedResult<PullRequestDto>> GetPullRequestsAsync(PullRequestPagedRequest request);
		public Task<PagedResult<IssueDto>> GetIssuesAsync(IssuePagedRequest request);

	}
}
