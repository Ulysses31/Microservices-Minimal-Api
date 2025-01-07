using FluentValidation;
using Services.Test.API.Models;

namespace Services.Test.API.Validator;

/// <summary>
/// Validator class for <see cref="WeatherForecastDto"/> DTO using FluentValidation.
/// Ensures the data integrity of <see cref="WeatherForecastDto"/> properties.
/// </summary>
public class DtoValidator : AbstractValidator<WeatherForecastDto>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="DtoValidator"/> class.
  /// Defines validation rules for <see cref="WeatherForecastDto"/>.
  /// </summary>
  public DtoValidator()
  {
    RuleFor(x => x.Date)
        .NotNull()
        .WithMessage("Date is required");

    RuleFor(x => x.TemperatureC)
        .NotNull()
        .WithMessage("TemperatureC is required");
  }
}
