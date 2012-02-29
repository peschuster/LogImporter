using System;
using System.Collections.Generic;
using System.Linq;
using LogImporter.Configurations;
using LogImporter.Database;
using LogImporter.GeoIp;
using LogImporter.Transformations;

namespace LogImporter
{
    public class Kernel
    {
        public void Run(IOptions options)
        {
            if (options == null)
                throw new ArgumentNullException("options");

            var configuration = new W3CExtended();
            var reader = new LogReader(configuration);       
    
            IDbAdapter db = new SqlServerAdapter(options.ConnectionString, options.TableName);

            var files = new FileRepository(options.Directory, options.Pattern);

            // Create the table in db (if not existent)
            if (options.CreateTable)
            {
                db.EnsureTable();
            }

            using (var service = new GeoIpLookupService())
            {
                // Initialize log parser
                var parser = new LogParser(
                    reader,
                    files,
                    db,
                    new CleanUrlTransformation(),
                    new GeoLookupTransformation(service));

                long count;
                LogEntry lastEntry = db.GetLastEntry();
                
                if (options.Force || lastEntry == null)
                {
                    ConsoleWriter.WriteInfo("Importing all log files...");

                    parser.ParseEntries(out count);
                } 
                else
                {
                    IEnumerable<string> importedFileNames = db.GetFileNames();

                    ConsoleWriter.WriteInfo("{0} files already in database.", importedFileNames.Count());
                    ConsoleWriter.WriteInfo("Importing only new entries...", importedFileNames.Count());

                    parser.ParseEntries(importedFileNames, lastEntry, out count);
                }

                ConsoleWriter.WriteSuccess("Imported {0} log entries.", count);
            }
        }
    }
}
