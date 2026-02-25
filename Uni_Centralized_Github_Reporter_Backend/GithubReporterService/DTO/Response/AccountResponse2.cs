using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubReporterService.DTO.Response
{
	public class AccountViewDTO
	{
		public Guid AccountId { get; set; }

		public string Name { get; set; }

		public string Email { get; set; }

		public int Status { get; set; }

		public int Role { get; set; }

		public DateTime DateCreated { get; set; }
	}

	public class AccountDetailDTO
	{
		public Guid AccountId { get; set; }

		public string Name { get; set; }

		public string Email { get; set; }

		public int Status { get; set; }

		public int Role { get; set; }

		public DateTime DateCreated { get; set; }

		public string GithubEmail { get; set; }
	}
	public class StudentAccountDetailDTO : AccountDetailDTO
	{	

		public Guid StudentId { get; set; }

		public string StudentCode { get; set; }

	}

	public class SupervisorAccountDetailDTO : AccountDetailDTO
	{

		public Guid SupervisorId { get; set; }

		public string SupervisorCode { get; set; }
	}
}
