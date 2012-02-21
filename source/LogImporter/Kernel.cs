using System;
using System.Collections.Generic;
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
    
            IDbAdapter db = options.Sequential 
                ? new SqlServerAdapter(options.ConnectionString, options.TableName)
                : new SqlServerParallelAdapter(options.ConnectionString, options.TableName);

            var files = new FileRepository(options.Directory, options.Pattern);

            if (options.CreateTable)
            {
                db.EnsureTable();
            }

            using (var service = new GeoIpLookupService())
            {
                var parser = new LogParser(reader, files);

                var transformations = new ITransformation[] 
                {
                    new CleanUrlTransformation(),
                    new GeoLookupTransformation(service)                
                };

                IEnumerable<LogEntry> entries;
                LogEntry lastEntry = db.GetLastEntry();
                
                if (options.Force || lastEntry == null)
                {
                    entries = parser.ParseEntries(transformations);
                } 
                else
                {
                    IEnumerable<string> importedFileNames = db.GetFileNames();

                    entries = parser.ParseEntries(importedFileNames, lastEntry, transformations);
                }

                long count;

                // Write log entries into database.
                db.Write(entries, out count);

                ConsoleWriter.WriteSuccess("Imported {0} log entries.", count);
            }
        }
    }
}
