using System;
using System.Collections.Generic;
using System.IO;

namespace LogImporter
{
    /// <summary>
    /// Returns available log files.
    /// </summary>
    public interface IFileRepository
    {
        /// <summary>
        /// Returns all available log files.
        /// </summary>
        /// <returns></returns>
        IEnumerable<FileInfo> GetFiles();

        /// <summary>
        /// Returns only files not already imported or equal to the latest entry in the database.
        /// </summary>
        /// <param name="importedFileNames">List of already imported files.</param>
        /// <param name="lastEntry">Latest entry in the database.</param>
        /// <returns></returns>
        IEnumerable<FileInfo> GetFiles(IEnumerable<string> importedFileNames, LogEntry lastEntry);
    }
}