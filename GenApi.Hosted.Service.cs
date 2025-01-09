
using Serilog.Core;
using Services.Test.API;
using Services.Test.API.Configuration;

namespace GenApi.Hosted.Service;

/// <summary>
/// GenApiHostedService class   
/// </summary>
public class GenApiHostedService : IHostedService
{
  private readonly IHostApplicationLifetime _appLifetime;
  private readonly ILogger<GenApiHostedService> _logger;
  private readonly IConfiguration _configuration;

  /// <summary>
  /// GenApiHostedService constructor  
  /// </summary>
  /// <param name="appLifetime">IHostApplicationLifetime</param>
  /// <param name="logger">Logger</param>
  /// <param name="configuration">IConfiguration</param>
  public GenApiHostedService(
    IHostApplicationLifetime appLifetime,
    ILogger<GenApiHostedService> logger,
    IConfiguration configuration
  )
  {
    this._appLifetime = appLifetime;
    this._logger = logger;
    this._configuration = configuration;

    this._configuration!
      .GetSection(CommonGenApiOptions.MyGenApi)
      .Bind(new CommonGenApiOptions());
  }

  /// <summary>
  /// StartAsync function 
  /// </summary>
  /// <param name="cancellationToken">CancellationToken</param>
  /// <returns>Task</returns>
  public Task StartAsync(CancellationToken cancellationToken)
  {
    _appLifetime.ApplicationStarted.Register(OnStarted);
    _logger.LogInformation("===> GenApiHostedService started");
    return Task.CompletedTask;
  }

  /// <summary>
  /// StopAsync function 
  /// </summary>
  /// <param name="cancellationToken"></param>
  /// <returns>Task</returns>
  public Task StopAsync(CancellationToken cancellationToken)
  {
    _logger.LogInformation("===> GenApiHostedService finished");
    return Task.CompletedTask;
  }

  private async void OnStarted()
  {
    // Code to execute after app.Run() 
    string genApiUriV1 = $"http://{CommonGenApiOptions.Domain}:{CommonGenApiOptions.Port}{CommonGenApiOptions.UrlV1}";
    string genApiUriV2 = $"http://{CommonGenApiOptions.Domain}:{CommonGenApiOptions.Port}{CommonGenApiOptions.UrlV2}";

    // Generate the hosted API Version 1 client
    await new Shared().GenerateHostedApiDoc(
      genApiUriV1,
      "WeatherForecastClient",
      CommonGenApiOptions.Version1!,
      _logger
    );

    // Generate the hosted API Version 2 client
    await new Shared().GenerateHostedApiDoc(
     genApiUriV2,
     "WeatherForecastClient",
     CommonGenApiOptions.Version2!,
     _logger
   );
  }
}
