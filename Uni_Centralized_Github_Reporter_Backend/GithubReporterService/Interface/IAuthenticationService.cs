using GithubReporterService.DTO;
using GithubReporterService.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubReporterService.Interface
{
	public interface IAuthenticationService
	{
		public ApiResponse<LoginResponse> ValidateUserCredentials(string email, string password);
	}
}
