using LogImporter.Converters;
using Xunit;
using Xunit.Extensions;

namespace LogImporter.Tests.Converters
{
    public class OptionalStringConverterTest
    {
        [Theory]
        [InlineData("")]
        [InlineData((string)null)]
        [InlineData("          ")]
        [InlineData("-")]
        public void TransformsToNullTest(string value)
        {
            var target = new OptionalStringConverter();

            string result = target.Transform(value);

            Assert.Null(result);
        }

        [Fact]
        public void TransformStringValueTest()
        {
            var target = new OptionalStringConverter();

            string value = "any value   ";
            string result = target.Transform(value);

            Assert.Equal(value, result);
        }
    }
}
