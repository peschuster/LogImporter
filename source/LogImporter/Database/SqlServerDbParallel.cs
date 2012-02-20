using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace LogImporter.Database
{
    public class SqlServerDbParallel : SqlServerDb
    {
        private int maxDegreeOfParallelism;

        public SqlServerDbParallel(string connectionString)
            : base(connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);

            this.maxDegreeOfParallelism = builder.MaxPoolSize;
        }

        public override void Write(IEnumerable<LogEntry> entries, string tableName)
        {
            var connections = new ConcurrentDictionary<int, IDbConnection>();


            try
            {
                string cmd = SqlMapperExtensions.GetInsertStatement(typeof(LogEntry), tableName: tableName);

                Parallel.ForEach(
                    entries, 
                    new ParallelOptions { MaxDegreeOfParallelism = this.maxDegreeOfParallelism },
                    (entry) => WriteEntry(
                        connections.GetOrAdd(Thread.CurrentThread.ManagedThreadId, (i) => this.CreateAndOpenConnection()), 
                        cmd, 
                        entry));
            }
            finally
            {
                foreach (var connection in connections.Values)
                {
                    if (connection.State != ConnectionState.Closed)
                        connection.Dispose();
                }
            }
        }

        private IDbConnection CreateAndOpenConnection()
        {
            var c = new SqlConnection(this.connectionString);

            c.Open();
            
            return c;
        }
    }
}
