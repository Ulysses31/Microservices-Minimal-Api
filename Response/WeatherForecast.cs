
using System.Text.Json.Serialization;

namespace Services.Test.API.Response;

/// <summary>
/// Represents a weather forecast with details about temperature, date, and summary.
/// </summary>
public class WeatherForecast
{
  /// <summary>
  /// Primary key
  /// </summary>
  /// <value>string</value>
  [JsonPropertyName("id")]
  public string Id { get; set; } = Guid.NewGuid().ToString();

  /// <summary>
  /// Gets or sets the date of the weather forecast.
  /// </summary>
  [JsonPropertyName("date")]
  public DateOnly? Date { get; set; }

  /// <summary>
  /// Gets or sets the temperature in degrees Celsius.
  /// </summary>
  [JsonPropertyName("temperatureC")]
  public int? TemperatureC { get; set; }

  /// <summary>
  /// Gets the temperature in degrees Fahrenheit, calculated from the Celsius value.
  /// </summary>
  [JsonPropertyName("temperatureF")]
  public int? TemperatureF => 32 + (int)(TemperatureC! / 0.5556);

  /// <summary>
  /// Gets or sets a brief description of the weather conditions (e.g., "Sunny", "Rainy").
  /// </summary>
  [JsonPropertyName("summary")]
  public string? Summary { get; set; }
}
