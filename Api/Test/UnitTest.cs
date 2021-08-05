using Core;
using Xunit;

namespace Test
{
    public class UnitTest
    {
        private readonly Temperature _temperature;

        public UnitTest()
        {
            _temperature = new Temperature();
        }

        [Theory]
        [InlineData(0, 32)]
        [InlineData(1, 33.8)]
        [InlineData(42, 107.6)]
        [InlineData(100, 212)]
        [InlineData(-1, 30.2)]
        [InlineData(-100, -148)]
        public void TestCToF(double c, double expected)
        {
            _temperature.Celsius = c;

            var f = _temperature.Fahrenheit;

            Assert.Equal(expected, f);
        }

        [Theory]
        [InlineData(0, 273.15)]
        [InlineData(1, 274.15)]
        [InlineData(42, 315.15)]
        [InlineData(100, 373.15)]
        [InlineData(-1, 272.15)]
        [InlineData(-100, 173.15)]
        public void TestCToK(double c, double expected)
        {
            _temperature.Celsius = c;

            var k = _temperature.Kelvin;

            Assert.Equal(expected, k);
        }

        [Theory]
        [InlineData(0, -17.78)]
        [InlineData(1, -17.22)]
        [InlineData(42, 5.56)]
        [InlineData(100, 37.78)]
        [InlineData(-1, -18.33)]
        [InlineData(-100, -73.33)]
        public void TestFToC(double f, double expected)
        {
            _temperature.Fahrenheit = f;

            var c = _temperature.Celsius;

            Assert.Equal(expected, c);
        }

        [Theory]
        [InlineData(0, 255.37)]
        [InlineData(1, 255.93)]
        [InlineData(42, 278.71)]
        [InlineData(100, 310.93)]
        [InlineData(-1, 254.82)]
        [InlineData(-100, 199.82)]
        public void TestFToK(double f, double expected)
        {
            _temperature.Fahrenheit = f;

            var k = _temperature.Kelvin;

            Assert.Equal(expected, k);
        }

        [Theory]
        [InlineData(0, -273.15)]
        [InlineData(1, -272.15)]
        [InlineData(42, -231.15)]
        [InlineData(100, -173.15)]
        [InlineData(-1, -274.15)]
        [InlineData(-100, -373.15)]
        public void TestKToC(double k, double expected)
        {
            _temperature.Kelvin = k;

            var c = _temperature.Celsius;

            Assert.Equal(expected, c);
        }

        [Theory]
        [InlineData(0, -459.67)]
        [InlineData(1, -457.87)]
        [InlineData(42, -384.07)]
        [InlineData(100, -279.67)]
        [InlineData(-1, -461.47)]
        [InlineData(-100, -639.67)]
        public void TestKToF(double k, double expected)
        {
            _temperature.Kelvin = k;

            var f = _temperature.Fahrenheit;

            Assert.Equal(expected, f);
        }

        [Fact]
        public void TestRounding()
        {
            _temperature.Fahrenheit = 11;

            var f = _temperature.Fahrenheit;
            
            Assert.Equal(11, f);
        }

        [Fact]
        public void TestStaticMethods()
        {
            var cToF = Temperature.FromCelsius(0).Fahrenheit;
            var cToK = Temperature.FromCelsius(0).Kelvin;
            var fToC = Temperature.FromFahrenheit(0).Celsius;
            var fToK = Temperature.FromFahrenheit(0).Kelvin;
            var kToC = Temperature.FromKelvin(0).Celsius;
            var kToF = Temperature.FromKelvin(0).Fahrenheit;

            Assert.Equal(32, cToF);
            Assert.Equal(273.15, cToK);
            Assert.Equal(-17.78, fToC);
            Assert.Equal(255.37, fToK);
            Assert.Equal(-273.15, kToC);
            Assert.Equal(-459.67, kToF);
        }
    }
}