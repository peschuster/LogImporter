using LogImporter.Configurations;
using LogImporter.GeoIp;
using LogImporter.Transformations;
using LogImporter.Exceptions;
using System;

namespace LogImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new CommandLine();

            try
            {
                // Read arguments from command line.
                options.Parse(args);

                // Validate all options.
                options.Validate();

                ILogConfiguration configuration = new W3CExtended();

                var reader = new LogReader(configuration);

                using (GeoIpLookupService service = new GeoIpLookupService())
                {
                    var parser = new LogParser(reader, options.Directory, options.Pattern);

                    var entries = parser.ParseEntries(
                        new CleanUrlTransformation(),
                        new GeoLookupTransformation(service));
                }

                Environment.Exit(0);
            }
            catch (UsageException exception)
            {
                Console.Error.WriteLine(exception.Message);

                options.PrintUsage(Console.Error);

                Environment.Exit(1);
            }
        }
    }
}
