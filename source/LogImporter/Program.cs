using LogImporter.Configurations;
using LogImporter.Transformations;

namespace LogImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            ILogConfiguration configuration = new W3CExtended();

            var reader = new LogReader(configuration);

            using (IpLookupService service = new IpLookupService())
            {
                LogParser parser = new LogParser(reader, @"J:\logs\W3SVC8_services", "*.log");

                var entries = parser.ParseEntries(
                    new CleanUrlTransformation(),
                    new GeoLookupTransformation(service));
            }
        }
    }
}
