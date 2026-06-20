using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace weather_Service.Tests;

public class WeatherServiceTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public WeatherServiceTest(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_weather_details()
    {
        var response = await _client.GetAsync("/weatherforecast");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var forecasts = await response.Content.ReadFromJsonAsync<WeatherForecast[]>();

        Assert.NotNull(forecasts);
        Assert.Equal(5, forecasts.Length);

        foreach (var forecast in forecasts)
        {
            Assert.NotNull(forecast.Summary);
            Assert.Equal(32 + (int)(forecast.TemperatureC / 0.5556), forecast.TemperatureF);
        }
    }
}
