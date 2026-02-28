using GithubReporterService.DTO;
using GithubReporterService.DTO.Request;
using GithubReporterService.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubReporterService.Interface;

public interface IAccountService2
{
	public Task<StudentAccountDetailDTO> CreateStudentAccount(CreateStudentAccountDTO request);

	public Task<SupervisorAccountDetailDTO> CreateSupervisorAccount(CreateSupervisorAccountDTO request);

	public Task DeleteAccount(Guid accountId);

	public Task<AccountDetailDTO> GetAccountById(Guid accountId);

	public Task<PagedResult<AccountViewDTO>> SearchAccount(AccountPagedRequest request);

	public Task UpdateStudentAccount(UpdateStudentAccountDTO request, Guid accountId);

	public Task UpdateSupervisorAccount(UpdateSupervisorAccountDTO request, Guid accountId);

}
