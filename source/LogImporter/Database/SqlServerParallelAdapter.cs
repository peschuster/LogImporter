using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace LogImporter.Database
{
    public class SqlServerParallelAdapter : SqlServerAdapter
    {
        public SqlServerParallelAdapter(string connectionString, string tableName)
            : base(connectionString, tableName)
        {
        }

        public override int MaxConcurrentConnections
        {
            get
            {
                var builder = new SqlConnectionStringBuilder(connectionString);

                return builder.MaxPoolSize;
            }
        }

        public override void Write(IEnumerable<LogEntry> entries)
        {
            // Generate insert statement.
            string cmd = SqlMapperExtensions.GetInsertStatement(typeof(LogEntry), tableName: this.tableName);

            // Only allow as many threads as the size of the connection pool.
            var options = new ParallelOptions 
            { 
                MaxDegreeOfParallelism = this.MaxConcurrentConnections 
            };

            using (var connections = new ConcurrentConnectionFactory(this.connectionString, true))
            {
                Parallel.ForEach(entries, options, (entry) => WriteEntry(connections.GetConnection(), cmd, entry));
            }
        }
    }
}
