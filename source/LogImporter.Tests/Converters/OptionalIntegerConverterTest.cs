using LogImporter.Converters;
using Xunit;
using Xunit.Extensions;

namespace LogImporter.Tests.Converters
{
    public class OptionalIntegerConverterTest
    {
        [Theory]
        [InlineData("25", 25)]
        [InlineData("0", 0)]
        public void TransformsValidValuesTest(string value, int? expected)
        {
            var target = new OptionalIntegerConverter(new OptionalStringConverter());

            int? result = target.Transform(value);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("25,25")]
        [InlineData("25.25")]
        [InlineData("0-0")]
        [InlineData("hallo")]
        [InlineData("0x2563")]
        [InlineData("-")]
        [InlineData("    ")]
        [InlineData("")]
        [InlineData(null)]
        public void TransformsInvalidValuesToNullTest(string value)
        {
            var target = new OptionalIntegerConverter(new OptionalStringConverter());

            int? result = target.Transform(value);

            Assert.Null(result);
        }
    }
}
