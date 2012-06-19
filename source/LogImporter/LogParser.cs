using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LogImporter.Database;
using LogImporter.Transformations;

namespace LogImporter
{
    internal class LogParser
    {
        private readonly IFileRepository fileService;

        private readonly LogReader reader;

        private readonly IDbAdapter db;

        private readonly ITransformation[] transformations;

        public LogParser(LogReader reader, IFileRepository fileService, IDbAdapter db, params ITransformation[] transformations)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            if (fileService == null)
                throw new ArgumentNullException("files");

            if (db == null)
                throw new ArgumentNullException("db");

            this.reader = reader;
            this.fileService = fileService;
            this.db = db;
            this.transformations = transformations;
        }

        public void ParseEntries(out long count)
        {
            IEnumerable<FileInfo> files = this.fileService.GetFiles();

            this.ParseEntries(files, null, out count);
        }

        public void ParseEntries(IEnumerable<string> importedFileNames, LogEntry lastEntry, out long count)
        {
            if (importedFileNames == null)
                throw new ArgumentNullException("importedFileNames");

            if (lastEntry == null)
                throw new ArgumentNullException("lastEntry");

            IEnumerable<FileInfo> files = this.fileService.GetFiles(importedFileNames.ToArray(), lastEntry);

            this.ParseEntries(files, lastEntry, out count);
        }

        /// <summary>
        /// Don't insert already existing records (determined by <c>LogRow</c> in latest inserted file).
        /// </summary>
        /// <param name="lastEntry"></param>
        /// <returns></returns>
        private static Func<LogEntry, bool> AllowInsert(LogEntry lastEntry)
        {
            if (lastEntry == null)
                return (entry) => true;

            return (entry) => 
                entry.LogFilename != lastEntry.LogFilename || entry.LogRow > lastEntry.LogRow;
        }

        private void ParseEntries(IEnumerable<FileInfo> files, LogEntry lastEntry, out long count)
        {
            count = 0;

            // Only allow as many parallel threads as the size of the connection pool.
            var options = new ParallelOptions { MaxDegreeOfParallelism = this.db.MaxConcurrentConnections };

            var subCounts = new Dictionary<string, long>();

            Parallel.ForEach(files, options, (file) =>
            {
                long subCount;

                // Read entries from file and apply transformations.
                var entries = this.ParseEntries(file, this.transformations).Where(AllowInsert(lastEntry));

                // Write entries to the database.
                this.db.Write(entries, out subCount);

                subCounts.Add(file.FullName, subCount);
            });

            // Set count to sum of all entry counts per file.
            count = subCounts.Values.Sum();
        }

        private IEnumerable<LogEntry> ParseEntries(FileInfo file, ITransformation[] transformations)
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
