using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web.Resource;
using Microsoft.Graph;

namespace JwtSvcAdGroup.Controllers
{
	//[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{
		private readonly GraphServiceClient _graphServiceClient;
		private static readonly string[] Summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		private readonly ILogger<WeatherForecastController> _logger;

		public WeatherForecastController(ILogger<WeatherForecastController> logger, GraphServiceClient graphServiceClient)
		{
			_logger = logger;
			_graphServiceClient = graphServiceClient;
		}

		[HttpGet(Name = "GetWeatherForecast")]
		[AllowAnonymous]
		public async Task<IEnumerable<WeatherForecast>> Get()
		{
			var user = await _graphServiceClient.Me.Request().GetAsync();
			return Enumerable.Range(1, 5).Select(index => new WeatherForecast
			{
				Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
				TemperatureC = Random.Shared.Next(-20, 55),
				Summary = Summaries[Random.Shared.Next(Summaries.Length)]
			})
			.ToArray();
		}

		[HttpGet()]
		[Authorize(Policy = "GroupNonAdmin")]
		[Route("NonAdminTestOperation")]
		public string NonAdminTestOeration()
		{
			var result = "This is a non-admin test operation";
			return result;
		}

		[HttpGet()]
		[Authorize(Policy = "GroupAdmin")]
		[Route("AdminTestOperation")]
		public string AdminTestOeration()
		{
			var result = "This is a admin test operation";
			return result;
		}
	}
}