using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubReporterService.DTO.Response
{
	public record ProjectViewDTO()
	{
		public Guid ProjectId { get; set; }

		public string? ProjectName { get; set; }

		public string? GithubLink { get; set; }

		public string? Description { get; set; }
	}

	public record ProjectDetailDTO()
	{
		public Guid ProjectId { get; set; }

		public string? ProjectName { get; set; }

		public string? GithubLink { get; set; }

		public string? Description { get; set; }

		public Guid CreatedBy { get; set; }

		public DateTime DateCreated { get; set; }

		public string? AccessToken { get; set; }
	}
}
