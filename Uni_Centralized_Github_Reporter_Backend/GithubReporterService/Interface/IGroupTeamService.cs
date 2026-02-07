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
		Task<PagedResult<GroupTeamViewDTO>> SearchGroupTeam(GroupTeamPagedRequest request);
		Task<GroupTeamDetailDTO> GetGroupTeamById(Guid groupId);
		Task CreateGroupTeam(CreateGroupDTO request);
		Task UpdateGroupTeam(UpdateGroupDTO request, Guid groupId);
		Task DeleteGroupTeam(Guid groupId);
	}
}
