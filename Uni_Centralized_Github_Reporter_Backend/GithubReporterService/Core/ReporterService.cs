using GithubReporterService.DTO;
using GithubReporterService.DTO.Request;
using GithubReporterService.DTO.Response;
using GithubReporterService.Interface;
using GithubReporterService.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubReporterService.Core
{
	public class ReporterService : IReporterService
	{
		private readonly IGithubService _githubService;

		public ReporterService(IGithubService githubService)
		{
			_githubService = githubService;
		}

		public async Task<PagedResult<CommitDto>> GetCommitsAsync(CommitPagedRequest request)
		{
			// Get all commits from GitHub service
			var allCommits = await _githubService.GetAllCommitsAsync(request.RepositoryUrl);

			// Apply pagination
			var pagedCommits = await PageConverter<CommitDto>.ToPagedResult(
				request.Page,
				request.PageSize,
				allCommits.Count,
				allCommits.AsQueryable()
			);	

			return pagedCommits;

		}

		public async Task<PagedResult<PullRequestDto>> GetPullRequestsAsync(PullRequestPagedRequest request)
		{
			// Get all commits from GitHub service
			var allCommits = await _githubService.GetAllPullRequestsAsync(request.RepositoryUrl);

			// Apply pagination
			var pagedCommits = await PageConverter<PullRequestDto>.ToPagedResult(
				request.Page,
				request.PageSize,
				allCommits.Count,
				allCommits.AsQueryable()
			);

			return pagedCommits;

		}

		public async Task<PagedResult<IssueDto>> GetIssuesAsync(IssuePagedRequest request)
		{
			// Get all commits from GitHub service
			var allCommits = await _githubService.GetAllIssuesAsync(request.RepositoryUrl);

			// Apply pagination
			var pagedCommits = await PageConverter<IssueDto>.ToPagedResult(
				request.Page,
				request.PageSize,
				allCommits.Count,
				allCommits.AsQueryable()
			);

			return pagedCommits;

		}

		public async Task<CommitReportDto> GenerateCommitReportAsync(string repositoryUrl)
		{
			// Get all commits
			var commits = await _githubService.GetAllCommitsAsync(repositoryUrl);

			if (!commits.Any())
			{
				return new CommitReportDto
				{
					RepositoryUrl = repositoryUrl,
					TotalCommits = 0,
					UserStatistics = new List<UserCommitStatistic>()
				};
			}

			// Group commits by author (username)
			var groupedCommits = commits
				.GroupBy(c => c.Author)
				.Select(g => new UserCommitStatistic
				{
					Username = g.Key,
					Email = g.First().AuthorEmail,
					CommitCount = g.Count(),
					Percentage = Math.Round((double)g.Count() / commits.Count * 100, 2),
					TotalAdditions = g.Sum(c => c.Additions),
					TotalDeletions = g.Sum(c => c.Deletions),
					TotalChanges = g.Sum(c => c.TotalChanges),
					FirstCommit = g.Min(c => c.Date),
					LastCommit = g.Max(c => c.Date)
				})
				.OrderByDescending(u => u.CommitCount)
				.ToList();

			return new CommitReportDto
			{
				RepositoryUrl = repositoryUrl,
				TotalCommits = commits.Count,
				UserStatistics = groupedCommits
			};
		}
	}
}
