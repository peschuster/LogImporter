using System;
using System.Collections.Generic;
using System.IO;
using LogImporter.Configurations;

namespace LogImporter
{
    public class LogReader
    {
        private readonly ILogConfiguration adapter;

        public LogReader(ILogConfiguration adapter)
        {
            if (adapter == null)
                throw new ArgumentNullException("adapter");

            this.adapter = adapter;
        }

        public IEnumerable<LogEntry> ReadFile(FileInfo file)
        {
            int row = 0;
            IDictionary<int, string> fields = this.adapter.DefaultFieldConfiguration ?? new Dictionary<int, string>();

            // Open file stream
            FileStream stream = file.OpenRead();

            try
            {
                using (var reader = new StreamReader(stream))
                {
                    stream = null;

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();

                        if (line == null)
                            continue;

                        if (this.adapter.IsFieldConfiguration(line))
                        {
                            fields = this.adapter.ReadFieldConfiguration(line);
                            continue;
                        }

                        if (this.adapter.IsComment(line))
                            continue;

                        // Create the log entry.
                        var entry = this.CreateEntry(fields, line.Split(' '));

                        if (entry != null)
                        {
                            entry.LogFilename = file.FullName;
                            entry.LogRow = ++row;

                            yield return entry;
                        }
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
                // Lesser values, than fields specified...
                if (values.Length <= index)
                    continue;

                this.adapter.SetValue(result, fields[index], values[index]);
            }

            return result;
        }
    }
}
