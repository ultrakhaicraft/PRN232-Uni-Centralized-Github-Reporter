using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubReporterService.Utilities
{
	public class NotFoundException : Exception
	{
		public NotFoundException(string message) : base(message) { }

		public NotFoundException(string message, Exception innerException)
		   : base(message, innerException)
		{
		}
	}

	public class BadRequestException : Exception
	{
		public BadRequestException(string message) : base(message)
		{
		}

		public BadRequestException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}

	public class ValidationException : Exception
	{
		public List<string> Errors { get; }

		public ValidationException(List<string> errors)
			: base("One or more validation errors occurred")
		{
			Errors = errors ?? new List<string>();
		}

		public ValidationException(string error)
			: base("One or more validation errors occurred")
		{
			Errors = new List<string> { error };
		}
	}

	public class UnauthorizedException : Exception
	{
		public UnauthorizedException(string message) : base(message)
		{
		}
	}

	public class ForbiddenException : Exception
	{
		public ForbiddenException(string message) : base(message)
		{
		}
	}
}
