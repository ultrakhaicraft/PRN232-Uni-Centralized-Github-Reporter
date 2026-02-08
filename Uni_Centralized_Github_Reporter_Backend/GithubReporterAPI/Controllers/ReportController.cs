using GithubReporterRepository.Enum;
using GithubReporterService.Core;
using GithubReporterService.DTO;
using GithubReporterService.DTO.Request;
using GithubReporterService.DTO.Response;
using GithubReporterService.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GithubReporterAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ReportController : Controller
	{
		private readonly IReporterService _reporterService;

		public ReportController(IReporterService reporterService)
		{
			_reporterService = reporterService;
		}

		[HttpGet("commits")]
		[Authorize]
		public async Task<ActionResult<ApiResponse<PagedResult<CommitDto>>>> GetAllCommit([FromQuery] CommitPagedRequest request)
		{

			var result = await _reporterService.GetCommitsAsync(request);
			if (result == null)
			{
				return NotFound(ApiResponse<PagedResult<CommitDto>>
					.ErrorResponse("No commits found", statusCode: APIStatusCode.NotFound.GetHashCode()));
			}

			return Ok(ApiResponse<PagedResult<CommitDto>>.SuccessResponse(result, "Commits from github project retrieved successfully"));

		}

		[HttpGet("pull-requests")]
		[Authorize]
		public async Task<ActionResult<ApiResponse<PagedResult<PullRequestDto>>>> GetAllPullRequest([FromQuery] PullRequestPagedRequest request)
		{

			var result = await _reporterService.GetPullRequestsAsync(request);
			if (result == null)
			{
				return NotFound(ApiResponse<PagedResult<PullRequestDto>>
					.ErrorResponse("No Issues found", statusCode: APIStatusCode.NotFound.GetHashCode()));
			}

			return Ok(ApiResponse<PagedResult<PullRequestDto>>.SuccessResponse(result, "Pull Requests from github project retrieved successfully"));

		}

		[HttpGet("issues")]
		[Authorize]
		public async Task<ActionResult<ApiResponse<PagedResult<IssueDto>>>> GetAllIssues([FromQuery] IssuePagedRequest request)
		{

			var result = await _reporterService.GetIssuesAsync(request);
			if (result == null)
			{
				return NotFound(ApiResponse<PagedResult<IssueDto>>
					.ErrorResponse("No Project found", statusCode: APIStatusCode.NotFound.GetHashCode()));
			}

			return Ok(ApiResponse<PagedResult<IssueDto>>.SuccessResponse(result, "Issues from github project retrieved successfully"));

		}

		[HttpGet("commits-contribute")]
		[Authorize]
		public async Task<ActionResult<ApiResponse<CommitReportDto>>> GetCommitContribute([FromBody] string repositoryURL)
		{

			var result = await _reporterService.GenerateCommitReportAsync(repositoryURL);
			if (result == null)
			{
				return NotFound(ApiResponse<CommitReportDto>
					.ErrorResponse("Unable to generate report", statusCode: APIStatusCode.NotFound.GetHashCode()));
			}

			return Ok(ApiResponse<CommitReportDto>.SuccessResponse(result, "Report generated successfully"));

		}
	}
}
