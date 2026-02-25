using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubReporterService.DTO.Request
{
	public class CreateStudentAccountDTO
	{
		public string Name { get; set; }

		public string Email { get; set; }

		public string Password { get; set; }

		public string GithubEmail { get; set; }

		public string StudentCode { get; set; }

	}

	public class CreateSupervisorAccountDTO
	{
		public string Name { get; set; }

		public string Email { get; set; }

		public string Password { get; set; }

		public string GithubEmail { get; set; }

		public string SupervisorCode { get; set; }
	}

	public class UpdateStudentAccountDTO
	{
		public string Name { get; set; }

		public string Email { get; set; }

		public string Password { get; set; }

		public string GithubEmail { get; set; }

		public string StudentCode { get; set; }

	}

	public class UpdateSupervisorAccountDTO
	{
		public string Name { get; set; }

		public string Email { get; set; }

		public string Password { get; set; }

		public string GithubEmail { get; set; }

		public string SupervisorCode { get; set; }
	}

}
