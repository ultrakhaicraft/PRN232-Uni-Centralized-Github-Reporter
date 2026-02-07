using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubReporterService.DTO;

public class CommitDto
{
	public string Sha { get; set; }
	public string Message { get; set; }
	public string Author { get; set; }
	public DateTime Date { get; set; }
	public int Additions { get; set; }
	public int Deletions { get; set; }
	public string Url { get; set; }
}

public class PullRequestDto
{
	public int Number { get; set; }
	public string Title { get; set; }
	public string State { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime? MergedAt { get; set; }
	public string Url { get; set; }
}

public class IssueDto
{
	public int Number { get; set; }
	public string Title { get; set; }
	public string State { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime? ClosedAt { get; set; }
	public int Comments { get; set; }
	public string Url { get; set; }
}

// DTOs/GithubReportRequest.cs
public class GithubReportRequest
{
	public string RepositoryOwner { get; set; }  // e.g., "microsoft"
	public string RepositoryName { get; set; }   // e.g., "vscode"
	public List<string> GithubUsernames { get; set; }  // Student usernames
	public DateTime? StartDate { get; set; }     // Optional: filter by date range
	public DateTime? EndDate { get; set; }
}

// DTOs/StudentContributionReport.cs
public class StudentContributionReport
{
	public string Username { get; set; }
	public string ProfileUrl { get; set; }
	public ContributionSummary Summary { get; set; }
	public List<CommitDto> Commits { get; set; }
	public List<PullRequestDto> PullRequests { get; set; }
	public List<IssueDto> Issues { get; set; }
}

public class ContributionSummary
{
	public int TotalCommits { get; set; }
	public int TotalPullRequests { get; set; }
	public int TotalIssues { get; set; }
	public int LinesAdded { get; set; }
	public int LinesDeleted { get; set; }
	public DateTime? FirstContribution { get; set; }
	public DateTime? LastContribution { get; set; }
}

// DTOs/GroupReport.cs
public class GroupReport
{
	public string RepositoryOwner { get; set; }
	public string RepositoryName { get; set; }
	public string RepositoryUrl { get; set; }
	public DateTime GeneratedAt { get; set; }
	public List<StudentContributionReport> StudentReports { get; set; }
	public GroupSummary GroupSummary { get; set; }
}

public class GroupSummary
{
	public int TotalStudents { get; set; }
	public int TotalCommits { get; set; }
	public int TotalPullRequests { get; set; }
	public int TotalIssues { get; set; }
	public int TotalLinesAdded { get; set; }
	public int TotalLinesDeleted { get; set; }
}
