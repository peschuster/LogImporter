using System;
using LogImporter.GeoIp;
using LogImporter.Transformations;
using Moq;
using Xunit;

namespace LogImporter.Tests.Transformations
{
    public class GeoLookupTransformationTest
    {
        private const string TestCountryName = "Germany";

        private const string TestCountryCode = "DE";

        [Fact]
        public void ApplyThrowsArgumentNullException()
        {
            var lookup = new Mock<IIpLookupService>();
            var target = new GeoLookupTransformation(lookup.Object);
            
            Assert.Throws<ArgumentNullException>(() => target.Apply(null));

            lookup.Verify(l => l.GetCountry(It.IsAny<string>()), Times.Never());
        }

        [Fact]
        public void ApplyAcceptsUnsetIp()
        {
            var lookup = new Mock<IIpLookupService>();
            var target = new GeoLookupTransformation(lookup.Object);

            var entry = new LogEntry();
            
            target.Apply(entry);

            Assert.Null(entry.CountryName);
            Assert.Null(entry.CountryCode);

            lookup.Verify(l => l.GetCountry(It.IsAny<string>()), Times.Never());
        }

        [Fact]
        public void ApplyResolvesClientIpAddress()
        {
            var lookup = new Mock<IIpLookupService>();
            var target = new GeoLookupTransformation(lookup.Object);

            string cIpAddress = "myClientIp";

            string sIpAddress = "myServerIp";

            var entry = new LogEntry
            {
                cIp = cIpAddress,
                sIp = sIpAddress,
            };

            lookup.Setup(l => l.GetCountry(It.IsAny<string>())).Returns(new Country(TestCountryCode, TestCountryName));

            target.Apply(entry);

            Assert.Equal(TestCountryName, entry.CountryName);
            Assert.Equal(TestCountryCode, entry.CountryCode);

            lookup.Verify(l => l.GetCountry(sIpAddress), Times.Never());
            lookup.Verify(l => l.GetCountry(cIpAddress), Times.AtLeastOnce());
        }
    }
}
