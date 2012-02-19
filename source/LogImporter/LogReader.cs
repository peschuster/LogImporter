using System.Collections.Generic;
using System.IO;
using LogImporter.Configurations;

namespace LogImporter
{
    public class LogReader
    {
        private readonly ILogConfiguration mapping;

        public LogReader(ILogConfiguration mapping)
        {
            this.mapping = mapping;
        }

        public IEnumerable<LogEntry> ReadFile(FileInfo file)
        {
            FileStream stream = file.OpenRead();

            int row = 0;
            IDictionary<int, string> fieldIndex = new Dictionary<int, string>();

            try
            {
                using (var reader = new StreamReader(stream))
                {
                    stream = null;

                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (this.mapping.IsFieldConfiguration(line))
                        {
                            fieldIndex = this.mapping.ReadFieldConfiguration(line);

                            continue;
                        }

                        if (this.mapping.IsComment(line))
                            continue;

                        var entry = this.CreateEntry(fieldIndex, line.Split(' '));

                        if (entry == null)
                            continue;

                        entry.LogFilename = file.Name;
                        entry.LogRow = ++row;

                        yield return entry;
                    }
                }
            }
            finally
            {
                if (stream != null)
                    stream.Dispose();
            }
        }

        private LogEntry CreateEntry(IDictionary<int, string> fields, string[] values)
        {
            var result = new LogEntry();

            foreach (int index in fields.Keys)
            {
                string field = fields[index];

                if (values.Length <= index)
                    continue;

                this.mapping.SetValue(result, field, values[index]);
            }

            return result;
        }
    }
}
