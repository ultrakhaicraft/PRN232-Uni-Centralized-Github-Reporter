using GithubReporterService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubReporterService.Interface;

public interface IGithubService
{
	Task<List<CommitDto>> GetAllCommitsAsync(string repositoryUrl);
	Task<List<PullRequestDto>> GetAllPullRequestsAsync(string repositoryUrl);
	Task<List<IssueDto>> GetAllIssuesAsync(string repositoryUrl);

}
