using JwtSvcAdGroup.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;

namespace JwtSvcAdGroup
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"))
				.EnableTokenAcquisitionToCallDownstreamApi(o =>
				{

				})
				.AddMicrosoftGraph(builder.Configuration.GetSection("MicrosoftGraph"))
				.AddInMemoryTokenCaches();

			builder.Services.AddSingleton<IAuthorizationHandler, GroupAuthorizationHandler>();
			builder.Services.AddAuthorization(options =>
			{

				options.AddPolicy("GroupAdmin", policy =>
				{
					var groupName = builder.Configuration.GetSection("JwtServiceGroups:JwtServiceOperators").Value;
					policy.RequireAuthenticatedUser();
					policy.AddRequirements(new GroupAuthorizationRequirement(groupName));
				});

				options.AddPolicy("Group2", policy =>
				{
					policy.RequireAuthenticatedUser();
					policy.RequireClaim("groups", "group2");
				});

			});



			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

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
