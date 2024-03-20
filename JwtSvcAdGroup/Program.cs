using JwtSvcAdGroup.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;

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
			var scopes = new Dictionary<string, string>();
			scopes.Add("api://25afc9bd-3f17-41d1-b3d3-29d78f1cf373/access_as_user", "access_as_user");
			scopes.Add("profile", "profile");
			scopes.Add("openid", "openid");
			builder.Services.AddSwaggerGen(options =>
			{
				options.AddSecurityRequirement(new OpenApiSecurityRequirement()
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "oauth2"
							},
							Scheme = "oauth2",
							Name = "oauth2",
							In = ParameterLocation.Header
						},
						new List <string> ()
					}
				});

				options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
				{
					Type = SecuritySchemeType.OAuth2,
					Flows = new OpenApiOAuthFlows
					{
						Implicit = new OpenApiOAuthFlow()
						{
							AuthorizationUrl = new Uri("https://login.microsoftonline.com/9e8754b6-f9cd-4aed-974d-a0ec0f3ed703/oauth2/v2.0/authorize"),
							TokenUrl = new Uri("https://login.microsoftonline.com/9e8754b6-f9cd-4aed-974d-a0ec0f3ed703/oauth2/v2.0/token"),
							Scopes = scopes
						}
					}
				});
			});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI(o =>
				{
					o.OAuthClientId("4c2d1c78-9caa-4e3a-9304-3bda78df07c5");
					o.OAuthAppName("JwtSvc PoC");
				});
			}

			app.UseHttpsRedirection();

			app.UseAuthentication();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}

//Test
