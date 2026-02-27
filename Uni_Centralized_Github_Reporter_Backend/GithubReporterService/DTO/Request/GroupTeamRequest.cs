using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubReporterService.DTO.Request
{
	public class CreateGroupDTO
	{

		public Guid AccountId { get; set; }

		public int GroupRole { get; set; }

		public Guid ProjectId { get; set; }

	}

	public class UpdateGroupDTO
	{

		public Guid AccountId { get; set; }

		public int GroupRole { get; set; }

		public Guid ProjectId { get; set; }

	}
}
