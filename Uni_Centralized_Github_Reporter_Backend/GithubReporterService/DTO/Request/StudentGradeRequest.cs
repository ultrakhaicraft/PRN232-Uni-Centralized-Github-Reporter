using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubReporterService.DTO.Request
{
	public class AddStudentGradeDTO
	{

		public Guid StudentId { get; set; }

		public Guid ProjectId { get; set; }

		public int Grade { get; set; }
	}

	public class UpdateStudentGradeDTO
	{

		public Guid StudentId { get; set; }

		public Guid ProjectId { get; set; }

		public int Grade { get; set; }
	}
}
