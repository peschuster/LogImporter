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

        /// <summary>
        /// Import data in one single thread.
        /// </summary>
        public bool Sequential { get; set; }

        /// <summary>
        /// Create the table if it does not alread exists.
        /// </summary>
        public bool CreateTable { get; set; }

        public void PrintUsage(TextWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");
            
            writer.WriteLine();
            writer.WriteLine(new string('=', 30));
            writer.WriteLine();

            writer.WriteLine("LogImporter usage:");
            writer.WriteLine();
            
            this.options.WriteOptionDescriptions(writer);
            
            writer.WriteLine();
            writer.WriteLine(new string('=', 30));
            writer.WriteLine();
        }

        public void Parse(string[] arguments)
        {
            try
            {
                this.Directory = new DirectoryInfo(".");
                this.Pattern = "*.*";
                this.Force = false;
                this.Sequential = false;

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
                    "d=",
                    "Directory with log files",
                    (string s) => this.Directory = new DirectoryInfo(StripQuotes(s)))

                .Add(
                    "p=",
                    "Pattern for log files",
                    (string s) => this.Pattern = StripQuotes(s))

                .Add(
                    "c=",
                    "Connection string for target database",
                    (string s) => this.ConnectionString = StripQuotes(s))

                .Add(
                    "t=",
                    "Target table name",
                    (string s) => this.TableName = StripQuotes(s))

                .Add(
                    "n",
                    "Create the table if it does not alread exists",
                    (string b) => this.CreateTable = b != null)

                .Add(
                    "f|force",
                    "Force full import of all files",
                    (string b) => this.Force = b != null)

                .Add(
                    "s|sequential",
                    "Import data in one single thread.",
                    (string b) => this.Sequential = b != null);

            return options;
        }

        private static string StripQuotes(string s)
        {
            if (s != null)
            {
                string t = s.Trim();

                if ((t.StartsWith("\"", StringComparison.OrdinalIgnoreCase) && t.EndsWith("\"", StringComparison.OrdinalIgnoreCase))
                    || (t.StartsWith("'", StringComparison.OrdinalIgnoreCase) && t.EndsWith("'", StringComparison.OrdinalIgnoreCase)))
                {
                    return t.Substring(1, t.Length - 2);
                }
            }

            return s;
        }
    }
}
