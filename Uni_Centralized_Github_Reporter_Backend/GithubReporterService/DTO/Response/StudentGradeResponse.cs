using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubReporterService.DTO.Response
{
	public class ViewStudentGradeDTO
	{
		public Guid GradePerProjectId { get; set; }

		public Guid StudentId { get; set; }
		public string StudentName { get; set; } = string.Empty;

		public Guid ProjectId { get; set; }
		public string ProjectName { get; set; } = string.Empty;

		public int Grade { get; set; }
	}
}
