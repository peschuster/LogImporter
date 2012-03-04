using LogImporter.Database;
using Xunit;
using Xunit.Extensions;

namespace LogImporter.Tests.Database
{
    public class SqlServerAdapterTest
    {
        [Theory]
        [InlineData("Data Source=myServerAddress;Initial Catalog=myDataBase;User Id=myUsername;Password=myPassword", 100)]
        [InlineData("Data Source=myServerAddress;Initial Catalog=myDataBase;User Id=myUsername;Password=myPassword;Max Pool Size=5", 5)]
        public void MaxConcurrentConnectionsTest(string connectionString, int expected)
        {
            var target = new SqlServerAdapter(connectionString, "MyTable");

            int result = target.MaxConcurrentConnections;

            Assert.Equal(expected, result);
        }
    }
}
