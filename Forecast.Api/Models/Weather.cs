namespace Forecast.Api.Models;

public class Weather
{
    public string City { get; set; }
    public float MaxTemperature { get; set; }
    public float MinTemperature { get; set; }
    public bool HasRain { get; set; }
}