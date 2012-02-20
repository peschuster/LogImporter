using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using Dapper;

namespace LogImporter.Database
{
    public class SqlServerDb : IDbAdapter
    {
        protected readonly string connectionString;

        public SqlServerDb(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public virtual void Write(IEnumerable<LogEntry> entries, string tableName)
        {
            using (IDbConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                string cmd = SqlMapperExtensions.GetInsertStatement(typeof(LogEntry), tableName: tableName);

                foreach (LogEntry entry in entries)
                {
                    WriteEntry(connection, cmd, entry);
                }
            }
        }

        protected static void WriteEntry(IDbConnection connection, string cmd, LogEntry entry)
        {
            try
            {
                connection.Execute(cmd, entry);
            }
            catch (SqlException exception)
            {
                Console.Error.WriteLine(exception.ErrorCode);

                // -2146232060 -> truncated data
                if (exception.ErrorCode == -2146232060)
                {
                    Console.Error.WriteLine("Error (data exceeded range):");
                    Console.Error.WriteLine(entry.LogFilename + " " + entry.LogRow + " - " + entry.csUriStem + entry.csUriQuery + " - " + entry.csUserAgent);

                    return;
                }
                
                throw;
            }
        }

        public virtual IEnumerable<string> GetFileNames(string tableName)
        {
            using (IDbConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                return connection
                    .Query<string>(string.Format(CultureInfo.InvariantCulture, "select distinct LogFilename from {0}", tableName));
            }
        }

        public virtual LogEntry GetLastEntry(string tableName)
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
