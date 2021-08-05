using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Core;

namespace Api.Controllers
{
    public enum Temperatures
    {
       Celsius,
       Fahrenheit,
       Kelvin
    }

    [ApiController]
    public class TemperatureController : ControllerBase
    {
        private readonly ILogger<TemperatureController> _logger;

        public TemperatureController(ILogger<TemperatureController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("temperature/convert/{temperature:double}/{from}")]
        public Temperature Convert(double temperature, Temperatures from)
        {
            var convertedTemperature = new Temperature();

            typeof(Temperature).GetProperty(from.ToString())?.SetValue(convertedTemperature, temperature);

            return convertedTemperature;
        }
    }
}
