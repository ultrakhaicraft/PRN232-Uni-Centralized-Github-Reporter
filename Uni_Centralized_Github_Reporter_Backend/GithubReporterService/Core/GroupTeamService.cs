using AutoMapper;
using GithubReporterRepository.Interface;
using GithubReporterRepository.Models;
using GithubReporterService.DTO;
using GithubReporterService.DTO.Request;
using GithubReporterService.DTO.Response;
using GithubReporterService.Interface;
using GithubReporterService.Utilities;
using Microsoft.EntityFrameworkCore;


namespace GithubReporterService.Core
{
	public class GroupTeamService : IGroupTeamService
	{
		private readonly IGenericRepository<GroupTeam> _groupTeamRepository;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;

		public GroupTeamService(IGenericRepository<GroupTeam> groupTeamRepository, IMapper mapper, IUnitOfWork unitOfWork)
		{
			_groupTeamRepository = groupTeamRepository;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}

		public async Task CreateGroupTeam(CreateGroupDTO request)
		{
			var newGroup = _mapper.Map<GroupTeam>(request);

			await _groupTeamRepository.AddAsync(newGroup);
			await _unitOfWork.SaveChangesAsync();
		}

		public async Task DeleteGroupTeam(Guid groupId)
		{
			GroupTeam groupTeam = await _groupTeamRepository.GetByIdAsync(groupId);

			if (groupTeam == null)
			{
				throw new Utilities.NotFoundException($"Group Team with {groupId} not found");
			}

			_groupTeamRepository.Delete(groupTeam);
			await _unitOfWork.SaveChangesAsync();
		}

		public async Task<GroupTeamDetailDTO> GetGroupTeamById(Guid groupId)
		{
			GroupTeam project = await _groupTeamRepository.GetByIdAsync(groupId);

			if (project == null)
			{
				throw new Utilities.NotFoundException($"Group team with {groupId} not found");
			}

			var detailDTO = _mapper.Map<GroupTeamDetailDTO>(project);

			return detailDTO;
		}

		public async Task<PagedResult<GroupTeamViewDTO>> SearchGroupTeam(GroupTeamPagedRequest request)
		{
			//Search + Filter
			IQueryable<GroupTeam> groupTeams = _groupTeamRepository.GetQueryable();

			// Search by GroupName

			if (!string.IsNullOrEmpty(request.SearchKeyword))
			{
				groupTeams = groupTeams.Where(p => p.GroupName.Contains(request.SearchKeyword));
			}

			// Search By AccountId

			if(request.AccountId.HasValue)
			{
				groupTeams = groupTeams.Where(p => p.AccountId == request.AccountId.Value);
			}


			if (groupTeams == null || !groupTeams.Any())
			{
				throw new Utilities.NotFoundException("No group team found with the matching criteria");
			}

			var totalCount = await groupTeams.CountAsync();

			// Sorting By Group Name
			if (request.IsAscending.HasValue)
			{
				groupTeams = request.IsAscending.Value ? groupTeams.OrderBy(t => t.GroupName) :
					groupTeams.OrderByDescending(t => t.GroupName);
			}

			// Pagination

			int totalPage = (int)Math.Ceiling((double)totalCount / request.PageSize);

			var pagedData = PageConverter<GroupTeamViewDTO>.ToPagedResult(
				page: request.Page,
				pageSize: request.PageSize,
				totalItems: totalCount,
				queryableData: groupTeams.Select(p => new GroupTeamViewDTO
				{
					GroupId = p.GroupId,
					GroupCode = p.GroupCode,
					GroupName = p.GroupName,
					GroupRole = p.GroupRole,
					AccountId = p.AccountId,
					SupervisorId = p.SupervisorId,
					ProjectId = p.ProjectId,
				})
			).Result;

			return pagedData;
		}

		public async Task UpdateGroupTeam(UpdateGroupDTO request, Guid groupId)
		{
			GroupTeam group = await _groupTeamRepository.GetByIdAsync(groupId);

			if (group == null)
			{
				throw new Utilities.NotFoundException($"Group team with {groupId} not found");
			}

			var updatedGroup = _mapper.Map<GroupTeam>(request);

			_groupTeamRepository.Update(updatedGroup);
			await _unitOfWork.SaveChangesAsync();
		}
	}
}
