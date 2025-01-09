
using Serilog.Core;
using Services.Test.API;

namespace GenApi.Hosted.Service;

/// <summary>
/// GenApiHostedService class   
/// </summary>
public class GenApiHostedService : IHostedService
{
  private readonly IHostApplicationLifetime _appLifetime;
  private readonly ILogger<GenApiHostedService> _logger;

  /// <summary>
  /// GenApiHostedService constructor  
  /// </summary>
  /// <param name="appLifetime">IHostApplicationLifetime</param>
  /// <param name="logger">Logger</param>
  public GenApiHostedService(
    IHostApplicationLifetime appLifetime,
    ILogger<GenApiHostedService> logger
  )
  {
    this._appLifetime = appLifetime;
    this._logger = logger;
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
    await new Shared().GenerateHostedApiDoc(
      "http://localhost:5096/swagger/v1/swagger.json",
      "WeatherForecastClient",
      _logger
    );
  }
}
