using System;
using System.Collections.Generic;
using LogImporter.Converters;
using Xunit;
using Xunit.Extensions;

namespace LogImporter.Tests.Converters
{
    public class OptionalDateTimeConverterTest
    {
        public static IEnumerable<object[]> ValidTestData
        {
            get
            {
                yield return new object[] { "2012-01-02 01:00", new DateTime(2012, 1, 2, 1, 0, 0) };
                yield return new object[] { "2012-01-02", new DateTime(2012, 1, 2) };
            }
        }

        [Theory]
        [PropertyData("ValidTestData")]
        public void TransformsValidValuesTest(string value, DateTime? expected)
        {
            var target = new OptionalDateTimeConverter(new OptionalStringConverter());

            DateTime? result = target.Transform(value);

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
