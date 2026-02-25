using GithubReporterService.DTO.Request;
using GithubReporterService.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubReporterService.Interface
{
	public interface IStudentGradeService
	{
		public Task<ViewStudentGradeDTO> GetGradeById(Guid gradeId);
		public Task UpdateStudentGrade(UpdateStudentGradeDTO request, Guid gradeId);
		public Task AddStudentGrade(AddStudentGradeDTO request);
		public Task<List<ViewStudentGradeDTO>> GetGradesByProjectIdAsync(Guid projectId);
		public Task<List<ViewStudentGradeDTO>> GetGradesByStudentIdAsync(Guid studentId);



	}
}
