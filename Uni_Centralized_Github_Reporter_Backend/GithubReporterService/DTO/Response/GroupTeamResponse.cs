using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubReporterService.DTO.Response
{
	public record GroupTeamViewDTO()
	{
		public Guid GroupId { get; set; }

		public string? GroupName { get; set; }

		public string? GroupCode { get; set; }

		public Guid AccountId { get; set; }

		public int GroupRole { get; set; }

		public Guid ProjectId { get; set; }

		public Guid SupervisorId { get; set; }
	}

	public record GroupTeamDetailDTO()
	{
		public Guid GroupId { get; set; }

		public string? GroupName { get; set; }

		public string? GroupCode { get; set; }

		public Guid AccountId { get; set; }

		public int GroupRole { get; set; }

		public Guid ProjectId { get; set; }

		public Guid SupervisorId { get; set; }
	}
}
