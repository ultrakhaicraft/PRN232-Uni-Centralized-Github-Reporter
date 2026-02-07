using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubReporterService.DTO.Request
{
	public record CreateProjectDTO()
	{
		public string? ProjectName { get; set; }

		public string? GithubLink { get; set; }

		public string? Description { get; set; }

		public Guid CreatedBy { get; set; }

		public DateTime DateCreated { get; set; }

		public string? AccessToken { get; set; }
	}

	public record UpdateProjectDTO()
	{
		public string? ProjectName { get; set; }

		public string? GithubLink { get; set; }

		public string? Description { get; set; }

		public Guid CreatedBy { get; set; }

		public DateTime DateCreated { get; set; }

		public string? AccessToken { get; set; }
	}
}
