using GithubReporterRepository.Enum;
using GithubReporterService.Core;
using GithubReporterService.DTO;
using GithubReporterService.DTO.Request;
using GithubReporterService.DTO.Response;
using GithubReporterService.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GithubReporterAPI.Controllers;

[Route("api/accounts")]
[ApiController]
public class AccountController2 : Controller
{
	private readonly IAccountService2 _accountService;

	public AccountController2(IAccountService2 accountService)
	{
		_accountService = accountService;
	}



	/// <summary>
	/// Search group with pagination, sorting and filtering
	/// </summary>
	/// <param name="request"></param>
	/// <returns></returns>
	[HttpGet]
	[Authorize]
	public async Task<ActionResult<ApiResponse<PagedResult<AccountViewDTO>>>> Search([FromQuery] AccountPagedRequest request)
	{

		var result = await _accountService.SearchAccount(request);
		if (result == null)
		{
			return NotFound(ApiResponse<PagedResult<AccountViewDTO>>
				.ErrorResponse("No Group Teams found", statusCode: APIStatusCode.NotFound.GetHashCode()));
		}

		return Ok(ApiResponse<PagedResult<AccountViewDTO>>.SuccessResponse(result, "Account retrieved successfully"));

	}

	[HttpGet("{accountId}")]
	[Authorize]
	public async Task<ActionResult<ApiResponse<object>>> GetDetail(Guid accountId)
	{

		var result = await _accountService.GetAccountById(accountId);

		if (result == null)
		{
			return NotFound(ApiResponse<object>
				.ErrorResponse($"No Group Team with Id {accountId} found", statusCode: APIStatusCode.NotFound.GetHashCode()));
		}

		return Ok(ApiResponse<object>.SuccessResponse(result, "Account retrieved successfully"));

	}


	[HttpPost("student")]
	[Authorize]
	public async Task<ActionResult<ApiResponse<object>>> CreateStudent([FromBody] CreateStudentAccountDTO request)
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

		var result =await _accountService.CreateStudentAccount(request);
		return StatusCode(statusCode: 201, ApiResponse<object>.CreatedSuccessReponse(result, "Student Account created successfully"));

	}


	[HttpPost("supervisor")]
	[Authorize]
	public async Task<ActionResult<ApiResponse<object>>> CreateSupervisor([FromBody] CreateSupervisorAccountDTO request)
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

		var result = await _accountService.CreateSupervisorAccount(request);
		return StatusCode(statusCode:201, ApiResponse<object>.CreatedSuccessReponse(result, "Supervisor Account created successfully"));

	}


	[HttpPut("student/{accountId}")]
	[Authorize]
	public async Task<ActionResult<ApiResponse<object>>> UpdateStudent(Guid accountId, [FromBody] UpdateStudentAccountDTO request)
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

		await _accountService.UpdateStudentAccount(request, accountId);
		return Ok(ApiResponse<object>.SuccessResponse(null, "Student updated successfully"));

	}

	[HttpPut("supervisor/{accountId}")]
	[Authorize]
	public async Task<ActionResult<ApiResponse<object>>> UpdateSupervisor(Guid accountId, [FromBody] UpdateSupervisorAccountDTO request)
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

		await _accountService.UpdateSupervisorAccount(request, accountId);
		return Ok(ApiResponse<object>.SuccessResponse(null, "Supervisor updated successfully"));

	}

	[HttpDelete("{accountId}")]
	[Authorize]
	public async Task<ActionResult<ApiResponse<object>>> Delete(Guid accountId)
	{

		await _accountService.DeleteAccount(accountId);
		return Ok(ApiResponse<object>.SuccessResponse(null, "Account deleted successfully"));

	}
}
