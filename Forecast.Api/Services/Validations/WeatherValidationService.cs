using Forecast.Api.Services;

namespace WeatherMVC.Services;

public class WeatherValidationService : IWeatherValidationService
{
    private readonly WeatherService _weatherService;

    public WeatherValidationService(WeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    public bool IsWarningNeeded(string cityKey, bool hasRain)
    {
        if (hasRain && _weatherService.IsWarningNeeded(cityKey))
        {
            _weatherService.AddWarning(cityKey);
            return true;
        }
        return false;
    }
}