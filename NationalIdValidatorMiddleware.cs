
namespace DependencyInjectionInDepth.Middlewares
{
	public class NationalCodeValidator
	{
		private readonly RequestDelegate _next;
		public NationalCodeValidator(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			string path = context.Request.Path;
			//Console.WriteLine($"Request path: {path}");

			//var segments = path.Split("/", StringSplitOptions.RemoveEmptyEntries);
			//if (segments != null)
			//{
			//	foreach (var segment in segments)
			//	{
			//		Console.WriteLine($"Segment: {segment}");
			//	}
			//}

			string nationalId = context.Request.Query["nationalId"];


			if (path == "/Test/getNationalId" && String.IsNullOrEmpty(nationalId))
			{
				context.Response.StatusCode = 400;
				await context.Response.WriteAsync("nationalId is empty!");
				return;
			}

			if (path == "/Test/getNationalId" && !String.IsNullOrEmpty(nationalId))
			{
				if (nationalId.Length != 10)
				{
					context.Response.StatusCode = 400;
					await context.Response.WriteAsync("nationalId is not valid!");
					return;
				}
				else
				{
					Console.WriteLine($"checking national ID: {nationalId}");
				}
			}

			await _next(context);

		}
	}
}
