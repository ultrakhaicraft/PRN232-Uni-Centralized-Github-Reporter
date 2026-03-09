using GithubReporterRepository.Core;
using GithubReporterRepository.Interface;
using GithubReporterService.Core;
using GithubReporterService.Interface;
using GithubReporterService.Utilities;

namespace GithubReporterAPI.Utilities
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services)
		{
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddSingleton<TokenProvider>();
			services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
			services.AddScoped<IAuthenticationService, AuthenticationService>();
			services.AddScoped<IProjectService, ProjectService>();
			services.AddScoped<IGroupTeamService, GroupTeamService>();
			services.AddScoped<IGithubService, GithubService>();
			services.AddScoped<IStudentGradeService, StudentGradeService>();
			services.AddScoped<IReporterService, ReporterService>();
			services.AddScoped<IAccountService2, AccountService2>();	
			services.AddAutoMapper(cfg => cfg.AddMaps(typeof(MapperProfile).Assembly));

			return services;
		}
	}
}
