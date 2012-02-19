using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Globalization;
using Dapper;

namespace LogImporter.Database
{
    public class SqlServerDb : IDbAdapter
    {
        private readonly string connectionString;

        public SqlServerDb(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Write(IEnumerable<LogEntry> entries, string tableName)
        {
            using (IDbConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                foreach (LogEntry entry in entries)
                {
                    connection.Insert(entry, tableName: tableName);
                }
            }
        }

        public IEnumerable<string> GetFileNames(string tableName)
        {
            using (IDbConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                return connection
                    .Query<string>(string.Format(CultureInfo.InvariantCulture, "select distinct LogFilename from {0} order by TimeStamp asc", tableName));
            }
        }

        public LogEntry GetLastEntry(string tableName)
        {
            using (IDbConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                return connection
                    .Query<LogEntry>(string.Format(CultureInfo.InvariantCulture, "select * from {0} order by TimeStamp desc", tableName))
                    .FirstOrDefault();
            }
        }
    }
}
