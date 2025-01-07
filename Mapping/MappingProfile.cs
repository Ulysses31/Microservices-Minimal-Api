using AutoMapper;
using Services.Test.API.Models;
using Services.Test.API.Response;

namespace Services.Test.API.Mapping
{
  /// <summary>
  /// Mapping profile for AutoMapper configurations.
  /// Defines mappings between DTOs and entities used in the API.
  /// </summary>
  public class MappingProfile : Profile
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="MappingProfile"/> class.
    /// Configures mappings for the AutoMapper library.
    /// </summary>
    public MappingProfile()
    {
      CreateMap<WeatherForecastDto, WeatherForecast>();
    }
  }
}
