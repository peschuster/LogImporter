using LogImporter.Configurations;
using LogImporter.GeoIp;
using LogImporter.Transformations;

namespace LogImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            ILogConfiguration configuration = new W3CExtended();

            var reader = new LogReader(configuration);

            using (GeoIpLookupService service = new GeoIpLookupService())
            {
                LogParser parser = new LogParser(reader, @"J:\mbi_logs\W3SVC8_services", "*.log");

                var entries = parser.ParseEntries(
                    new CleanUrlTransformation(),
                    new GeoLookupTransformation(service));
            }
        }
    }
}
