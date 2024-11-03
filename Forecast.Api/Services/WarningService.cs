

using Newtonsoft.Json;

namespace Forecast.Api.Services;

public class WarningService
{
    private readonly string _warningFilePath;

    public WarningService(IConfiguration configuration)
    {
        
        _warningFilePath = configuration["WeatherService:WarningFilePath"];
    }

    public Dictionary<string, DateTime> GetWarnings()
    {
        if (System.IO.File.Exists(_warningFilePath))
        {
            var json = System.IO.File.ReadAllText(_warningFilePath);
            return JsonConvert.DeserializeObject<Dictionary<string, DateTime>>(json);
        }

        return new Dictionary<string, DateTime>();
    }

    public void SaveWarnings(Dictionary<string, DateTime> warnings)
    {
        var json = JsonConvert.SerializeObject(warnings);
        File.WriteAllText(_warningFilePath, json);
    }

    public bool IsWarningNeeded(string cityKey)
    {
        var warnings = GetWarnings();
        if (warnings.TryGetValue(cityKey, out DateTime lastWarning))
        {
            return (DateTime.Now - lastWarning).TotalDays >= 1;
        }
        return true;
    }

    public void AddWarning(string cityKey)
    {
        var warnings = GetWarnings();
        warnings[cityKey] = DateTime.Now;
        SaveWarnings(warnings);
    }
}
