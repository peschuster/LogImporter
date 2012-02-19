using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LogImporter
{
    public class FileRepository : IFileRepository
    {
        private readonly FileInfo[] files;

        public FileRepository(DirectoryInfo directory, string pattern)
        {
            if (directory == null)
                throw new ArgumentNullException("directory");

            if (!directory.Exists)
                throw new ArgumentException("Directory does not exist.", "directory");
            
            this.files = string.IsNullOrWhiteSpace(pattern)
                ? directory.GetFiles()
                : directory.GetFiles(pattern);
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
                    where f.Name == lastEntry.LogFilename || !importedFileNames.Contains(f.Name)
                    orderby f.Name
                    select f);
        }
    }
}
