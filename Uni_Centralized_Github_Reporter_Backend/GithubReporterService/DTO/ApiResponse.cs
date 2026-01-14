using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubReporterService.DTO
{
	public class ApiResponse<T> where T: class
	{
		public bool Success { get; set; }
		public string Message { get; set; } = string.Empty;
		public int StatusCode { get; set; }
		public T? Data { get; set; }
		public List<string>? Errors { get; set; }
		public DateTime Timestamp { get; set; } = DateTime.UtcNow;

		/// <summary>
		/// Success response factory method
		/// </summary>
		/// <param name="data"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		public static ApiResponse<T> SuccessResponse(T data, string message = "Success")
		{
			return new ApiResponse<T>
			{
				Success = true,
				Message = message,
				StatusCode = 200,
				Data = data
			};
		}

		/// <summary>
		/// Error response factory method, status code is default to 500
		/// </summary>
		/// <param name="message"></param>
		/// <param name="statusCode"></param>
		/// <param name="errors"></param>
		/// <returns></returns>
		public static ApiResponse<T> ErrorResponse(string message, int statusCode = 500, List<string>? errors = null)
		{
			return new ApiResponse<T>
			{
				Success = false,
				Message = message,
				StatusCode = statusCode,
				Errors = errors
			};
		}
	}
}
