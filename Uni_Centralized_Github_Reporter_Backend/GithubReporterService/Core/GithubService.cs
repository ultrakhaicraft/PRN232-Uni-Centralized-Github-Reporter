using GithubReporterService.DTO;
using GithubReporterService.Interface;
using Microsoft.Extensions.Configuration;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
		public Task<GroupReport> GenerateGroupReportAsync(GithubReportRequest request)
		{
			throw new NotImplementedException();
		}

		public Task<StudentContributionReport> GenerateStudentReportAsync(string owner, string repo, string username, DateTime? startDate = null, DateTime? endDate = null)
		{
			throw new NotImplementedException();
		}
	}
}
