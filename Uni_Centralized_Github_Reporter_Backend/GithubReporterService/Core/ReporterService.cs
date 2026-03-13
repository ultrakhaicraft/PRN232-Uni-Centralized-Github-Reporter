using GithubReporterRepository.Interface;
using GithubReporterRepository.Models;
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
		private readonly IGenericRepository<Account> _accountRepository;

		public ReporterService(IGithubService githubService, IGenericRepository<Account> accountRepository)
		{
			_githubService = githubService;
			_accountRepository = accountRepository;
		}

		public async Task<PagedResult<CommitDto>> GetCommitsAsync(CommitPagedRequest request)
		{
			if(request.RepositoryUrl == null)
			{
				throw new BadRequestException("Repository URL cannot be null or empty");
			}

			// Get all commits from GitHub service
			var allCommits = await _githubService.GetAllCommitsAsync(request.RepositoryUrl);

			// Apply pagination
			var pagedCommits =  PageConverter<CommitDto>.EnumrableToPagedResult(
				request.Page,
				request.PageSize,
				allCommits.Count,
				allCommits
			);	

			return pagedCommits;

		}

		public async Task<PagedResult<PullRequestDto>> GetPullRequestsAsync(PullRequestPagedRequest request)
		{
			if (request.RepositoryUrl == null)
			{
				throw new BadRequestException("Repository URL cannot be null or empty");
			}

			// Get all commits from GitHub service
			var allCommits = await _githubService.GetAllPullRequestsAsync(request.RepositoryUrl);

			// Apply pagination
			var pagedCommits =  PageConverter<PullRequestDto>.EnumrableToPagedResult(
				request.Page,
				request.PageSize,
				allCommits.Count,
				allCommits
			);

			return pagedCommits;

		}

		public async Task<PagedResult<IssueDto>> GetIssuesAsync(IssuePagedRequest request)
		{
			if (request.RepositoryUrl == null)
			{
				throw new BadRequestException("Repository URL cannot be null or empty");
			}

			// Get all commits from GitHub service
			var allCommits = await _githubService.GetAllIssuesAsync(request.RepositoryUrl);

			// Apply pagination
			var pagedCommits =  PageConverter<IssueDto>.EnumrableToPagedResult(
				request.Page,
				request.PageSize,
				allCommits.Count,
				allCommits
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

			// ✅ Match GitHub emails with database accounts
			foreach (var userStat in groupedCommits)
			{
				// Try to find account by GitHub email
				var account = await _accountRepository.FirstOrDefaultAsync(a => a.GithubEmail == userStat.Email);

				if (account != null)
				{
					// If found, use the account's name and email from database
					userStat.Username = account.Name;
					userStat.Email = account.Email;
				}
				// else: Keep the GitHub username and email
			}

			return new CommitReportDto
			{
				RepositoryUrl = repositoryUrl,
				TotalCommits = commits.Count,
				UserStatistics = groupedCommits
			};
		}
	}
}
