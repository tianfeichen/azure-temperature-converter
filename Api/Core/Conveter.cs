using System;

namespace Core
{
    /// <summary>
    /// Temperature in different units of measurements.
    /// Precision is kept up to 2 digits after decimal point.
    /// </summary>
    public class Temperature
    {
        private double _celsius;

        public double Celsius
        {
            get => Math.Round(_celsius, 2);
            set => _celsius = value;
        }

        public double Kelvin
        {
            get => Math.Round(_celsius + 273.15, 2);
            set => Celsius = value - 273.15;
        }

        public double Fahrenheit
        {
            get => Math.Round(_celsius * 9 / 5 + 32, 2);
            set => Celsius = (value - 32) * 5 / 9;
        }

        public static Temperature FromKelvin(double kelvin)
        {
            var temperature = new Temperature {Kelvin = kelvin};
            return temperature;
        }

        public static Temperature FromFahrenheit(double fahrenheit)
        {
            var temperature = new Temperature {Fahrenheit = fahrenheit};
            return temperature;
        }

        public static Temperature FromCelsius(double celsius)
        {
            var temperature = new Temperature {Celsius = celsius};
            return temperature;
        }
    }
}