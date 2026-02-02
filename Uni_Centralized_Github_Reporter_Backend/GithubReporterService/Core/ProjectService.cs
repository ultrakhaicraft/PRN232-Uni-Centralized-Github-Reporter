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

		public ProjectService(IUnitOfWork unitOfWork, IGenericRepository<Project> projectRepository)
		{
			_unitOfWork = unitOfWork;
			_projectRepository = projectRepository;
		}

		public async Task CreateProject(CreateProjectDTO request)
		{

			var newProject = _mapper.Map<Project>(request);

			await _projectRepository.AddAsync(newProject);
			await _unitOfWork.SaveChangesAsync();

		}

		public async Task DeleteProject(int projectId)
		{
			GithubReporterRepository.Models.Project category = await _projectRepository.GetByIdAsync(projectId);

			if (category == null)
			{
				throw new NotFoundException($"Project with {projectId} not found");
			}

			_projectRepository.Delete(category);
			await _unitOfWork.SaveChangesAsync();
		}

		public async Task<ProjectDetailDTO> GetProjectById(int projectId)
		{

			Project category = await _projectRepository.GetByIdAsync(projectId);

			if (category == null)
			{
				throw new NotFoundException($"Project with {projectId} not found");
			}

			var detailDTO =  _mapper.Map<ProjectDetailDTO>(category);

			return detailDTO;
		}

		public async Task<PagedResult<ProjectViewDTO>> SearchProject(ProjectPagedRequest request)
		{
			//Search + Filter
			IQueryable<Project> projects = _projectRepository.GetQueryable();

			if (projects == null || !projects.Any())
			{
				throw new NotFoundException("No projects found");
			}

			var totalCount = await projects.CountAsync();


			// Sorting when needed, sort by CategoryName alphabetically
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

		public async Task UpdateProject(UpdateProjectDTO request, int projectId)
		{
			Project project = await _projectRepository.GetByIdAsync(projectId);

			if (project == null)
			{
				throw new NotFoundException($"Project with {projectId} not found");
			}

			var updatedProject = _mapper.Map<Project>(request);

			_projectRepository.Update(updatedProject);
			await _unitOfWork.SaveChangesAsync();

		}
	}
}
