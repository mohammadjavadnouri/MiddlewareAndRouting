using DependencyInjectionInDepth.Domain.Entities;
using DependencyInjectionInDepth.Infra;
using System;
using System.Text;

namespace DependencyInjectionInDepth.Middlewares
{
	public class LogRequestBody
	{
		private readonly RequestDelegate _next;
		public LogRequestBody(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context, AppDbContext dbContext)
		{
			if (context.Request.Method == HttpMethods.Post ||
					context.Request.Method == HttpMethods.Put)
			{
				context.Request.EnableBuffering();
				using StreamReader reader = new StreamReader(
					context.Request.Body,
					encoding: Encoding.UTF8,
					detectEncodingFromByteOrderMarks: false,
					bufferSize: 1024,
					leaveOpen: true);
				var body = await reader.ReadToEndAsync();
				context.Request.Body.Position = 0;

				RequestLog requestLog = new RequestLog() { body = body };
				dbContext.RequestLogs.Add(requestLog);
				await dbContext.SaveChangesAsync();

				Console.WriteLine($"request body: {body}");
			}


			await _next(context);
		}
	}
}
