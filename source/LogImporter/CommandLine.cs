using System;
using System.Collections.Generic;
using System.IO;
using LogImporter.Exceptions;
using NDesk.Options;

namespace LogImporter
{
    internal class CommandLine : IOptions
    {
        private readonly OptionSet options;

        public CommandLine()
        {
            this.options = this.Initialize();
        }

        /// <summary>
        /// Directory with log files.
        /// </summary>
        public DirectoryInfo Directory { get; set; }

        /// <summary>
        /// Pattern for log file names.
        /// </summary>
        public string Pattern { get; set; }

        /// <summary>
        /// Connection string for target database.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Target table name.
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Force full import of all files.
        /// </summary>
        public bool Force { get; set; }

        public void PrintUsage(TextWriter writer)
        {
            this.options.WriteOptionDescriptions(writer);
        }

        public void Parse(string[] arguments)
        {
            try
            {
                this.Directory = new DirectoryInfo(".");
                this.Pattern = "*.*";
                this.Force = false;

                List<string> unknown = options.Parse(arguments);

                if (unknown != null && unknown.Count != 0)
                {
                    foreach (var s in unknown)
                    {
                        // empty "unkown" parameters are allowed
                        if (s != null && !string.IsNullOrEmpty(s.Trim()))
                            throw new UsageException("Unkown parameter(s): " + string.Join(", ", unknown.ToArray()));
                    }
                }
            }
            catch (OptionException e)
            {
                throw new UsageException(e.Message, e);
            }
        }

        public void Validate()
        {
            if (this.Directory == null || !this.Directory.Exists)
                throw new UsageException("Please specify a directory with log files.");

            if (string.IsNullOrWhiteSpace(this.TableName))
                throw new UsageException("Please specify a table name.");

            if (string.IsNullOrWhiteSpace(this.ConnectionString))
                throw new UsageException("Please specify a connection string.");

            if (string.IsNullOrWhiteSpace(this.Pattern))
            {
                this.Pattern = "*.*";
            }
        }

        private OptionSet Initialize()
        {
            var options = new OptionSet();

            options
                .Add(
                    "d|directory=",
                    "Directory with log files",
                    (string s) => this.Directory = new DirectoryInfo(s))

                .Add(
                    "p|pattern=",
                    "Pattern for log files",
                    (string s) => this.Pattern = s)

                .Add(
                    "c|connectionstring=",
                    "Connection string for target database",
                    (string s) => this.ConnectionString = s)

                .Add(
                    "t|tablename=",
                    "Target table name",
                    (string s) => this.TableName = s)

                .Add(
                    "f|force",
                    "Force full import of all files",
                    (bool b) => this.Force = b);

            return options;
        }
    }
}
