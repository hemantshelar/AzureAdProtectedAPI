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

			builder.Services.AddSingleton<IAuthorizationHandler, AdminGroupAuthorizationHandler>();
			builder.Services.AddSingleton<IAuthorizationHandler, NonAdminGroupAuthorizationHandler>();
			builder.Services.AddAuthorization(options =>
			{

				options.AddPolicy("GroupAdmin", policy =>
				{
					var groupNameAdmin = builder.Configuration.GetSection("JwtGroups:JwtAdminTest").Value;
					policy.RequireAuthenticatedUser();
					policy.AddRequirements(new AdminGroupAuthorizationRequirement(groupNameAdmin));
				});

				options.AddPolicy("GroupNonAdmin", policy =>
				{
					var groupNameNonAdmin = builder.Configuration.GetSection("JwtGroups:JwtNonAdminTest").Value;
					policy.RequireAuthenticatedUser();
					policy.AddRequirements(new NonAdminGroupAuthorizationRequirement(groupNameNonAdmin));
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
