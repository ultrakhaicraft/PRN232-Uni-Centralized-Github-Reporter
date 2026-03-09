using AutoMapper;
using GithubReporterRepository.Interface;
using GithubReporterRepository.Models;
using GithubReporterService.DTO;
using GithubReporterService.DTO.Request;
using GithubReporterService.DTO.Response;
using GithubReporterService.Interface;
using GithubReporterService.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubReporterService.Core
{
	public class ProjectService : IProjectService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IGenericRepository<Project> _projectRepository;
		private readonly IMapper _mapper;

		public ProjectService(IUnitOfWork unitOfWork, IGenericRepository<Project> projectRepository, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_projectRepository = projectRepository;
			_mapper = mapper;
		}

		public async Task<ProjectDetailDTO> CreateProject(CreateProjectDTO request)
		{

			var newProject = _mapper.Map<Project>(request);

			newProject.ProjectId = Guid.NewGuid();

			var addedProject = await _projectRepository.AddAsync(newProject);
			await _unitOfWork.SaveChangesAsync();

			if (addedProject == null)
			{
				throw new CRUDException("Failed to create project");
			}

			var projectDetailDTO = _mapper.Map<ProjectDetailDTO>(addedProject);
			return projectDetailDTO;

		}

		public async Task DeleteProject(Guid projectId)
		{
			GithubReporterRepository.Models.Project project = await _projectRepository.GetByIdAsync(projectId);

			if (project == null)
			{
				throw new NotFoundException($"Project with {projectId} not found");
			}

			_projectRepository.Delete(project);
			await _unitOfWork.SaveChangesAsync();

			//Check if the project is deleted successfully
			var deletedProject = await _projectRepository.GetByIdAsync(projectId);
			if (deletedProject != null)
			{
				throw new CRUDException($"Failed to delete project with {projectId}");
			}

		}

		public async Task<ProjectDetailDTO> GetProjectById(Guid projectId)
		{

			Project project = await _projectRepository.GetByIdAsync(projectId);

			if (project == null)
			{
				throw new NotFoundException($"Project with {projectId} not found");
			}

			var detailDTO =  _mapper.Map<ProjectDetailDTO>(project);

			return detailDTO;
		}

		public async Task<PagedResult<ProjectViewDTO>> SearchProject(ProjectPagedRequest request)
		{
			//Search + Filter
			IQueryable<Project> projects = _projectRepository.GetQueryable();

			// Search by ProjectName

			if (!string.IsNullOrEmpty(request.SearchKeyword))
			{
				projects = projects.Where(p => p.ProjectName.Contains(request.SearchKeyword));
			}


			if (projects == null || !projects.Any())
			{
				throw new NotFoundException("No projects found with the matching criteria");
			}

			var totalCount = await projects.CountAsync();


			// Sorting when needed, sort by projectName alphabetically
			if (request.IsAscending.HasValue)
			{
				projects = request.IsAscending.Value ? projects.OrderBy(t => t.ProjectName) :
					projects.OrderByDescending(t => t.ProjectName);
			}


			// Pagination

			int totalPage = (int)Math.Ceiling((double)totalCount / request.PageSize);

			var pagedData = PageConverter<ProjectViewDTO>.ToPagedResult(
				page: request.Page,
				pageSize: request.PageSize,
				totalItems: totalCount,
				queryableData: projects.Select(p => new ProjectViewDTO
				{
					ProjectId = p.ProjectId,
					ProjectName = p.ProjectName,
					GithubLink = p.GithubLink,
					Description = p.Description
				})
			).Result;

			return pagedData;
		}

		public async Task UpdateProject(UpdateProjectDTO request, Guid projectId)
		{
			Project project = await _projectRepository.GetByIdAsync(projectId);

			if (project == null)
			{
				throw new NotFoundException($"Project with {projectId} not found");
			}

			project.ProjectName = request.ProjectName;
			project.GithubLink = request.GithubLink;
			project.Description = request.Description;
			project.CreatedBy = request.CreatedBy;
			project.AccessToken = request.AccessToken;


			_projectRepository.Update(project);
			await _unitOfWork.SaveChangesAsync();

		}
	}
}
