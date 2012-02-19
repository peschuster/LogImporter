using System.Collections.Generic;

namespace LogImporter.Writers
{
    public interface IEntryWriter
    {
        void Write(IEnumerable<LogEntry> entries, string tableName);

        LogEntry GetLastEntry(string tableName);
    }
}