using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubReporterService.DTO.Request
{
	public class CreateGroupDTO
	{
		public string GroupName { get; set; }

		public string GroupCode { get; set; }

		public Guid AccountId { get; set; }

		public int GroupRole { get; set; }

		public Guid ProjectId { get; set; }

		public Guid SupervisorId { get; set; }
	}

	public class UpdateGroupDTO
	{
		public string GroupName { get; set; }

		public string GroupCode { get; set; }

		public Guid AccountId { get; set; }

		public int GroupRole { get; set; }

		public Guid ProjectId { get; set; }

		public Guid SupervisorId { get; set; }
	}
}
