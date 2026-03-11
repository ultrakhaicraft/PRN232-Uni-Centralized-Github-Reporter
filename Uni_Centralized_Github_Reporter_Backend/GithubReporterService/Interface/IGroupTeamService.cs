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
	public interface IGroupTeamService
	{
		public Task<PagedResult<GroupTeamViewDTO>> SearchGroupTeam(GroupTeamPagedRequest request);
		public Task<List<GroupTeamDetailDTO>> GetGroupTeamByProjectId(Guid projectId);
		public Task<GroupTeamDetailDTO> CreateGroupTeam(CreateGroupDTO request);
		public Task RemoveTeamMember(Guid accountId, Guid projectId);
		public Task UpdateTeamMemberRole(UpdateGroupDTO request);
		public Task<GroupTeamDetailDTO> AddTeamMember(CreateGroupDTO request);
		public Task<GroupTeamDetailDTO> GetAMemberFromAProject(Guid projectId, Guid accountId);



	}
}
