using System.Collections.Generic;

namespace LogImporter.Configurations
{
    public interface ILogConfiguration
    {
        IDictionary<int, string> DefaultFieldConfiguration { get; }

        bool IsComment(string line);

        bool IsFieldConfiguration(string line);

        IDictionary<int, string> ReadFieldConfiguration(string fieldsLine);

        bool SetValue(LogEntry entry, string field, string value);
    }
}