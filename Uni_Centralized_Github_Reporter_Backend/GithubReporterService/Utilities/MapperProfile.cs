using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GithubReporterRepository.Models;
using GithubReporterService.DTO.Request;
using GithubReporterService.DTO.Response;


namespace GithubReporterService.Utilities
{
	public class MapperProfile : Profile 
	{
		public MapperProfile()
		{
			CreateMap<Project, ProjectViewDTO>();
			CreateMap<Project, ProjectDetailDTO>();
			CreateMap<CreateProjectDTO, Project>();
			CreateMap<UpdateProjectDTO, Project>();

			CreateMap<GradePerProject, ViewStudentGradeDTO>();
			CreateMap<AddStudentGradeDTO, GradePerProject>();
			CreateMap<UpdateStudentGradeDTO, GradePerProject>();


			CreateMap<GroupTeam, GroupTeamDetailDTO>();
			CreateMap<GroupTeam, GroupTeamViewDTO>();
			CreateMap<CreateGroupDTO, GroupTeam>();
			CreateMap<UpdateGroupDTO, GroupTeam>();

			CreateMap<Account, AccountViewDTO>();
			CreateMap<Account, StudentAccountDetailDTO>();
			CreateMap<Student, StudentAccountDetailDTO>();
			CreateMap<Account, SupervisorAccountDetailDTO>();
			CreateMap<Supervisor, SupervisorAccountDetailDTO>();
			CreateMap<CreateStudentAccountDTO, Account>();
			CreateMap<UpdateStudentAccountDTO, Account>();
			CreateMap<CreateSupervisorAccountDTO, Account>();
			CreateMap<UpdateSupervisorAccountDTO, Account>();



		}
	}
}
