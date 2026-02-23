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

		public Guid ProjectId { get; set; }

		public int Grade { get; set; }
	}
}
