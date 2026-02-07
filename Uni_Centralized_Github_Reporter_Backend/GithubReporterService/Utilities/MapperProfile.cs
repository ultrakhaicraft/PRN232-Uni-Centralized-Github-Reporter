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



		}
	}
}
