using System.Xml.Serialization;
using Microsoft.AspNetCore.WebUtilities;

namespace Services.Test.API.Configuration;

/// <summary>
/// XmlResult class 
/// </summary>
/// <typeparam name="T">xml</typeparam>
public class XmlResult<T> : IResult
{
  // Create the serializer that will actually perform the XML serialization
  private static readonly XmlSerializer Serializer = new(typeof(T));

  // The object to serialize
  private readonly T _result;

  /// <summary>
  /// XmlResult constructor 
  /// </summary>
  /// <param name="result">xml</param>
  public XmlResult(T result)
  {
    _result = result;
  }

  /// <summary>
  /// ExecuteAsync function
  /// </summary>
  /// <param name="httpContext">HttpContext</param>
  /// <returns>Task</returns>
  public async Task ExecuteAsync(HttpContext httpContext)
  {
    // NOTE: best practice would be to pull this, we'll look at this shortly
    using var ms = new FileBufferingWriteStream();

    // Serialize the object synchronously then rewind the stream
    Serializer.Serialize(ms, _result);

    httpContext.Response.ContentType = "application/xml";
    await ms.DrainBufferAsync(httpContext.Response.Body);
  }
}
