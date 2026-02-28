using AutoMapper;
using GithubReporterRepository.Enum;
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

		/// <summary>
		/// Create a new group team with a singluar account
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public async Task<GroupTeamDetailDTO> CreateGroupTeam(CreateGroupDTO request)
		{
			var newGroup = _mapper.Map<GroupTeam>(request);
			var addedGroup = await _groupTeamRepository.AddAsync(newGroup);
			await _unitOfWork.SaveChangesAsync();

			var groupTeamDetailDTO = _mapper.Map<GroupTeamDetailDTO>(addedGroup);
			return groupTeamDetailDTO;
		}

		/// <summary>
		/// Assign a team member to a project with some validation (Member =0, Leader = 1, Manager = 2)
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		/// <exception cref="Utilities.NotFoundException"></exception>
		/// <exception cref="Utilities.BadRequestException"></exception>
		public async Task<GroupTeamDetailDTO> AddTeamMember(CreateGroupDTO request)
		{
			// Check if group team exist by checking if there is a projectId exist
			var existingGroupTeam = await _groupTeamRepository.FindAsync(o => o.ProjectId == request.ProjectId);
			if (existingGroupTeam == null)
			{
				throw new Utilities.NotFoundException($"Group team with project id {request.ProjectId} not found");
			}

			// Check if the team member already exist
			var existingTeamMember = await _groupTeamRepository.FirstOrDefaultAsync(o => o.ProjectId == request.ProjectId && o.AccountId == request.AccountId);
			if (existingTeamMember != null)
			{
				throw new Utilities.BadRequestException($"Team member with account id {request.AccountId} already exist in project {request.ProjectId}");
			}

			// Check if there is a team leader exist in the group team 
			GroupTeam teamLeader = existingGroupTeam.FirstOrDefault(o => o.ProjectId == request.ProjectId && o.GroupRole == GroupRole.Leader.GetHashCode());
			if (teamLeader != null)
			{
				throw new Utilities.NotFoundException($"A Team Leader has already exist, please assign a different role");
			}


			var newGroup = _mapper.Map<GroupTeam>(request);
			var addedMember = await _groupTeamRepository.AddAsync(newGroup);
			await _unitOfWork.SaveChangesAsync();

			var groupTeamDetailDTO = _mapper.Map<GroupTeamDetailDTO>(addedMember);
			return groupTeamDetailDTO;
		}

		/// <summary>
		/// Updates the role of a team member within a group based on the specified request.
		/// </summary>
		/// <param name="request">An object containing the details required to update the team member's role, including the group and member
		/// identifiers and the new role to assign. Cannot be null.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		public async Task UpdateTeamMemberRole(UpdateGroupDTO request)
		{
			// Check if the team member  exist
			var existingTeamMember = await _groupTeamRepository.FirstOrDefaultAsync(o => o.ProjectId == request.ProjectId && o.AccountId == request.AccountId);
			if (existingTeamMember == null)
			{
				throw new Utilities.BadRequestException($"Team member with account id {request.AccountId} already exist in project {request.ProjectId}");
			}

			existingTeamMember.GroupRole = request.GroupRole;

			_groupTeamRepository.Update(existingTeamMember);
			await _unitOfWork.SaveChangesAsync();

		}

		/// <summary>
		///  Remove a team member from a project by deleting the corresponding GroupTeam entry based on the provided accountId and projectId.
		/// </summary>
		/// <param name="accountId"></param>
		/// <param name="projectId"></param>
		/// <returns></returns>
		/// <exception cref="Utilities.NotFoundException"></exception>
		public async Task RemoveTeamMember(Guid accountId, Guid projectId)
		{
			GroupTeam groupTeam = await _groupTeamRepository.FirstOrDefaultAsync(o=>o.AccountId==accountId && o.ProjectId==projectId);

			if (groupTeam == null)
			{
				throw new Utilities.NotFoundException($"Can't found the specific team member with {accountId} in project {projectId} not found");
			}

			_groupTeamRepository.Delete(groupTeam);
			await _unitOfWork.SaveChangesAsync();

			//Check if the team member still exist after deletion
			var checkTeamMember = await _groupTeamRepository.FirstOrDefaultAsync(o => o.AccountId == accountId && o.ProjectId == projectId);
			if (checkTeamMember != null)
			{
				throw new Utilities.CRUDException($"Failed to remove team member with account id {accountId} from project {projectId}");
			}
		}


		/// <summary>
		/// Get all the members in a specific group team by the projectId 
		/// </summary>
		/// <param name="groupId"></param>
		/// <returns></returns>
		/// <exception cref="Utilities.NotFoundException"></exception>
		public async Task<List<GroupTeamDetailDTO>> GetGroupTeamByProjectId(Guid projectId)
		{
			IEnumerable<GroupTeam> groupTeams = await _groupTeamRepository.FindAsync(o => o.ProjectId == projectId);

			if (groupTeams == null)
			{
				throw new Utilities.NotFoundException($"Group team with {projectId} not found");
			}

			// Convert to List of DTOs
			var memberDtos = groupTeams.Select(gt => _mapper.Map<GroupTeamDetailDTO>(gt)).ToList();
			return memberDtos;
		}


		/// <summary>
		/// Get a paginated list of all the group teams available with search and filter
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		/// <exception cref="Utilities.NotFoundException"></exception>
		public async Task<PagedResult<GroupTeamViewDTO>> SearchGroupTeam(GroupTeamPagedRequest request)
		{
			//Search + Filter
			IQueryable<GroupTeam> groupTeams = _groupTeamRepository.GetQueryable();

			

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

			// Sorting By Project Id
			if (request.IsAscending.HasValue)
			{
				groupTeams = request.IsAscending.Value ? groupTeams.OrderBy(t => t.ProjectId) :
					groupTeams.OrderByDescending(t => t.ProjectId);
			}

			// Pagination

			int totalPage = (int)Math.Ceiling((double)totalCount / request.PageSize);

			var pagedData = PageConverter<GroupTeamViewDTO>.ToPagedResult(
				page: request.Page,
				pageSize: request.PageSize,
				totalItems: totalCount,
				queryableData: groupTeams.Select(p => new GroupTeamViewDTO
				{			
					GroupRole = p.GroupRole,
					AccountId = p.AccountId,
					ProjectId = p.ProjectId,
				})
			).Result;

			return pagedData;
		}

	
	}
}
