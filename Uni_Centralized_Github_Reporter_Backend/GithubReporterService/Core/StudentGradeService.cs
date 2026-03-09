using AutoMapper;
using GithubReporterRepository.Core;
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
	public class StudentGradeService : IStudentGradeService
	{
		private readonly IGenericRepository<GradePerProject> _gradePerProjectRepository;
		private readonly IGenericRepository<Student> _studentRepository;
		private readonly IGenericRepository<Project> _projectRepository;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;

		public StudentGradeService(IGenericRepository<GradePerProject> gradePerProjectRepository, IGenericRepository<Student> studentRepository, IGenericRepository<Project> projectRepository, IMapper mapper, IUnitOfWork unitOfWork)
		{
			_gradePerProjectRepository = gradePerProjectRepository;
			_studentRepository = studentRepository;
			_projectRepository = projectRepository;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}








		/// <summary>
		/// Get grades by student id with pagination, student id is required
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		/// <exception cref="NotFoundException"></exception>
		public async Task<List<ViewStudentGradeDTO>> GetGradesByStudentIdAsync(Guid studentId)
		{
			IQueryable<GradePerProject> projects = _gradePerProjectRepository.GetQueryable();


			//Adjust conditions suitable
			if (!string.IsNullOrEmpty(studentId.ToString()))
			{
				projects = projects.Where(p => p.StudentId == studentId);
			}

			if (projects == null || !projects.Any())
			{
				throw new NotFoundException("No grade found with the matching criteria");
			}

			var result = projects.Select(p => new ViewStudentGradeDTO
			{
				ProjectId = p.ProjectId,
				StudentId = p.StudentId,
				GradePerProjectId = p.GradePerProjectId,
				Grade = p.Grade
			});

			return result.ToList();

		}

		/// <summary>
		/// Get grades by student id with pagination, student id is required
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		/// <exception cref="NotFoundException"></exception>
		public async Task<List<ViewStudentGradeDTO>> GetGradesByProjectIdAsync(Guid projectId)
		{
			IQueryable<GradePerProject> projects = _gradePerProjectRepository.GetQueryable();


			//Adjust conditions suitable
			if (!string.IsNullOrEmpty(projectId.ToString()))
			{
				projects = projects.Where(p => p.ProjectId == projectId);
			}

			if (projects == null || !projects.Any())
			{
				throw new NotFoundException("No grade found with the matching criteria");
			}



			var result = projects.Select(p => new ViewStudentGradeDTO
			{
				ProjectId = p.ProjectId,
				StudentId = p.StudentId,
				GradePerProjectId = p.GradePerProjectId,
				Grade = p.Grade
			});

			return result.ToList();

		}

		public async Task<ViewStudentGradeDTO> AddStudentGrade(AddStudentGradeDTO request)
		{
			var newGrade = _mapper.Map<GradePerProject>(request);
			newGrade.GradePerProjectId = Guid.NewGuid();
			var addedGrade = await _gradePerProjectRepository.AddAsync(newGrade);
			await _unitOfWork.SaveChangesAsync();
			var gradeDetailDTO = _mapper.Map<ViewStudentGradeDTO>(addedGrade);	
			return gradeDetailDTO;

		}

		public async Task UpdateStudentGrade(UpdateStudentGradeDTO request, Guid gradeId)
		{
			GradePerProject grade = await _gradePerProjectRepository.GetByIdAsync(gradeId);

			if (grade == null)
			{
				throw new NotFoundException($"Grade with {gradeId} not found");
			}

			grade.StudentId = request.StudentId;
			grade.ProjectId = request.ProjectId;
			grade.Grade = request.Grade;

			_gradePerProjectRepository.Update(grade);
			await _unitOfWork.SaveChangesAsync();

		}

		public async Task<ViewStudentGradeDTO> GetGradeById(Guid gradeId)
		{

			GradePerProject grade = await _gradePerProjectRepository.GetByIdAsync(gradeId);

			if (grade == null)
			{
				throw new NotFoundException($"Grade with {gradeId} not found");
			}

			var detailDTO = _mapper.Map<ViewStudentGradeDTO>(grade);

			return detailDTO;
		}

		public async Task DeleteStudentGrade(Guid gradeId)
		{
			GradePerProject grade = await _gradePerProjectRepository.GetByIdAsync(gradeId);
			if (grade == null)
			{
				throw new Utilities.NotFoundException($"Grade with {gradeId} not found");
			}
			_gradePerProjectRepository.Delete(grade);
			await _unitOfWork.SaveChangesAsync();

			//Check if the project is deleted successfully
			var deletedGrade = await _gradePerProjectRepository.GetByIdAsync(gradeId);
			if (deletedGrade != null)
			{
				throw new CRUDException($"Failed to delete grade with {gradeId}");
			}

		}
	}
}
