using GithubReporterRepository.Enum;
using GithubReporterService.DTO;
using GithubReporterService.DTO.Request;
using GithubReporterService.DTO.Response;
using GithubReporterService.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GithubReporterAPI.Controllers;


/// <summary>
/// Manage Github-like projects in the application.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProjectController : Controller
{
	private readonly IProjectService _projectService;

	public ProjectController(IProjectService projectService)
	{
		_projectService = projectService;
	}

	/// <summary>
	/// Search accounts with pagination, sorting and filtering
	/// </summary>
	/// <param name="request"></param>
	/// <returns></returns>
	[HttpGet]
	[Authorize]
	public async Task<ActionResult<ApiResponse<PagedResult<ProjectViewDTO>>>> Search([FromQuery] ProjectPagedRequest request)
	{

		var result = await _projectService.SearchProject(request);
		if(result == null)
		{
			return NotFound(ApiResponse<PagedResult<ProjectViewDTO>>
				.ErrorResponse("No Project found", statusCode: APIStatusCode.NotFound.GetHashCode()));
		}
		
		return Ok(ApiResponse<PagedResult<ProjectViewDTO>>.SuccessResponse(result, "Projects retrieved successfully"));

	}

	[HttpGet("{projectId}")]
	[Authorize]
	public async Task<ActionResult<ApiResponse<ProjectDetailDTO>>> GetDetail(Guid projectId)
	{

		var result = await _projectService.GetProjectById(projectId);

		if (result == null)
		{
			return NotFound(ApiResponse<ProjectDetailDTO>
				.ErrorResponse($"No Project with Id {projectId} found", statusCode: APIStatusCode.NotFound.GetHashCode()));
		}

		return Ok(ApiResponse<ProjectDetailDTO>.SuccessResponse(result, "Projects retrieved successfully"));

	}

	[HttpPost]
	[Authorize]
	public async Task<ActionResult<ApiResponse<object>>> Create([FromBody] CreateProjectDTO request)
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

		await _projectService.CreateProject(request);
		return Ok(ApiResponse<object>.SuccessResponse(null, "Account created successfully"));

	}


	[HttpPut("{projectId}")]
	[Authorize]
	public async Task<ActionResult<ApiResponse<object>>> Edit(Guid projectId, [FromBody] UpdateProjectDTO request)
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

		await _projectService.UpdateProject(request, projectId);
		return Ok(ApiResponse<object>.SuccessResponse(null, "Project updated successfully"));

	}

	[HttpDelete("{projectId}")]
	[Authorize]
	public async Task<ActionResult<ApiResponse<object>>> Delete(int projectId)
	{

		await _projectService.DeleteProject(projectId);
		return Ok(ApiResponse<object>.SuccessResponse(null, "Project deleted successfully"));

	}
}
