using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using AutoMapper;
using FluentValidation;
using Serilog;
using Services.Test.API.Models;
using Services.Test.API.Mapping;
using Services.Test.API.Response;
using Services.Test.API.Validator;
using GenApi.Hosted.Service;
using Services.Test.API.RateLimit;
using Services.Test.API.Configuration;

namespace Services.Test.API;

/// <summary>
/// Entry point of the application.
/// </summary>
public class Program
{
  /// <summary>
  /// Main method where the application starts execution.
  /// </summary>
  /// <param name="args">Command-line arguments passed to the application.</param>
  public static void Main(string[] args)
  {
    RequesterInfo requesterInfo = new RequesterInfo();
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
    IServiceCollection services = builder.Services;
    string envName = builder.Environment.EnvironmentName;

    JsonSerializerOptions jsonOptions = new JsonSerializerOptions
    {
      WriteIndented = true,
      Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    WeatherForecastDto[] forecast = [
      new WeatherForecastDto {
        Id = "38b7942a-8a8f-4a34-9744-e4dea6eaed78",
        Date = DateOnly.FromDateTime(DateTime.Now),
        TemperatureC = 25,
        Summary = "Hot"
      },
      new WeatherForecastDto {
        Id = "3db3a34a-9dcf-42e6-977f-d6bbb2329f16",
        Date = DateOnly.FromDateTime(DateTime.Now),
        TemperatureC = 15,
        Summary = "Cool"
      },
      new WeatherForecastDto {
        Id = "76d5e039-63b3-4c7f-bb8d-0847f729dcde",
        Date = DateOnly.FromDateTime(DateTime.Now),
        TemperatureC = 5,
        Summary = "Cold"
      },
      new WeatherForecastDto {
        Id = "1130f076-1d75-4977-8a50-323a4ecf8f4e",
        Date = DateOnly.FromDateTime(DateTime.Now),
        TemperatureC = 35,
        Summary = "Very Hot"
      },
      new WeatherForecastDto {
        Id = "2fa8d533-c8fd-45e6-8ee4-988e5b1d8d04",
        Date = DateOnly.FromDateTime(DateTime.Now),
        TemperatureC = 20,
        Summary = "Warm"
      }
    ];

    // Logging setup
    #region Logger
    // Configures HTTP logging and Serilog for structured logging.
    services.AddHttpLogging(options =>
    {
      options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
      options.ResponseHeaders.Add("minimal-openapi-example-api");
      options.MediaTypeOptions.AddText("application/json");
    });

    // Configure Serilog for structured logging with enriched properties
    var _logger = new LoggerConfiguration()
        .Enrich.WithProperty("Source", requesterInfo.reqInfo.SourceName)
        .Enrich.WithProperty("OSVersion", requesterInfo.reqInfo.OsVersion)
        .Enrich.WithProperty("ServerName", requesterInfo.hostInfo.Hostname)
        .Enrich.WithProperty("UserName", requesterInfo.hostInfo.Username)
        .Enrich.WithProperty("UserDomainName", requesterInfo.hostInfo.userDomainName)
        // Uncomment and implement to enrich logs with additional properties
        .Enrich.WithProperty("Address", requesterInfo.hostInfo.Addr)
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();

    // Add Serilog to the application's logging providers
    builder.Logging.AddSerilog(_logger);

    // Enable Serilog self-logging in development mode for debugging
    if (envName.Equals("Development", StringComparison.Ordinal))
    {
      Serilog.Debugging.SelfLog.Enable(Console.Error);
    }
    #endregion Logger

    // Add essential services to the dependency injection container
    services.AddAuthorization();

    // Rate Limiting setup
    services.CommonRateLimitSetup();

    // Configures Swagger/OpenAPI for API documentation.
    services.CommonSwaggerSetup($"{Assembly.GetExecutingAssembly().GetName().Name}.xml");

    // Mapping
    services.AddAutoMapper(typeof(MappingProfile));

    // Add FluentValidation to the dependency injection container
    services.AddScoped<IValidator<WeatherForecastDto>, DtoValidator>();

    // Add hosted service to the dependency injection container
    if (envName.Equals("Development", StringComparison.Ordinal))
    {
      services.AddHostedService<GenApiHostedService>();
    }

    // Build the application pipeline
    var app = builder.Build();
    var apiv1 = app.NewApiVersionSet("WeatherForecast v1").Build();
    var apiv2 = app.NewApiVersionSet("WeatherForecast v2").Build();

    // Middleware to log the start and end of each request
    app.Use(async (context, next) =>
    {
      await new Shared().LogUserActivity(
        context,
        next,
        _logger,
        requesterInfo,
        jsonOptions
      );
    });

    #region ENDPOINTS
    // Weather forecast endpoint setup
    var summaries = new[]
    {
      "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm",
      "Balmy", "Hot", "Sweltering", "Scorching"
    };

    // Maps a GET endpoint to retrieve a weather forecast.
    app.MapGet(
      "/api/v{version:apiVersion}/weatherforecast",
      (
        HttpContext context,
        IMapper mapper
      ) =>
    {
      // forecast = Enumerable.Range(1, 5).Select(index =>
      //         new WeatherForecast
      //         {
      //           Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
      //           TemperatureC = Random.Shared.Next(-20, 55),
      //           Summary = summaries[Random.Shared.Next(summaries.Length)]
      //         })
      //         .ToArray();
      // httpContext.Response.Headers.Append("Total", forecast.Length.ToString());

      WeatherForecast[] resp = mapper.Map<WeatherForecast[]>(forecast);

      if (string.IsNullOrEmpty(context.Response.ContentType))
        return Results.StatusCode(415);

      if (context.Response.ContentType!.Equals("application/xml"))
        return new XmlResult<WeatherForecast[]>(resp);

      return Results.Ok(resp);
    })
    .Produces<WeatherForecast[]>(200, "application/json", new[] { "application/xml" })
    .Produces(401)
    .Produces(415)
    .Produces(429)
    .Produces(500)
    .WithDescription("Get the weather forecast for the next five days.")
    .WithSummary("Get all the weather forecast.")
    .WithApiVersionSet(apiv2)
    .HasApiVersion(2.0)
    .RequireRateLimiting(CommonRateLimitExtension.FixedPolicy!);

    // GET by Id
    app.MapGet(
      "/api/v{version:apiVersion}/weatherforecast/{id}",
      (
        HttpContext context,
        string? id,
        IMapper mapper
      ) =>
      {
        try
        {
          if (string.IsNullOrEmpty(id))
            return Results.BadRequest("The id is required.");

          WeatherForecastDto? result = forecast
                                      .Where(f => f.Id == id)
                                      .FirstOrDefault();

          if (result == null)
            return Results.NotFound();

          var resp = mapper.Map<WeatherForecast>(result);

          if (string.IsNullOrEmpty(context.Response.ContentType))
            return Results.StatusCode(415);

          if (context.Response.ContentType!.Equals("application/xml"))
            return new XmlResult<WeatherForecast>(resp);

          return Results.Ok(resp);
        }
        catch (System.Exception ex)
        {
          return Results.BadRequest(ex.Message);
        }
      })
    .Produces<WeatherForecast>(200, "application/json", new[] { "application/xml" })
    .Produces(400)
    .Produces(401)
    .Produces(404)
    .Produces(415)
    .Produces(429)
    .Produces(500)
    .WithDescription("Get the weather forecast by id.")
    .WithSummary("Get specific weather forecast.")
    .WithApiVersionSet(apiv1)
    .HasApiVersion(1.0)
    .RequireRateLimiting(CommonRateLimitExtension.FixedPolicy!);

    // POST endpoint to create a new weather forecast.
    app.MapPost(
      "/api/v{version:apiVersion}/weatherforecast",
      async (
        HttpContext context,
        WeatherForecastDto data,
        IValidator<WeatherForecastDto> validator
      ) =>
      {
        try
        {
          // Validation
          var validationResult = await validator.ValidateAsync(data);
          if (!validationResult.IsValid)
          {
            // var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            //return Results.BadRequest(errors);
            return Results.ValidationProblem(validationResult.ToDictionary());
          }

          forecast = [.. forecast, data];

          if (string.IsNullOrEmpty(context.Response.ContentType))
            return Results.StatusCode(415);

          if (context.Response.ContentType!.Equals("application/xml"))
          {
            return new XmlResult<WeatherForecastDto>(data);
          }

          return Results.Created(
            "/api/v1/weatherforecast/{data.Id}",
            data
          );
        }
        catch (System.Exception ex)
        {
          return Results.BadRequest(ex.Message);
        }
      })
    .Accepts<WeatherForecastDto>("application/json")
    .Produces<WeatherForecast>(201, "application/json", new[] { "application/xml" })
    .Produces(400)
    .Produces(401)
    .Produces(415)
    .Produces(429)
    .Produces(500)
    .WithDescription("Insert a new weather forecast detail.")
    .WithSummary("Insert weather forecast.")
    .WithApiVersionSet(apiv1)
    .HasApiVersion(1.0)
    .RequireRateLimiting(CommonRateLimitExtension.FixedPolicy!);

    // PUT endpoint to update a weather forecast.
    app.MapMethods(
      "/api/v{version:apiVersion}/weatherforecast/{id}",
      [HttpMethod.Patch.Method],
      async (
        HttpContext context,
        string? id,
        WeatherForecastDto data,
        IValidator<WeatherForecastDto> validator
      ) =>
      {
        try
        {
          if (string.IsNullOrEmpty(id))
            return Results.BadRequest("The id is required.");

          // Validation
          var validationResult = await validator.ValidateAsync(data);
          if (!validationResult.IsValid)
          {
            // var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            //return Results.BadRequest(errors);
            return Results.ValidationProblem(validationResult.ToDictionary());
          }

          WeatherForecastDto? result = forecast
                                      .Where(f => f.Id == id)
                                      .FirstOrDefault();

          if (result == null)
            return Results.NotFound();

          data.Id = id;

          forecast = forecast
                      .Select(f => f.Id == id ? data : f)
                      .ToArray();

          return Results.NoContent();
        }
        catch (System.Exception ex)
        {
          return Results.BadRequest(ex.Message);
        }
      }
      )
    .Accepts<WeatherForecastDto>("application/json")
    .Produces(204)
    .Produces(400)
    .Produces(401)
    .Produces(404)
    .Produces(429)
    .Produces(500)
    .WithDescription("Modify an existing weather forecast by id.")
    .WithSummary("Edit weather forecast.")
    .WithApiVersionSet(apiv1)
    .HasApiVersion(1.0)
    .RequireRateLimiting(CommonRateLimitExtension.FixedPolicy!);

    // DELETE endpoint to remove a weather forecast.
    app.MapDelete(
      "/api/v{version:apiVersion}/weatherforecast/{id}",
      (
        HttpContext context,
        string? id
      ) =>
      {
        try
        {
          if (string.IsNullOrEmpty(id))
            return Results.BadRequest("The id is required.");

          WeatherForecastDto? result = forecast
                                      .Where(f => f.Id == id)
                                      .FirstOrDefault();

          if (result == null)
            return Results.NotFound();

          forecast = forecast.Where(f => f.Id != id).ToArray();

          return Results.NoContent();
        }
        catch (System.Exception ex)
        {
          return Results.BadRequest(ex.Message);
        }
      }
      )
    .Produces(204)
    .Produces(400)
    .Produces(401)
    .Produces(429)
    .Produces(500)
    .WithDescription("Remove existing weather forecast by id.")
    .WithSummary("Delete weather forecast.")
    .WithApiVersionSet(apiv1)
    .HasApiVersion(1.0)
    .RequireRateLimiting(CommonRateLimitExtension.FixedPolicy!);

    #endregion ENDPOINTS

    // Configure the HTTP request pipeline
    if (app.Environment.IsDevelopment())
      app.UseCommonSwagger();
    else
      // Enables HTTPS redirection for secure communication in production.
      app.UseHttpsRedirection();

    // Middleware configurations
    app.UseHttpLogging();

    app.UseAuthorization();

    app.UseRateLimiter();

    _logger.Information("===> Environment: {envName}", envName);
    _logger.Information("===> Host: {HostIpAddress}", requesterInfo.hostInfo.Addr);

    // if (envName.Equals("Development", StringComparison.Ordinal))
    // {
    //   _logger.Information(
    //       "--> Database Connection: Server={server},{port};Database={database};User={user};Password={password};TrustServerCertificate=True",
    //       server, port, database, user, password
    //   );
    // }

    // Run the application
    app.Run();


  }
}
