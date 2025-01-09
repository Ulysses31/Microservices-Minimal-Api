namespace Services.Test.API.Configuration;

/// <summary>
/// Represents configuration options for GenApi.
/// </summary>
public class CommonGenApiOptions
{
  /// <summary>
  /// The name used to identify the GenApi options section.
  /// </summary>
  public const string MyGenApi = "GenApi";

  /// <summary>
  /// Gets or sets the domain of the version 1 API.
  /// </summary>
  /// <value>string</value>
  public static string? Domain { get; set; }

  /// <summary>
  /// Gets or sets the url of the version 1 API.
  /// </summary>
  /// <value>string</value>
  public static string? UrlV1 { get; set; }

  /// <summary>
  /// Gets or sets the url of the version 2 API.
  /// </summary>
  /// <value>string</value>
  public static string? UrlV2 { get; set; }

  /// <summary>
  /// Gets or sets the version 1 API.
  /// </summary>
  /// <value>string</value>
  public static string? Version1 { get; set; }

  /// <summary>
  /// Gets or sets the version 2 API.
  /// </summary>
  /// <value>string</value>
  public static string? Version2 { get; set; }

  /// <summary>
  /// Gets or sets the port of the API.
  /// </summary>
  /// <value>string</value>
  public static string? Port { get; set; }
}
