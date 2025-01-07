using System.Reflection;
using System.Text.Json.Serialization;

namespace Services.Test.API;

/// <summary>
/// Represents information about the requester, including environment details and request/response data.
/// </summary>
public class RequesterInfo
{
  /// <summary>
  /// Gets or sets detailed information about the requester.
  /// </summary>
  [JsonPropertyName("system")]
  public RequesterSystem reqInfo { get; set; } = new RequesterSystem();

  /// <summary>
  /// Gets or sets detailed information about the host environment.
  /// </summary>
  [JsonPropertyName("host")]
  public HostInfo hostInfo { get; set; } = new HostInfo();

  /// <summary>
  /// Gets or sets the method at which the request was made.
  /// </summary>
  [JsonPropertyName("request_method")]
  public string? RequestMethod { get; set; }

  /// <summary>
  /// Gets or sets the path at which the request was made.
  /// </summary>
  [JsonPropertyName("request_path")]
  public string? RequestPath { get; set; }

  /// <summary>
  /// Gets or sets the time at which the request was made.
  /// </summary>
  [JsonPropertyName("request_time")]
  public DateTime RequestTimeAt { get; set; } = DateTime.Now;

  /// <summary>
  /// Gets or sets the body of the request sent to the API.
  /// </summary>
  [JsonPropertyName("request_body")]
  public string? RequestBody { get; set; }

  /// <summary>
  /// Gets or sets the body of the response received from the API.
  /// </summary>
  [JsonPropertyName("response_body")]
  public string? ResponseBody { get; set; }
}

/// <summary>
/// Represents system-level information about the requester.
/// </summary>
public class RequesterSystem
{
  /// <summary>
  /// Gets or sets the source name of the application, defaulting to the executing assembly's name.
  /// </summary>
  [JsonPropertyName("source_name")]
  public string? SourceName { get; set; } = Assembly.GetExecutingAssembly().GetName().Name;

  /// <summary>
  /// Gets the operating system version of the environment.
  /// </summary>
  [JsonPropertyName("os_version")]
  public string OsVersion { get; set; } = Environment.OSVersion.VersionString;
}

/// <summary>
/// Represents host-level information, such as hostname, user details, and IP address.
/// </summary>
public class HostInfo
{
  /// <summary>
  /// Gets the hostname of the machine where the application is running.
  /// </summary>
  [JsonPropertyName("host")]
  public string Hostname { get; set; } = System.Net.Dns.GetHostName();

  /// <summary>
  /// Gets the username of the user currently logged into the environment.
  /// </summary>
  [JsonPropertyName("username")]

  public string Username { get; set; } = Environment.UserName;

  /// <summary>
  /// Gets the domain name of the user currently logged into the environment.
  /// </summary>
  [JsonPropertyName("domain_name")]
  public string userDomainName { get; set; } = Environment.UserDomainName;

  /// <summary>
  /// Gets or sets the IP address of the host machine.
  /// </summary>
  [JsonPropertyName("address")]
  public string Addr { get; set; } = new Shared().GetHostIpAddress();
}
