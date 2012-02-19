using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Globalization;
using Dapper;

namespace LogImporter.Writers
{
    public class SqlServerWriter : IEntryWriter
    {
        private readonly string connectionString;

        public SqlServerWriter(string connectionString)
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
