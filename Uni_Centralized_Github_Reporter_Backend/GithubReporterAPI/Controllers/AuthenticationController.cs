using GithubReporterService.DTO;
using GithubReporterService.DTO.Request;
using GithubReporterService.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GithubReporterAPI.Controllers;

public class AuthenticationController : ControllerBase
{
	private readonly IAuthenticationService _authenticationService;

	public AuthenticationController(IAuthenticationService authenticationService)
	{
		_authenticationService = authenticationService;
	}

	[HttpPost("login")]
	public IActionResult Login([FromBody] LoginRequest loginRequest)
	{
		try
		{
			var response = _authenticationService.ValidateUserCredentials(loginRequest.Email, loginRequest.Password);

			if (response.Success)
			{
				return Ok(response);
			}
			else
			{
				return StatusCode(response.StatusCode, response);
			}
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			var error = ApiResponse<string>.ErrorResponse("An unexpected error occurred while processing the login request.",
				errors: new List<string>() { e.Message});
			return StatusCode(500, error);
		}

	}
}
