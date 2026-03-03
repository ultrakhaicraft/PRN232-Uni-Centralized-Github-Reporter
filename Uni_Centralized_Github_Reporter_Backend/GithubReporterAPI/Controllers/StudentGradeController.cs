using GithubReporterRepository.Enum;
using GithubReporterService.DTO;
using GithubReporterService.DTO.Request;
using GithubReporterService.DTO.Response;
using GithubReporterService.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GithubReporterAPI.Controllers;

[ApiController]
[Route("api/studentgrades")]	
public class StudentGradeController : Controller
{
	private readonly IStudentGradeService _studentGradeService;

	public StudentGradeController(IStudentGradeService studentGradeService)
	{
		_studentGradeService = studentGradeService;
	}


	/// <summary>
	/// Search grade with pagination, sorting and filtering
	/// </summary>
	/// <param name="projectId"></param>
	/// <returns></returns>
	[HttpGet("project/{projectId}")]
	[Authorize]
	public async Task<ActionResult<ApiResponse<List<ViewStudentGradeDTO>>>> SearchByProjectId(Guid projectId)
	{

		var result = await _studentGradeService.GetGradesByProjectIdAsync(projectId);
		if (result == null)
		{
			return NotFound(ApiResponse<List<ViewStudentGradeDTO>>
				.ErrorResponse("No Student Grade found", statusCode: APIStatusCode.NotFound.GetHashCode()));
		}

		return Ok(ApiResponse<List<ViewStudentGradeDTO>>.SuccessResponse(result, "Student Grade retrieved successfully"));

	}

	/// <summary>
	/// Search grade with pagination, sorting and filtering
	/// </summary>
	/// <param name="studentId"></param>
	/// <returns></returns>
	[HttpGet("student/{studentId}")]
	[Authorize]
	public async Task<ActionResult<ApiResponse<List<ViewStudentGradeDTO>>>> SearchByStudentId(Guid studentId)
	{

		var result = await _studentGradeService.GetGradesByStudentIdAsync(studentId);
		if (result == null)
		{
			return NotFound(ApiResponse<List<ViewStudentGradeDTO>>
				.ErrorResponse("No Student Grade found", statusCode: APIStatusCode.NotFound.GetHashCode()));
		}

		return Ok(ApiResponse<List<ViewStudentGradeDTO>>.SuccessResponse(result, " Student Grade retrieved successfully"));

	}

	[HttpGet("{studentGradeId}")]
	[Authorize]
	public async Task<ActionResult<ApiResponse<ViewStudentGradeDTO>>> GetDetail(Guid studentGradeId)
	{

		var result = await _studentGradeService.GetGradeById(studentGradeId);

		if (result == null)
		{
			return NotFound(ApiResponse<ViewStudentGradeDTO>
				.ErrorResponse($"No Group Team with Id {studentGradeId} found", statusCode: APIStatusCode.NotFound.GetHashCode()));
		}

		return Ok(ApiResponse<ViewStudentGradeDTO>.SuccessResponse(result, "Student Grade retrieved successfully"));

	}


	[HttpPost]
	[Authorize]
	public async Task<ActionResult<ApiResponse<object>>> Create([FromBody] AddStudentGradeDTO request)
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

		var result = await _studentGradeService.AddStudentGrade(request);
		return StatusCode(statusCode: 201, ApiResponse<object>.CreatedSuccessReponse(result, "Student Grade created successfully"));

	}


	[HttpPut("{studentGradeId}")]
	[Authorize]
	public async Task<ActionResult<ApiResponse<object>>> Edit(Guid groupId, [FromBody] UpdateStudentGradeDTO request)
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

		await _studentGradeService.UpdateStudentGrade(request, groupId);
		return Ok(ApiResponse<object>.SuccessResponse(null, "Student Grade updated successfully"));

	}

	[HttpDelete("{studentGradeId}")]
	[Authorize]
	public async Task<ActionResult<ApiResponse<object>>> Delete(Guid studentGradeId)
	{
		await _studentGradeService.DeleteStudentGrade(studentGradeId);
		return Ok(ApiResponse<object>.SuccessResponse(null, "Student Grade deleted successfully"));
	}


}
