using System.Net;
using System.Text;
using System.Text.Json;
using Serilog.Core;

namespace Services.Test.API
{
  /// <summary>
  /// Shared class
  /// </summary>
  public class Shared
  {
    /// <summary>
    /// GetHostIpAddress function
    /// </summary>
    /// <returns>String</returns>
    public string GetHostIpAddress()
    {
      IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());

      foreach (var item in ipHostInfo.AddressList)
      {
        if (item.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
        {
          IPAddress ipAddress = item;
          return ipAddress.ToString();
        }
      }

      return String.Empty;
    }

    /// <summary>
    /// Logging user activity
    /// </summary>
    /// <param name="context">HttpContext</param>
    /// <param name="next">Task</param>
    /// <param name="_logger">Logger</param>
    /// <param name="requesterInfo">RequesterInfo</param>
    /// <param name="jsonOptions">JsonSerializerOptions</param>
    /// <returns>void</returns>
    public async Task LogUserActivity(
      HttpContext context,
      Func<Task> next,
      Logger _logger,
      RequesterInfo requesterInfo,
      JsonSerializerOptions jsonOptions
    )
    {
      if (context.Request.Path.StartsWithSegments("/swagger"))
      {
        await next.Invoke();
        return;
      }

      // Do work that can write to the Response.
      _logger.Information($"===> Request started at: {DateTime.Now}");

      //****** Request body ********
      context.Request.EnableBuffering();
      using var reader = new StreamReader(
        context.Request.Body,
        Encoding.UTF8,
        leaveOpen: true
      );
      var reqBody = await reader.ReadToEndAsync();
      context.Request.Body.Position = 0;
      //***************************** 

      //****** Response body ********
      var orig = context.Response.Body;
      using var memoryStream = new MemoryStream();
      context.Response.Body = memoryStream;
      //***************************** 

      await next.Invoke();

      //****** Response body ********
      memoryStream.Seek(0, SeekOrigin.Begin);
      var respBody = await new StreamReader(memoryStream).ReadToEndAsync();
      memoryStream.Seek(0, SeekOrigin.Begin);
      await memoryStream.CopyToAsync(orig);
      //***************************** 

      requesterInfo = new RequesterInfo()
      {
        RequestMethod = context.Request.Method,
        RequestPath = context.Request.Path,
        RequestBody = reqBody,
        ResponseBody = respBody
      };

      _logger.Information(JsonSerializer.Serialize(requesterInfo, jsonOptions));

      _logger.Information($"===> Request finished at: {DateTime.Now}");
    }
  }
}
