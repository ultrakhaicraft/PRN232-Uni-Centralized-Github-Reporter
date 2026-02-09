using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubReporterService.DTO.Response
{
	public class CommitReportDto
	{
		public string RepositoryUrl { get; set; }
		public int TotalCommits { get; set; }
		public List<UserCommitStatistic> UserStatistics { get; set; }
	}

	public class UserCommitStatistic
	{
		public string Username { get; set; }
		public string Email { get; set; }
		public int CommitCount { get; set; }
		public double Percentage { get; set; }
		public int TotalAdditions { get; set; }
		public int TotalDeletions { get; set; }
		public int TotalChanges { get; set; }
		public DateTime? FirstCommit { get; set; }
		public DateTime? LastCommit { get; set; }
	}
}
