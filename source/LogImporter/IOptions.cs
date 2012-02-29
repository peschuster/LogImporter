using System.IO;

namespace LogImporter
{
    public interface IOptions
    {
        /// <summary>
        /// Directory with log files.
        /// </summary>
        DirectoryInfo Directory { get; set; }

        /// <summary>
        /// Pattern for log file names.
        /// </summary>
        string Pattern { get; set; }

        /// <summary>
        /// Connection string for target database.
        /// </summary>
        string ConnectionString { get; set; }

        /// <summary>
        /// Target table name.
        /// </summary>
        string TableName { get; set; }

        /// <summary>
        /// Force full import of all files.
        /// </summary>
        bool Force { get; set; }

        /// <summary>
        /// Create the table if it does not alread exists.
        /// </summary>
        bool CreateTable { get; set; }
    }
}