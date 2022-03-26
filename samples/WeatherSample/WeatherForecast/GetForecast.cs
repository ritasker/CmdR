using CmdR.Handler;

namespace WeatherSample.WeatherForecast;

public class GetForecast : RequestHandler<ForecastResponse[]>
{
    public override void Configure()
    {
        Get("/forecast");
    }
    
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public override Task<ForecastResponse[]> HandleAsync(CancellationToken ct)
    {
        return Task.FromResult(Enumerable.Range(1, 5).Select(index => new ForecastResponse
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray());
    }
}