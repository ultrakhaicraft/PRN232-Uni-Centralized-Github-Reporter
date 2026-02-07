using GithubReporterRepository.Enum;
using GithubReporterService.Core;
using GithubReporterService.DTO;
using GithubReporterService.DTO.Request;
using GithubReporterService.DTO.Response;
using GithubReporterService.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace GithubReporterAPI.Controllers
{
	public class GroupTeamController : Controller
	{
		private readonly IGroupTeamService _groupTeamService;

		public GroupTeamController(IGroupTeamService groupTeamService)
		{
			_groupTeamService = groupTeamService;
		}


		/// <summary>
		/// Search accounts with pagination, sorting and filtering
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

		[HttpGet("{groupId}")]
		[Authorize]
		public async Task<ActionResult<ApiResponse<GroupTeamDetailDTO>>> GetDetail(Guid groupId)
		{

			var result = await _groupTeamService.GetGroupTeamById(groupId);

			if (result == null)
			{
				return NotFound(ApiResponse<GroupTeamDetailDTO>
					.ErrorResponse($"No Group Team with Id {groupId} found", statusCode: APIStatusCode.NotFound.GetHashCode()));
			}

			return Ok(ApiResponse<GroupTeamDetailDTO>.SuccessResponse(result, "Groups retrieved successfully"));

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

			await _groupTeamService.CreateGroupTeam(request);
			return Ok(ApiResponse<object>.SuccessResponse(null, "Group created successfully"));

		}


		[HttpPut("{groupId}")]
		[Authorize]
		public async Task<ActionResult<ApiResponse<object>>> Edit(Guid groupId, [FromBody] UpdateGroupDTO request)
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

			await _groupTeamService.UpdateGroupTeam(request, groupId);
			return Ok(ApiResponse<object>.SuccessResponse(null, "Group Team updated successfully"));

		}

		[HttpDelete("{groupId}")]
		[Authorize]
		public async Task<ActionResult<ApiResponse<object>>> Delete(Guid groupId)
		{

			await _groupTeamService.DeleteGroupTeam(groupId);
			return Ok(ApiResponse<object>.SuccessResponse(null, "Group Team deleted successfully"));

		}
	}
}
