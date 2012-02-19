using System;
using LogImporter.Transformations;
using Xunit;

namespace LogImporter.Tests.Transformations
{
    public class CleanUrlTransformationTest
    {
        [Fact]
        public void ApplyThrowsArgumentNullException()
        {
            var target = new CleanUrlTransformation();

            Assert.Throws<ArgumentNullException>(() => target.Apply(null));
        }

        [Fact]
        public void ApplyAcceptsNullUrl()
        {
            var target = new CleanUrlTransformation();

            var entry = new LogEntry
            {
                csUriStem = null,
            };

            Assert.DoesNotThrow(() => target.Apply(entry));
            Assert.Null(entry.CleanUri);
        }

        [Fact]
        public void ApplyLeavesOriginalUrlUntouched()
        {
            var target = new CleanUrlTransformation();

            string originalUrl = "my/url.php";

            var entry = new LogEntry
            {
                csUriStem = originalUrl,
            };

            target.Apply(entry);

            Assert.Equal(originalUrl, entry.csUriStem);
        }

        [Fact]
        public void ApplyRemovesGuids()
        {
            var target = new CleanUrlTransformation();

            string originalUrl = "my/url/" + Guid.NewGuid() + "/index.php";
            string cleanlUrl = "my/url/index.php";

            var entry = new LogEntry
            {
                csUriStem = originalUrl,
            };

            target.Apply(entry);

            Assert.Equal(cleanlUrl, entry.CleanUri);
        }

        [Fact]
        public void ApplyRemovesGuidsAtEnding()
        {
            var target = new CleanUrlTransformation();

            string originalUrl = "my/url/" + Guid.NewGuid();
            string cleanlUrl = "my/url/";

            var entry = new LogEntry
            {
                csUriStem = originalUrl,
            };

            target.Apply(entry);

            Assert.Equal(cleanlUrl, entry.CleanUri);
        }
    }
}
