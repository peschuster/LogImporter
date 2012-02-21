using System.Collections.Generic;

namespace LogImporter.Database
{
    /// <summary>
    /// Database adapter.
    /// </summary>
    public interface IDbAdapter
    {
        /// <summary>
        /// Returns the maximum number of possible concurrent connections.
        /// </summary>
        int MaxConcurrentConnections { get; }

        /// <summary>
        /// Writes all log entries into the database.
        /// </summary>
        /// <param name="entries">The list of log entries.</param>
        void Write(IEnumerable<LogEntry> entries, out long count);

        /// <summary>
        /// Returns the latest entry from the database.
        /// </summary>
        /// <returns></returns>
        LogEntry GetLastEntry();

        /// <summary>
        /// Returns all distinct log file names in the database table.
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetFileNames();

        /// <summary>
        /// Ensures that the log table exists and creates it, if not alread present.
        /// </summary>
        /// <returns></returns>
        bool EnsureTable();
    }
}