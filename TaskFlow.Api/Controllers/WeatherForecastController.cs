using Microsoft.AspNetCore.Mvc;

namespace TaskFlow.Api.Controllers
{
    [ApiController]  // enables API-specific behavior such as automatic request handling and validation.
    [Route("[controller]")]  // is a special route token; removes the Controller suffix from the class name.
    public class WeatherForecastController : ControllerBase  // ControllerBase provides common API features without returning HTML views.
    {
        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        [HttpGet(Name = "GetWeatherForecast")]  // gives the endpoint a name for generating links to this endpoint elsewhere in the application.
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
