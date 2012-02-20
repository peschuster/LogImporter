using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using Dapper;

namespace LogImporter.Database
{
    public class SqlServerAdapter : IDbAdapter
    {
        protected readonly string connectionString;

        protected readonly string tableName;

        public SqlServerAdapter(string connectionString, string tableName)
        {
            this.connectionString = connectionString;
            this.tableName = tableName;
        }
        
        public virtual int MaxConcurrentConnections 
        {
            get { return 1; }
        }

        public virtual void Write(IEnumerable<LogEntry> entries, out long count)
        {
            count = 0;

            using (IDbConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                string cmd = SqlMapperExtensions.GetInsertStatement(typeof(LogEntry), tableName: this.tableName);

                foreach (LogEntry entry in entries)
                {
                    WriteEntry(connection, cmd, entry);
                    
                    count++;
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
                    ConsoleWriter.WriteError("Error (data exceeded range):");
                    ConsoleWriter.WriteError(entry.LogFilename + " " + entry.LogRow + " - " + entry.csUriStem + entry.csUriQuery + " - " + entry.csUserAgent);

                    return;
                }
                
                throw;
            }
        }

        public virtual IEnumerable<string> GetFileNames()
        {
            using (IDbConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                return connection
                    .Query<string>(string.Format(CultureInfo.InvariantCulture, "select distinct LogFilename from {0}", this.tableName));
            }
        }

        public virtual LogEntry GetLastEntry()
        {
            using (IDbConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                return connection
                    .Query<LogEntry>(string.Format(CultureInfo.InvariantCulture, "select * from {0} order by TimeStamp desc", this.tableName))
                    .FirstOrDefault();
            }
        }
    }
}
