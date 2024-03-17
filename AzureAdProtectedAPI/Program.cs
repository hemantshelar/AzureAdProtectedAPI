
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

namespace AzureAdProtectedAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.AddRequiredScopeAuthorization();

			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(o => builder.Configuration.Bind("AzureAd", o));
			builder.Services.AddAuthorization(config =>
			{

				/*config.AddPolicy("default", policy =>
				{
					policy.RequireAuthenticatedUser();
					//policy.RequireScope("CardReader TestScope");
					//policy.RequireScope("api://d269eec9-5d0a-409d-bf69-0426237a7153/TestScope");
					//policy.RequireClaim("CardReader TestScope");
					//policy.RequireClaim("api://d269eec9-5d0a-409d-bf69-0426237a7153/TestScope");
				});*/

			});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthentication();
			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
