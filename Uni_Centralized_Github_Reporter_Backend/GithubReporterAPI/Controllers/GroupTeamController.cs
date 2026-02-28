using GithubReporterRepository.Enum;
using GithubReporterService.Core;
using GithubReporterService.DTO;
using GithubReporterService.DTO.Request;
using GithubReporterService.DTO.Response;
using GithubReporterService.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace GithubReporterAPI.Controllers;

[Route("api/groupteams")]
[ApiController]
public class GroupTeamController : Controller
{
	private readonly IGroupTeamService _groupTeamService;

	public GroupTeamController(IGroupTeamService groupTeamService)
	{
		_groupTeamService = groupTeamService;
	}


	/// <summary>
	/// Search group with pagination, sorting and filtering. Do not use SearchKeyword for now as it is not implemented yet, use AccountId for search
	/// </summary>
	/// <param name="request"></param>
	/// <returns></returns>
	[HttpGet]
	[Authorize]
	public async Task<ActionResult<ApiResponse<PagedResult<GroupTeamViewDTO>>>> Search([FromQuery] GroupTeamPagedRequest request)
	{

		var result = await _groupTeamService.SearchGroupTeam(request);
		if (result == null)
		{
			return NotFound(ApiResponse<PagedResult<GroupTeamViewDTO>>
				.ErrorResponse("No Group Teams found", statusCode: APIStatusCode.NotFound.GetHashCode()));
		}

		return Ok(ApiResponse<PagedResult<GroupTeamViewDTO>>.SuccessResponse(result, "Group Teams retrieved successfully"));

	}

	[HttpGet("{groupTeamId}")]
	[Authorize]
	public async Task<ActionResult<ApiResponse<List<GroupTeamDetailDTO>>>> GetDetail(Guid groupTeamId)
	{

		var result = await _groupTeamService.GetGroupTeamByProjectId(groupTeamId);

		if (result == null)
		{
			return NotFound(ApiResponse<List<GroupTeamDetailDTO>>
				.ErrorResponse($"No Group Team with Id {groupTeamId} found", statusCode: APIStatusCode.NotFound.GetHashCode()));
		}

		return Ok(ApiResponse<List<GroupTeamDetailDTO>>.SuccessResponse(result, "Groups retrieved successfully"));

	}

	
	[HttpPost]
	[Authorize]
	public async Task<ActionResult<ApiResponse<object>>> Create([FromBody] CreateGroupDTO request)
	{

		if (!ModelState.IsValid)
		{
			var errors = ModelState.Values
				.SelectMany(v => v.Errors)
				.Select(e => e.ErrorMessage)
				.ToList();

			return BadRequest(ApiResponse<object>.ErrorResponse(
				"Validation failed",
				statusCode: APIStatusCode.BadRequest.GetHashCode(),
				errors
			));
		}

		var result = await _groupTeamService.CreateGroupTeam(request);
		return Ok(ApiResponse<object>.CreatedSuccessReponse(result, "Group created successfully"));

	}

	[HttpPost("add")]
	[Authorize]
	public async Task<ActionResult<ApiResponse<object>>> AddTeamMember([FromBody] CreateGroupDTO request)
	{

		if (!ModelState.IsValid)
		{
			var errors = ModelState.Values
				.SelectMany(v => v.Errors)
				.Select(e => e.ErrorMessage)
				.ToList();

			return BadRequest(ApiResponse<object>.ErrorResponse(
				"Validation failed",
				statusCode: APIStatusCode.BadRequest.GetHashCode(),
				errors
			));
		}

		await _groupTeamService.AddTeamMember(request);
		return Ok(ApiResponse<object>.CreatedSuccessReponse(null, "Team Member added successfully"));

	}

	/// <summary>
	/// Change the role of team member in the project, accountId in the request body is used only for querying the team member, and projectId is also only for querying the project, then update the role of the team member in the project
	/// </summary>
	/// <param name="projectId"></param>
	/// <param name="request"></param>
	/// <returns></returns>
	[HttpPatch("update-role")]
	public async Task<ActionResult<ApiResponse<object>>> Edit([FromBody] UpdateGroupDTO request)
	{

		if (!ModelState.IsValid)
		{
			var errors = ModelState.Values
				.SelectMany(v => v.Errors)
				.Select(e => e.ErrorMessage)
				.ToList();

			return BadRequest(ApiResponse<object>.ErrorResponse(
				"Validation failed",
				400,
				errors
			));
		}

		await _groupTeamService.UpdateTeamMemberRole(request);
		return Ok(ApiResponse<object>.SuccessResponse(null, "Team Member role updated successfully"));

	}


	[HttpDelete("delete")]
	[Authorize]
	public async Task<ActionResult<ApiResponse<object>>> RemoveTeamMember([FromQuery] Guid accountId, [FromQuery] Guid projectId)
	{

		await _groupTeamService.RemoveTeamMember(accountId, projectId);
		return Ok(ApiResponse<object>.SuccessResponse(null, "A Team Member deleted successfully"));

	}
}
