using Forecast.Api.Services;
using Microsoft.AspNetCore.Mvc;

using WeatherMVC.Services;

public class HomeController : Controller
{
    private readonly WeatherService _weatherService;
    private readonly IWeatherValidationService _weatherValidationService;

    public HomeController(WeatherService weatherService, IWeatherValidationService weatherValidationService)
    {
        _weatherService = weatherService;
        _weatherValidationService = weatherValidationService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> GetWeather(string cityName)
    {
        var city = await _weatherService.GetCityKeyAsync(cityName);
        if (city != null)
        {
            var weather = await _weatherService.GetWeatherAsync(city.Key);
            weather.City = city.LocalizedName;

            if (_weatherValidationService.IsWarningNeeded(city.Key, weather.HasRain))
            {
                ViewBag.Warning = $"Warning: Rain is expected in {weather.City} today!";
            }

            return Json(new
            {
                city = weather.City,
                maxTemperature = weather.MaxTemperature,
                minTemperature = weather.MinTemperature,
                warning = ViewBag.Warning
            });
        }

        return BadRequest("City not found");
    }
}