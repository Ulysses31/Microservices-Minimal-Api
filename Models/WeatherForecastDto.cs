namespace Services.Test.API.Models;

/// <summary>
/// Represents a weather forecast with details about temperature, date, and summary.
/// </summary>
public class WeatherForecastDto
{
  /// <summary>
  /// Primary key
  /// </summary>
  /// <value>string</value>
  public string Id { get; set; } = Guid.NewGuid().ToString();

  /// <summary>
  /// Gets or sets the date of the weather forecast.
  /// </summary>
  public DateOnly? Date { get; set; }

  /// <summary>
  /// Gets or sets the temperature in degrees Celsius.
  /// </summary>
  public int? TemperatureC { get; set; }

  /// <summary>
  /// Gets the temperature in degrees Fahrenheit, calculated from the Celsius value.
  /// </summary>
  public int? TemperatureF => 32 + (int)(TemperatureC! / 0.5556);

  /// <summary>
  /// Gets or sets a brief description of the weather conditions (e.g., "Sunny", "Rainy").
  /// </summary>
  public string? Summary { get; set; }
}

