using Microsoft.AspNetCore.Mvc;

namespace DiaryApp.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Route("Array")]
    public IEnumerable<WeatherForecastModel> Get()
    {
        Console.WriteLine("GetWeatherForecast");
        return Enumerable.Range(1, 5).Select(index => new WeatherForecastModel
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
    //
    // [Route("Single")]
    // public WeatherForecastModel GetSingle()
    // {
    //     Console.WriteLine("GetWeatherForecast");
    //     return new WeatherForecastModel
    //     {
    //         Date = DateTime.Now.AddDays(1),
    //         TemperatureC = Random.Shared.Next(-20, 55),
    //         Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    //     };
    // }
}