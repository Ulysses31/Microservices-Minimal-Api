using System.Net;
using System.Text;
using System.Text.Json;
using NSwag;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.TypeScript;
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

    /// <summary>
    /// GenerateCSharpClient function 
    /// </summary>
    /// <param name="openApiLocation">string</param>
    /// <param name="className">string</param>
    /// <param name="_logger">Logger</param>
    /// <returns>Task</returns>
    public async Task GenerateCSharpClient(
      string openApiLocation,
      string className,
      Logger _logger
    )
    {
      var document = await OpenApiDocument.FromUrlAsync(openApiLocation);

      var settings = new CSharpClientGeneratorSettings
      {
        UseBaseUrl = false,
        ClassName = className,
        GenerateClientInterfaces = true,
        CSharpGeneratorSettings = { Namespace = "HttpClients" },
      };

      var generator = new CSharpClientGenerator(document, settings);
      var generatedCode = generator.GenerateFile();
      var path = $"./ApiGen/{settings.CSharpGeneratorSettings.Namespace}/{settings.ClassName}_client.cs";

      _logger.Information($"===> Generated http-client file path: {path}");

      var file = new FileInfo(path);
      file.Directory?.Create();
      await File.WriteAllTextAsync(file.FullName, generatedCode);
    }

    /// <summary>
    /// GenerateCSharpController function 
    /// </summary>
    /// <param name="openApiLocation">string</param>
    /// <param name="className">string</param>
    /// <param name="_logger">Logger</param>
    /// <returns>Task</returns>
    public async Task GenerateCSharpController(
      string openApiLocation,
      string className,
      Logger _logger
    )
    {
      var document = await OpenApiDocument.FromUrlAsync(openApiLocation);

      var settings = new CSharpControllerGeneratorSettings
      {
        ControllerBaseClass = "ControllerBase",
        GenerateClientClasses = true,
        GenerateClientInterfaces = true,
        CSharpGeneratorSettings = { Namespace = "Controllers" },
      };

      var generator = new CSharpControllerGenerator(document, settings);
      var generatedCode = generator.GenerateFile();
      var path = $"./ApiGen/{settings.CSharpGeneratorSettings.Namespace}/{className}_controller.cs";

      _logger.Information($"===> Generated controller file path: {path}");

      var file = new FileInfo(path);
      file.Directory?.Create();
      await File.WriteAllTextAsync(file.FullName, generatedCode);
    }


    /// <summary>
    /// GenerateTypeScriptClient function 
    /// </summary>
    /// <param name="openApiLocation">string</param>
    /// <param name="className">string</param>
    /// <param name="_logger">Logger</param>
    /// <returns>Task</returns>
    public async Task GenerateTypeScriptClient(
      string openApiLocation, 
      string className,
      Logger _logger
    ) { 
      var document = await OpenApiDocument.FromUrlAsync(openApiLocation); 
      
      var settings = new TypeScriptClientGeneratorSettings { 
        ClassName = className, 
        Template = TypeScriptTemplate.Fetch, 
        TypeScriptGeneratorSettings = { Namespace = "HttpClients" }, 
      }; var generator = new TypeScriptClientGenerator(document, settings); 
      
      var generatedCode = generator.GenerateFile(); 
      var path = $"./ApiGen/{settings.TypeScriptGeneratorSettings.Namespace}/{settings.ClassName}.ts";
    
      _logger.Information($"===> Generated typescript-client file path: {path}");

      var file = new FileInfo(path); 
      file.Directory?.Create(); 
      await File.WriteAllTextAsync(file.FullName, generatedCode); 
    }


  }
}
