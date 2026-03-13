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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubReporterService.Core
{
	public class StudentGradeService : IStudentGradeService
	{
		private readonly IGenericRepository<GradePerProject> _gradePerProjectRepository;
		private readonly IGenericRepository<Student> _studentRepository;
		private readonly IGenericRepository<Account> _accountRepository;
		private readonly IGenericRepository<Project> _projectRepository;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;

		public StudentGradeService(IGenericRepository<GradePerProject> gradePerProjectRepository, IGenericRepository<Student> studentRepository, IGenericRepository<Account> accountRepository, IGenericRepository<Project> projectRepository, IMapper mapper, IUnitOfWork unitOfWork)
		{
			_gradePerProjectRepository = gradePerProjectRepository;
			_studentRepository = studentRepository;
			_accountRepository = accountRepository;
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
				ProjectName = p.Project.ProjectName,
				StudentId = p.StudentId,
				StudentName = p.Student.Account.Name,
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
				ProjectName = p.Project.ProjectName,
				StudentId = p.StudentId,
				StudentName = p.Student.Account.Name,
				GradePerProjectId = p.GradePerProjectId,
				Grade = p.Grade
			});

			return result.ToList();

		}

		public async Task<ViewStudentGradeDTO> AddStudentGrade(AddStudentGradeDTO request)
		{
			//Check if there is duplicate grade for the same student and project
			var existingGrade = await _gradePerProjectRepository.GetQueryable()
				.FirstOrDefaultAsync(g => g.StudentId == request.StudentId && g.ProjectId == request.ProjectId);

			if (existingGrade != null)
			{
				throw new CRUDException($"Grade for student {request.StudentId} and project {request.ProjectId} already exists");
			}

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

			Student student = await _studentRepository.GetByIdAsync(grade.StudentId);
			if (student == null)
			{
				throw new NotFoundException($"Student with {grade.StudentId} not found");
			}

			Project project = await _projectRepository.GetByIdAsync(grade.ProjectId);
			if ( project == null)
			{
				throw new NotFoundException($"Project with {grade.ProjectId} not found");
			}

			Account account = await _accountRepository.GetByIdAsync(student.AccountId);

			if (account == null)
			{
				throw new NotFoundException($"Account with {student.AccountId} not found");
			}

			var result = new ViewStudentGradeDTO
			{
				ProjectId =grade.ProjectId,
				ProjectName = project.ProjectName,
				StudentId = grade.StudentId,
				StudentName = account.Name,
				GradePerProjectId = grade.GradePerProjectId,
				Grade = grade.Grade
			};

			return result;
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
