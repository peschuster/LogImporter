using System.Collections.Generic;

namespace LogImporter.Database
{
    public interface IDbAdapter
    {
        void Write(IEnumerable<LogEntry> entries, string tableName);

        LogEntry GetLastEntry(string tableName);

        IEnumerable<string> GetFileNames(string tableName);
    }
}