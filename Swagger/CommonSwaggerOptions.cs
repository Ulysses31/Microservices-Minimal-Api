using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Test.API
{
  /// <summary>
  /// Represents configuration options for Swagger documentation.
  /// </summary>
  public class CommonSwaggerOptions
  {
    /// <summary>
    /// The name used to identify the Swagger options section.
    /// </summary>
    public const string MySwagger = "Swagger";

    /// <summary>
    /// Gets or sets the description of the API.
    /// </summary>
    public static string? Description { get; set; }

    /// <summary>
    /// Gets or sets the title of the API.
    /// </summary>
    public static string? Title { get; set; }

    /// <summary>
    /// Gets or sets the terms of service URL for the API.
    /// </summary>
    public static string? TermsOfService { get; set; }

    /// <summary>
    /// Gets or sets additional options for API versioning and sunset policies.
    /// </summary>
    public static Options? Options { get; set; }

    /// <summary>
    /// Gets or sets the license information for the API.
    /// </summary>
    public static License? License { get; set; }

    /// <summary>
    /// Gets or sets the contact information for the API.
    /// </summary>
    public static Contact? Contact { get; set; }
  }

  /// <summary>
  /// Represents additional configuration options for API versioning and policies.
  /// </summary>
  public class Options
  {
    /// <summary>
    /// Gets or sets the description for deprecated API versions.
    /// </summary>
    public string? Deprecate_Version_Description { get; set; }

    /// <summary>
    /// Gets or sets the description of the API's sunset policy.
    /// </summary>
    public string? Sunset_Policy_Description { get; set; }
  }

  /// <summary>
  /// Represents licensing information for the API.
  /// </summary>
  public class License
  {
    /// <summary>
    /// Gets or sets the name of the license.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the URL where the license information is available.
    /// </summary>
    public string? Url { get; set; }
  }

  /// <summary>
  /// Represents contact information for the API.
  /// </summary>
  public class Contact
  {
    /// <summary>
    /// Gets or sets the name of the contact person or entity.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the email address of the contact person or entity.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the URL for the contact person or entity.
    /// </summary>
    public string? Url { get; set; }
  }
}
