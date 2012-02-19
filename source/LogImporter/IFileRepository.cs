using System;
using System.Collections.Generic;
using System.IO;

namespace LogImporter
{
    public interface IFileRepository
    {
        IEnumerable<FileInfo> GetFiles();

        IEnumerable<FileInfo> GetFiles(IEnumerable<string> importedFileNames, LogEntry lastEntry);
    }
}