using System;
using System.Collections.Generic;
using System.IO;
using LogImporter.Transformations;

namespace LogImporter
{
    internal class LogParser
    {
        private readonly FileInfo[] files;

        private readonly LogReader reader;

        public LogParser(LogReader reader, string path, string pattern)
        {
            this.reader = reader;
            var directory = new DirectoryInfo(path);

            if (!directory.Exists)
                throw new ArgumentException("Directory does not exist.", "directory");

            this.files = directory.GetFiles(pattern);
        }

        public IEnumerable<LogEntry> ParseEntries(params ITransformation[] transformations)
        {
            foreach (var file in this.files)
            {
                var entries = this.reader.ReadFile(file);

                foreach (var entry in entries)
                {
                    if (transformations != null)
                    {
                        this.TransformEntry(entry, transformations);
                    }

                    yield return entry;
                }
            }
        }

        private void TransformEntry(LogEntry entry, ITransformation[] transformations)
        {
            foreach (ITransformation transformation in transformations)
            {
                if (transformation == null)
                    continue;

                transformation.Apply(entry);
            }
        }
    }
}
