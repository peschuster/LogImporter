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
            var db = new SqlServerDb(options.ConnectionString);

            var files = new FileRepository(options.Directory, options.Pattern);

            using (var service = new GeoIpLookupService())
            {
                var parser = new LogParser(reader, files);

                var transformations = new ITransformation[] 
                {
                    new CleanUrlTransformation(),
                    new GeoLookupTransformation(service)                
                };

                IEnumerable<LogEntry> entries;
                
                if (options.Force)
                {
                    entries = parser.ParseEntries(transformations);
                } 
                else
                {
                    LogEntry lastEntry = db.GetLastEntry(options.TableName);
                    IEnumerable<string> importedFileNames = db.GetFileNames(options.TableName);

                    entries = parser.ParseEntries(importedFileNames, lastEntry, transformations);
                }

                // Write log entries into database.
                db.Write(entries, options.TableName);
            }
        }
    }
}
