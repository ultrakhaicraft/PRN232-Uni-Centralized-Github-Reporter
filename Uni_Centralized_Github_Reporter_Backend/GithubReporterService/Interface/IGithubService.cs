using GithubReporterService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubReporterService.Interface;

public interface IGithubService
{
	Task<GroupReport> GenerateGroupReportAsync(GithubReportRequest request);
	Task<StudentContributionReport> GenerateStudentReportAsync(
		string owner,
		string repo,
		string username,
		DateTime? startDate = null,
		DateTime? endDate = null);
}
