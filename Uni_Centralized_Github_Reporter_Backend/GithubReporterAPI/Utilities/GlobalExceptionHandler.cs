using GithubReporterService.DTO;
using GithubReporterService.Utilities;
using System.Net;
using System.Text.Json;

namespace GithubReporterAPI.Utilities
{
	public class GlobalExceptionHandler
	{

		private readonly RequestDelegate _next;
		private readonly ILogger<GlobalExceptionHandler> _logger;
		private readonly IWebHostEnvironment _env;

		public GlobalExceptionHandler(
			RequestDelegate next,
			ILogger<GlobalExceptionHandler> logger,
			IWebHostEnvironment env)
		{
			_next = next;
			_logger = logger;
			_env = env;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception exception)
			{
				await HandleExceptionAsync(context, exception);
			}
		}

		private async Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			// Log the full exception details
			_logger.LogError(
				exception,
				"An error occurred while processing the request. Path: {Path}, Method: {Method}",
				context.Request.Path,
				context.Request.Method
			);

			context.Response.ContentType = "application/json";

			// Map exception to status code and response
			var (statusCode, message, errors) = MapException(exception);

			context.Response.StatusCode = statusCode;

			var response = ApiResponse<object>.ErrorResponse(
				message,
				statusCode,
				errors
			);

			var options = new JsonSerializerOptions
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				WriteIndented = _env.IsDevelopment() // Pretty print in development
			};

			var json = JsonSerializer.Serialize(response, options);

			await context.Response.WriteAsync(json);
		}

		private (int StatusCode, string Message, List<string>? Errors) MapException(Exception exception)
		{
			return exception switch
			{
				NotFoundException notFoundEx => (
					StatusCode: (int)HttpStatusCode.NotFound,
					notFoundEx.Message,
					Errors: null
				),

				BadRequestException badRequestEx => (
					StatusCode: (int)HttpStatusCode.BadRequest,
					badRequestEx.Message,
					Errors: null
				),

				ValidationException validationEx => (
					StatusCode: (int)HttpStatusCode.BadRequest,
					Message: "Validation failed",
					validationEx.Errors
				),

				UnauthorizedException unauthorizedEx => (
					StatusCode: (int)HttpStatusCode.Unauthorized,
					Message: "You are not authorized to access this resource or use the method.",
					Errors: null
				),

				ForbiddenException forbiddenEx => (
					StatusCode: (int)HttpStatusCode.Forbidden,
					Message: "You do not have permission to access this resource or use the method.",
					Errors: null
				),

				CRUDException crudEx => (
					StatusCode: (int)HttpStatusCode.InternalServerError,
					Message: crudEx.Message,
					Errors: null
				),

				// Catch-all for unexpected exceptions
				_ => (
					StatusCode: (int)HttpStatusCode.InternalServerError,
					Message: "An internal server error occurred. Please try again later.",
					Errors: new List<string> { exception.StackTrace ?? "No stack trace available" }

				)
			};
		}
	}
}
