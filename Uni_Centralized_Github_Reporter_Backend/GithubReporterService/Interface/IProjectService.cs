using GithubReporterService.DTO;
using GithubReporterService.DTO.Request;
using GithubReporterService.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubReporterService.Interface
{
	public interface IProjectService
	{
		Task<PagedResult<ProjectViewDTO>> SearchProject(ProjectPagedRequest request);
		Task<ProjectDetailDTO> GetProjectById(int projectId);
		Task CreateProject(CreateProjectDTO request);
		Task UpdateProject(UpdateProjectDTO request, int projectId);
		Task DeleteProject(int projectId);


	}
}
