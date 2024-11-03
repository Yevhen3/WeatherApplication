using Forecast.Api.Models;
using Forecast.Api.Services;
using Newtonsoft.Json;
using Forecast.Api.Models;
namespace WeatherMVC.Services;

public class WeatherService
{
    private readonly HttpClient _httpClient;
    private readonly WarningService _warningService;
    private readonly string _apiKey;

    public WeatherService(HttpClient httpClient, WarningService warningService, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _warningService = warningService;
        _apiKey = configuration["WeatherService:ApiKey"];
    }

    public async Task<City> GetCityKeyAsync(string cityName)
    {
        var response = await _httpClient.GetStringAsync($"http://dataservice.accuweather.com/locations/v1/cities/search?q={cityName}&apikey={_apiKey}");
        var cities = JsonConvert.DeserializeObject<List<City>>(response);
        return cities.FirstOrDefault();
    }

    public async Task<Weather> GetWeatherAsync(string cityKey)
    {
        var response = await _httpClient.GetStringAsync($"http://dataservice.accuweather.com/forecasts/v1/daily/1day/{cityKey}?apikey={_apiKey}");
        var forecast = JsonConvert.DeserializeObject<dynamic>(response);
        var weather = new Weather
        {
            MaxTemperature = (float)forecast.DailyForecasts[0].Temperature.Maximum.Value,
            MinTemperature = (float)forecast.DailyForecasts[0].Temperature.Minimum.Value,
            HasRain = (bool)forecast.DailyForecasts[0].Day.HasPrecipitation
        };
        return weather;
    }

    public bool IsWarningNeeded(string cityKey)
    {
        return _warningService.IsWarningNeeded(cityKey);
    }

    public void AddWarning(string cityKey)
    {
        _warningService.AddWarning(cityKey);
    }
}