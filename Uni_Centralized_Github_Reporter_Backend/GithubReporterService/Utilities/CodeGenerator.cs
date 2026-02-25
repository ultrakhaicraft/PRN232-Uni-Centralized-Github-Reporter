using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubReporterService.Utilities
{
	public static class CodeGenerator
	{

		/// <summary>
		/// Generates student code in format FSExxxxxxxx where x is a digit
		/// </summary>
		/// <returns></returns>
		public static string GenerateStudentCode()
		{
			string digits = "0123456789";
			Random random = new Random();
			string prefix = "FSE";
			StringBuilder codeBuilder = new StringBuilder(prefix);
			for (int i = 0; i < 8; i++)
			{
				int index = random.Next(digits.Length);
				codeBuilder.Append(digits[index]);
			}

			return codeBuilder.ToString();
		}

		/// <summary>
		/// Generates student code in format FSRxxxxxxxx where x is a digit
		/// </summary>
		/// <returns></returns>
		public static string GenerateSupervisorCode()
		{
			string digits = "0123456789";
			Random random = new Random();
			string prefix = "FSR";
			StringBuilder codeBuilder = new StringBuilder(prefix);
			for (int i = 0; i < 8; i++)
			{
				int index = random.Next(digits.Length);
				codeBuilder.Append(digits[index]);
			}

			return codeBuilder.ToString();
		}
	}
}
