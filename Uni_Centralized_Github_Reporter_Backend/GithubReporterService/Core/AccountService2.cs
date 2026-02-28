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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubReporterService.Core
{
	public class AccountService2 : IAccountService2
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IGenericRepository<Account> _accountRepository;
		private readonly IGenericRepository<Student> _studentRepository;
		private readonly IGenericRepository<Supervisor> _supervisorRepository;
		private readonly IMapper _mapper;

		public AccountService2(IUnitOfWork unitOfWork, IGenericRepository<Account> accountRepository, IGenericRepository<Student> studentRepository, IGenericRepository<Supervisor> supervisorRepository, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_accountRepository = accountRepository;
			_studentRepository = studentRepository;
			_supervisorRepository = supervisorRepository;
			_mapper = mapper;
		}

		public async Task<StudentAccountDetailDTO> CreateStudentAccount(CreateStudentAccountDTO request)
		{

			try
			{
				await _unitOfWork.BeginTransactionAsync();

				var account = _mapper.Map<GithubReporterRepository.Models.Account>(request);

				var accountId = Guid.NewGuid();

				account.AccountId = accountId;
				account.Role = Role.Student.GetHashCode();
				account.DateCreated = DateTime.UtcNow;
				account.Status = AccountStatus.Active.GetHashCode();


				var addedAccount = await _accountRepository.AddAsync(account);

				var student = new Student
				{
					StudentId = Guid.NewGuid(),
					AccountId = accountId,
					StudentCode = request.StudentCode

				};

				var addedStudent = await _studentRepository.AddAsync(student);

				await _unitOfWork.SaveChangesAsync();
				await _unitOfWork.CommitAsync();

				var studentDTO = new StudentAccountDetailDTO
				{
					AccountId = addedAccount.AccountId,
					Name = addedAccount.Name,
					Email = addedAccount.Email,
					Status = addedAccount.Status,
					Role = addedAccount.Role,
					DateCreated = addedAccount.DateCreated,
					GithubEmail = addedAccount.GithubEmail,
					StudentCode = addedStudent.StudentCode
				};

				return studentDTO;
			}
			catch (Exception)
			{
				await _unitOfWork.RollbackAsync();
				throw;
			}

		}

		public async Task<SupervisorAccountDetailDTO> CreateSupervisorAccount(CreateSupervisorAccountDTO request)
		{

			try
			{

				await _unitOfWork.BeginTransactionAsync();
				var account = _mapper.Map<Account>(request);

				var accountId = Guid.NewGuid();

				account.AccountId = accountId;
				account.Role = Role.Supervisor.GetHashCode();
				account.DateCreated = DateTime.UtcNow;
				account.Status = AccountStatus.Active.GetHashCode();

				var addedAccount = await _accountRepository.AddAsync(account);

				var supervisor = new Supervisor
				{
					SupervisorCode = request.SupervisorCode,
					SupervisorId = Guid.NewGuid(),
					AccountId = accountId
				};

				var addedSupervisor = await _supervisorRepository.AddAsync(supervisor);

				await _unitOfWork.SaveChangesAsync();
				await _unitOfWork.CommitAsync();

				var supervisorDTO = new SupervisorAccountDetailDTO
				{
					AccountId = addedAccount.AccountId,
					Name = addedAccount.Name,
					Email = addedAccount.Email,
					Status = addedAccount.Status,
					Role = addedAccount.Role,
					DateCreated = addedAccount.DateCreated,
					GithubEmail = addedAccount.GithubEmail,
					SupervisorCode = addedSupervisor.SupervisorCode
				};	

				return supervisorDTO;
			}
			catch (Exception)
			{
				await _unitOfWork.RollbackAsync();	
				throw;
			}

		}

		public async Task DeleteAccount(Guid accountId)
		{
			using var transaction = await _unitOfWork.BeginTransactionAsync();

			try
			{

				Account account = await _accountRepository.GetByIdAsync(accountId);

				if (account == null)
				{
					throw new NotFoundException($"Account with {accountId} not found");
				}

				//First, we need to check if the account is a student or supervisor and
				//delete the corresponding record in the Student or Supervisor table
				/*
				switch (account.Role)
				{
					case 1:
						Student student = await _studentRepository.GetByIdAsync(accountId);
						if (student != null)
						{
							_studentRepository.Delete(student);

						}
						break;
					case 2:
						Supervisor supervisor = await _supervisorRepository.GetByIdAsync(accountId);
						if (supervisor != null)
						{
							_supervisorRepository.Delete(supervisor);

						}
						break;
					default:
						throw new InvalidOperationException($"Invalid role {account.Role} for account with {accountId}");
				}
				*/

				_accountRepository.Delete(account);
				await _unitOfWork.SaveChangesAsync();
				await transaction.CommitAsync();

				//Check if the account is deleted successfully
				var deletedAccount = await _accountRepository.GetByIdAsync(accountId);
				if (deletedAccount != null)
				{
					throw new CRUDException($"Failed to delete account with {accountId}");
				}

			}
			catch (Exception)
			{
				await transaction.RollbackAsync();
				throw;
			}
		}

		public async Task<AccountDetailDTO> GetAccountById(Guid accountId)
		{
			Account account = await _accountRepository.GetByIdAsync(accountId);
			if (account == null)
			{
				throw new NotFoundException($"Account with ID {accountId} not found");
			}

			return account.Role switch
			{
				1 => await GetStudentDetailsAsync(account),
				2 => await GetSupervisorDetailsAsync(account),
				_ => throw new InvalidOperationException($"Invalid role '{account.Role}' for account {accountId}")
			};
		}

		private async Task<StudentAccountDetailDTO> GetStudentDetailsAsync(GithubReporterRepository.Models.Account account)
		{
			var student = await _studentRepository.FirstOrDefaultAsync(a => a.AccountId== account.AccountId);
			if (student == null)
			{
				throw new Utilities.NotFoundException($"Student details not found for account {account.AccountId}");
			}
			var detail= _mapper.Map<StudentAccountDetailDTO>(student);
			detail.AccountId = account.AccountId;
			detail.Name = account.Name;
			detail.Email = account.Email;
			detail.Status = account.Status;
			detail.Role = account.Role;
			detail.DateCreated = account.DateCreated;
			detail.GithubEmail = account.GithubEmail;

			return detail;

		}

		private async Task<SupervisorAccountDetailDTO> GetSupervisorDetailsAsync(GithubReporterRepository.Models.Account account)
		{
			var supervisor = await _supervisorRepository.FirstOrDefaultAsync(a => a.AccountId == account.AccountId);
			if (supervisor == null)
			{
				throw new Utilities.NotFoundException($"Supervisor details not found for account {account}");
			}
			var detail = _mapper.Map<SupervisorAccountDetailDTO>(supervisor);
			detail.AccountId = account.AccountId;
			detail.Name = account.Name;
			detail.Email = account.Email;
			detail.Status = account.Status;
			detail.Role = account.Role;
			detail.DateCreated = account.DateCreated;
			detail.GithubEmail = account.GithubEmail;
			return detail;
		}

		public async Task<PagedResult<AccountViewDTO>> SearchAccount(AccountPagedRequest request)
		{
			//Search + Filter
			IQueryable<Account> accounts = _accountRepository.GetQueryable();

			// Search by Name

			if (!string.IsNullOrEmpty(request.SearchKeyword))
			{
				accounts = accounts.Where(p => p.Name.Contains(request.SearchKeyword));
			}

			// Filter by Role
			if (request.Role.HasValue)
			{
				accounts = accounts.Where(p => p.Role == request.Role.Value);
			}

			// Filter by Status
			if (request.Status.HasValue)
			{
				accounts = accounts.Where(p => p.Status == request.Status.Value);
			}



			if (accounts == null || !accounts.Any())
			{
				throw new NotFoundException("No accounts found with the matching criteria");
			}

			var totalCount = await accounts.CountAsync();


			// Sorting when needed, sort by projectName alphabetically
			if (request.IsAscending.HasValue)
			{
				accounts = request.IsAscending.Value ? accounts.OrderBy(t => t.Name) :
					accounts.OrderByDescending(t => t.Name);
			}


			// Pagination

			int totalPage = (int)Math.Ceiling((double)totalCount / request.PageSize);

			var pagedData = PageConverter<AccountViewDTO>.ToPagedResult(
				page: request.Page,
				pageSize: request.PageSize,
				totalItems: totalCount,
				queryableData: accounts.Select(p => new AccountViewDTO
				{
					AccountId = p.AccountId,
					Name = p.Name,
					Email = p.Email,
					Status = p.Status,
					Role = p.Role,
					DateCreated = p.DateCreated

				})
			).Result;

			return pagedData;
		}

		public async Task UpdateStudentAccount(UpdateStudentAccountDTO request, Guid accountId)
		{
			try
			{
				await _unitOfWork.BeginTransactionAsync();
				Account account = await _accountRepository.GetByIdAsync(accountId);

				if (account == null)
				{
					throw new NotFoundException($"Account with {accountId} not found");
				}

				Student student = await _studentRepository.FirstOrDefaultAsync(o => o.AccountId == accountId);

				if (student == null)
				{
					throw new NotFoundException($"Student data with {accountId} not found");
				}

				account.Name = request.Name;
				account.Email = request.Email;
				account.Password = request.Password;
				account.GithubEmail = request.GithubEmail;
				student.StudentCode = request.StudentCode;



				_accountRepository.Update(account);
				_studentRepository.Update(student);
				await _unitOfWork.SaveChangesAsync();
				await _unitOfWork.CommitAsync();
			}
			catch (Exception)
			{
				await _unitOfWork.RollbackAsync();
				throw;
			}

		}

		public async Task UpdateSupervisorAccount(UpdateSupervisorAccountDTO request, Guid accountId)
		{
			try
			{
				await _unitOfWork.BeginTransactionAsync();
				Account account = await _accountRepository.GetByIdAsync(accountId);

				if (account == null)
				{
					throw new NotFoundException($"Account with {accountId} not found");
				}

				Supervisor supervisor = await _supervisorRepository.FirstOrDefaultAsync(o => o.AccountId == accountId);

				if (supervisor == null)
				{
					throw new NotFoundException($"Supervisor data with {accountId} not found");
				}

				account.Name = request.Name;
				account.Email = request.Email;
				account.Password = request.Password;
				account.GithubEmail = request.GithubEmail;
				supervisor.SupervisorCode = request.SupervisorCode;



				_accountRepository.Update(account);
				_supervisorRepository.Update(supervisor);
				await _unitOfWork.SaveChangesAsync();
				await _unitOfWork.CommitAsync();
			}
			catch (Exception)
			{
				await _unitOfWork.RollbackAsync();
				throw;
			}

		}
	}
}
