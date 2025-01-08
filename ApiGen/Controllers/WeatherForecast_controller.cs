//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v14.2.0.0 (NJsonSchema v11.1.0.0 (Newtonsoft.Json v13.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

#pragma warning disable 108 // Disable "CS0108 '{derivedDto}.ToJson()' hides inherited member '{dtoBase}.ToJson()'. Use the new keyword if hiding was intended."
#pragma warning disable 114 // Disable "CS0114 '{derivedDto}.RaisePropertyChanged(String)' hides inherited member 'dtoBase.RaisePropertyChanged(String)'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword."
#pragma warning disable 472 // Disable "CS0472 The result of the expression is always 'false' since a value of type 'Int32' is never equal to 'null' of type 'Int32?'
#pragma warning disable 612 // Disable "CS0612 '...' is obsolete"
#pragma warning disable 649 // Disable "CS0649 Field is never assigned to, and will always have its default value null"
#pragma warning disable 1573 // Disable "CS1573 Parameter '...' has no matching param tag in the XML comment for ...
#pragma warning disable 1591 // Disable "CS1591 Missing XML comment for publicly visible type or member ..."
#pragma warning disable 8073 // Disable "CS8073 The result of the expression is always 'false' since a value of type 'T' is never equal to 'null' of type 'T?'"
#pragma warning disable 3016 // Disable "CS3016 Arrays as attribute arguments is not CLS-compliant"
#pragma warning disable 8603 // Disable "CS8603 Possible null reference return"
#pragma warning disable 8604 // Disable "CS8604 Possible null reference argument for parameter"
#pragma warning disable 8625 // Disable "CS8625 Cannot convert null literal to non-nullable reference type"
#pragma warning disable 8765 // Disable "CS8765 Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes)."

namespace Controllers
{
    using System = global::System;

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "14.2.0.0 (NJsonSchema v11.1.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public interface IController
    {

        /// <returns>OK</returns>

        System.Threading.Tasks.Task ApiGenAsync();

        /// <summary>
        /// Get specific weather forecast.
        /// </summary>

        /// <remarks>
        /// Get the weather forecast by id.
        /// </remarks>

        /// <returns>OK</returns>

        System.Threading.Tasks.Task<WeatherForecast> WeatherforecastGETAsync(string id);

        /// <summary>
        /// Edit weather forecast.
        /// </summary>

        /// <remarks>
        /// Modify an existing weather forecast by id.
        /// </remarks>



        /// <returns>No Content</returns>

        System.Threading.Tasks.Task WeatherforecastPATCHAsync(string id, WeatherForecastDto body);

        /// <summary>
        /// Delete weather forecast.
        /// </summary>

        /// <remarks>
        /// Remove existing weather forecast by id.
        /// </remarks>

        /// <returns>No Content</returns>

        System.Threading.Tasks.Task WeatherforecastDELETEAsync(string id);

        /// <summary>
        /// Insert weather forecast.
        /// </summary>

        /// <remarks>
        /// Insert a new weather forecast detail.
        /// </remarks>

        /// <returns>Created</returns>

        System.Threading.Tasks.Task<WeatherForecast> WeatherforecastPOSTAsync(WeatherForecastDto body);

    }

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "14.2.0.0 (NJsonSchema v11.1.0.0 (Newtonsoft.Json v13.0.0.0))")]

    public partial class Controller : ControllerBase
    {
        private IController _implementation;

        public Controller(IController implementation)
        {
            _implementation = implementation;
        }

        /// <returns>OK</returns>
        [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.Route("api/v1/api-gen")]
        public System.Threading.Tasks.Task ApiGen()
        {

            return _implementation.ApiGenAsync();
        }

        /// <summary>
        /// Get specific weather forecast.
        /// </summary>
        /// <remarks>
        /// Get the weather forecast by id.
        /// </remarks>
        /// <returns>OK</returns>
        [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.Route("api/v1/weatherforecast/{id}")]
        public System.Threading.Tasks.Task<WeatherForecast> WeatherforecastGET(string id)
        {

            return _implementation.WeatherforecastGETAsync(id);
        }

        /// <summary>
        /// Edit weather forecast.
        /// </summary>
        /// <remarks>
        /// Modify an existing weather forecast by id.
        /// </remarks>
        /// <returns>No Content</returns>
        [Microsoft.AspNetCore.Mvc.HttpPatch, Microsoft.AspNetCore.Mvc.Route("api/v1/weatherforecast/{id}")]
        public System.Threading.Tasks.Task WeatherforecastPATCH(string id, [Microsoft.AspNetCore.Mvc.FromBody] WeatherForecastDto body)
        {

            return _implementation.WeatherforecastPATCHAsync(id, body);
        }

        /// <summary>
        /// Delete weather forecast.
        /// </summary>
        /// <remarks>
        /// Remove existing weather forecast by id.
        /// </remarks>
        /// <returns>No Content</returns>
        [Microsoft.AspNetCore.Mvc.HttpDelete, Microsoft.AspNetCore.Mvc.Route("api/v1/weatherforecast/{id}")]
        public System.Threading.Tasks.Task WeatherforecastDELETE(string id)
        {

            return _implementation.WeatherforecastDELETEAsync(id);
        }

        /// <summary>
        /// Insert weather forecast.
        /// </summary>
        /// <remarks>
        /// Insert a new weather forecast detail.
        /// </remarks>
        /// <returns>Created</returns>
        [Microsoft.AspNetCore.Mvc.HttpPost, Microsoft.AspNetCore.Mvc.Route("api/v1/weatherforecast")]
        public System.Threading.Tasks.Task<WeatherForecast> WeatherforecastPOST([Microsoft.AspNetCore.Mvc.FromBody] WeatherForecastDto body)
        {

            return _implementation.WeatherforecastPOSTAsync(body);
        }

    }

    /// <summary>
    /// Represents a weather forecast with details about temperature, date, and summary.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.2.0.0 (NJsonSchema v11.1.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class WeatherForecast
    {
        /// <summary>
        /// Primary key
        /// </summary>
        [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the date of the weather forecast.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("date", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [Newtonsoft.Json.JsonConverter(typeof(DateFormatConverter))]
        public System.DateTimeOffset? Date { get; set; }

        /// <summary>
        /// Gets or sets the temperature in degrees Celsius.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("temperatureC", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int? TemperatureC { get; set; }

        /// <summary>
        /// Gets the temperature in degrees Fahrenheit, calculated from the Celsius value.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("temperatureF", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int? TemperatureF { get; set; }

        /// <summary>
        /// Gets or sets a brief description of the weather conditions (e.g., "Sunny", "Rainy").
        /// </summary>
        [Newtonsoft.Json.JsonProperty("summary", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Summary { get; set; }

    }

    /// <summary>
    /// Represents a weather forecast with details about temperature, date, and summary.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.2.0.0 (NJsonSchema v11.1.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class WeatherForecastDto
    {
        /// <summary>
        /// Primary key
        /// </summary>
        [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the date of the weather forecast.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("date", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [Newtonsoft.Json.JsonConverter(typeof(DateFormatConverter))]
        public System.DateTimeOffset? Date { get; set; }

        /// <summary>
        /// Gets or sets the temperature in degrees Celsius.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("temperatureC", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int? TemperatureC { get; set; }

        /// <summary>
        /// Gets the temperature in degrees Fahrenheit, calculated from the Celsius value.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("temperatureF", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int? TemperatureF { get; set; }

        /// <summary>
        /// Gets or sets a brief description of the weather conditions (e.g., "Sunny", "Rainy").
        /// </summary>
        [Newtonsoft.Json.JsonProperty("summary", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Summary { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.2.0.0 (NJsonSchema v11.1.0.0 (Newtonsoft.Json v13.0.0.0))")]
    internal class DateFormatConverter : Newtonsoft.Json.Converters.IsoDateTimeConverter
    {
        public DateFormatConverter()
        {
            DateTimeFormat = "yyyy-MM-dd";
        }
    }


}

#pragma warning restore  108
#pragma warning restore  114
#pragma warning restore  472
#pragma warning restore  612
#pragma warning restore 1573
#pragma warning restore 1591
#pragma warning restore 8073
#pragma warning restore 3016
#pragma warning restore 8603
#pragma warning restore 8604
#pragma warning restore 8625