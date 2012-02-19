using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LogImporter.Transformations;

namespace LogImporter
{
    internal class LogParser
    {
        private readonly IFileRepository fileService;

        private readonly LogReader reader;

        public LogParser(LogReader reader, IFileRepository fileService)
        {
             if (reader == null)
                throw new ArgumentNullException("reader");

             if (fileService == null)
                 throw new ArgumentNullException("files");

            this.reader = reader;
            this.fileService = fileService;
        }

        public IEnumerable<LogEntry> ParseEntries(params ITransformation[] transformations)
        {
            return this.ParseEntries(
                this.fileService.GetFiles(), 
                transformations);
        }

        public IEnumerable<LogEntry> ParseEntries(IEnumerable<string> importedFileNames, LogEntry lastEntry, params ITransformation[] transformations)
        {
            if (importedFileNames == null)
                throw new ArgumentNullException("importedFileNames");

            if (lastEntry == null)
                throw new ArgumentNullException("lastEntry");

            return this.ParseEntries(
                this.fileService.GetFiles(importedFileNames, lastEntry), 
                transformations)
                .Where(e => e.LogFilename != lastEntry.LogFilename || e.LogRow > lastEntry.LogRow);
        }
  
        private IEnumerable<LogEntry> ParseEntries(IEnumerable<FileInfo> files, ITransformation[] transformations)
        {
            foreach (var file in files)
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
