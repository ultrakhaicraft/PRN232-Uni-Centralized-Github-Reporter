using GithubReporterRepository.EntityModel;
using GithubReporterRepository.Interface;
using GithubReporterService.DTO;
using GithubReporterService.DTO.Response;
using GithubReporterService.Interface;
using GithubReporterService.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubReporterService.Core;

public class AuthenticationService : IAuthenticationService
{
	private readonly IAccountRepository _accountRepository;
	private readonly TokenProvider _tokenProvider;

	public AuthenticationService(IAccountRepository accountRepository, TokenProvider tokenProvider)
	{
		_accountRepository = accountRepository;
		_tokenProvider = tokenProvider;
	}




	// Login method to validate user credentials
	public ApiResponse<LoginResponse> ValidateUserCredentials(string email, string password)
	{
		Account account = _accountRepository.GetByEmailMockAsync(email).Result;
		if (account == null)
		{
			return ApiResponse<LoginResponse>.ErrorResponse("Account not foun with Email.", 404);
		}

		bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, account.PasswordHash);

		if(!isPasswordValid)
		{
			return ApiResponse<LoginResponse>.ErrorResponse("Invalid password.", 401);
		}

		string token = _tokenProvider.generateAccessToken(account);

		var loginResponse = new LoginResponse
		{
			Token = token,
			Expiration = DateTime.UtcNow.AddHours(10)
		};

		return ApiResponse<LoginResponse>.SuccessResponse(loginResponse, "Login successful.");
	}

}
