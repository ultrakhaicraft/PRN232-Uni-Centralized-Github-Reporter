using GithubReporterService.DTO;
using GithubReporterService.Interface;
using Microsoft.Extensions.Configuration;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GithubReporterService.Core
{
	public class GithubService : IGithubService
	{
		private readonly GitHubClient _client;

		public GithubService(IConfiguration configuration)
		{
			_client = new GitHubClient(new ProductHeaderValue("GithubReporterService"));
			var token = configuration["GitHub:Token"];
			if (!string.IsNullOrEmpty(token))
			{
				_client.Credentials = new Credentials(token);
			}

		}

		public async Task<List<CommitDto>> GetAllCommitsAsync(string repositoryUrl)
		{
			var (owner,repo) = ParseGithubUrl(repositoryUrl);

			try
			{
				var commits = await _client.Repository.Commit.GetAll(owner, repo);

				return commits.Select(c => new CommitDto
				{
					Sha = c.Sha,
					Message = c.Commit.Message,
					Author = c.Commit.Author.Name,
					Date = c.Commit.Author.Date.DateTime,
					Url = c.HtmlUrl
				}).ToList();
			}
			catch (NotFoundException)
			{
				throw new Exception($"Repository not found: {owner}/{repo}");
			}
			catch (RateLimitExceededException)
			{
				throw new Exception("GitHub API rate limit exceeded. Please try again later.");
			}
		}

		public async Task<List<PullRequestDto>> GetAllPullRequestsAsync(string repositoryUrl)
		{
			var (owner, repo) = ParseGithubUrl(repositoryUrl);

			try
			{
				// Get all pull requests (open, closed, and merged)
				var pullRequestRequest = new PullRequestRequest
				{
					State = ItemStateFilter.All,
					SortProperty = PullRequestSort.Created,
					SortDirection = SortDirection.Descending
				};

				var pullRequests = await _client.PullRequest.GetAllForRepository(owner, repo, pullRequestRequest);

				return pullRequests.Select(pr => new PullRequestDto
				{
					Number = pr.Number,
					Title = pr.Title,
					State = pr.State.StringValue,
					CreatedBy = pr.User.Login,
					CreatedAt = pr.CreatedAt.DateTime,
					UpdatedAt = pr.UpdatedAt.DateTime,
					ClosedAt = pr.ClosedAt?.DateTime,
					MergedAt = pr.MergedAt?.DateTime,
					IsMerged = pr.Merged,
					Url = pr.HtmlUrl,
					Body = pr.Body,
					CommitsCount = pr.Commits,
					ChangedFiles = pr.ChangedFiles,
					Additions = pr.Additions,
					Deletions = pr.Deletions
				}).ToList();
			}
			catch (NotFoundException)
			{
				throw new Exception($"Repository not found: {owner}/{repo}");
			}
			catch (RateLimitExceededException)
			{
				throw new Exception("GitHub API rate limit exceeded. Please try again later.");
			}
		}


		public async Task<List<IssueDto>> GetAllIssuesAsync(string repositoryUrl)
		{
			var (owner, repo) = ParseGithubUrl(repositoryUrl);

			try
			{
				// Get all issues (open and closed)
				var issueRequest = new RepositoryIssueRequest
				{
					State = ItemStateFilter.All,
					SortProperty = IssueSort.Created,
					SortDirection = SortDirection.Descending
				};

				var issues = await _client.Issue.GetAllForRepository(owner, repo, issueRequest);

				return issues.Select(issue => new IssueDto
				{
					Number = issue.Number,
					Title = issue.Title,
					State = issue.State.StringValue,
					CreatedBy = issue.User.Login,
					CreatedAt = issue.CreatedAt.DateTime,
					UpdatedAt = issue.UpdatedAt?.DateTime,
					ClosedAt = issue.ClosedAt?.DateTime,
					Body = issue.Body,
					Comments = issue.Comments,
					Url = issue.HtmlUrl,
					Labels = issue.Labels.Select(l => l.Name).ToList(),
					Assignees = issue.Assignees.Select(a => a.Login).ToList()
				}).ToList();
			}
			catch (NotFoundException)
			{
				throw new Exception($"Repository not found: {owner}/{repo}");
			}
			catch (RateLimitExceededException)
			{
				throw new Exception("GitHub API rate limit exceeded. Please try again later.");
			}
		}


		/// <summary>
		/// Parse GitHub repository URL to extract owner and repository name
		/// Supports formats:
		/// - https://github.com/owner/repo
		/// - https://github.com/owner/repo.git
		/// - github.com/owner/repo
		/// - owner/repo
		/// </summary>
		private (string owner, string repo) ParseGithubUrl(string url)
		{
			if (string.IsNullOrWhiteSpace(url))
			{
				throw new ArgumentException("Repository URL cannot be empty");
			}

			// Remove .git suffix if present
			url = url.TrimEnd('/').Replace(".git", "");

			// Pattern to match GitHub URLs
			var patterns = new[]
			{
			@"github\.com[/:]([^/]+)/([^/]+)",  // https://github.com/owner/repo or git@github.com:owner/repo
            @"^([^/]+)/([^/]+)$"                 // owner/repo
		    };

			foreach (var pattern in patterns)
			{
				var match = Regex.Match(url, pattern, RegexOptions.IgnoreCase);
				if (match.Success)
				{
					var owner = match.Groups[1].Value;
					var repo = match.Groups[2].Value;
					return (owner, repo);
				}
			}

			throw new ArgumentException($"Invalid GitHub repository URL: {url}");
		}
	}
}
