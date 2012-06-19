using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LogImporter
{
    /// <summary>
    /// Repository for file system access.
    /// </summary>
    public class FileRepository : IFileRepository
    {
        private readonly IEnumerable<FileInfo> files;

        public FileRepository(DirectoryInfo directory, string pattern = null)
        {
            if (directory == null)
                throw new ArgumentNullException("directory");

            if (!directory.Exists)
                throw new ArgumentException("Directory does not exist.", "directory");

            if (string.IsNullOrWhiteSpace(pattern))
            {
                // Read all files
                this.files = directory.EnumerateFiles();
            }
            else
            {
                // Read all files for pattern
                this.files = directory.EnumerateFiles(pattern);
            }
        }

        public IEnumerable<FileInfo> GetFiles()
        {
            return this.files
                .OrderBy(f => f.Name);
        }

        public IEnumerable<FileInfo> GetFiles(IEnumerable<string> importedFileNames, LogEntry lastEntry)
        {
            if (importedFileNames == null)
                throw new ArgumentNullException("importedFileNames");

            if (lastEntry == null)
                throw new ArgumentNullException("lastEntry");

            return (from f in this.files
                    where f.FullName == lastEntry.LogFilename || !importedFileNames.Contains(f.FullName)
                    orderby f.Name
                    select f);
        }
    }
}
