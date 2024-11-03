namespace Forecast.Api.Services;

public interface IWeatherValidationService
{
    bool IsWarningNeeded(string cityKey, bool hasRain);
}