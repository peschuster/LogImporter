using System;
using LogImporter.GeoIp;

namespace LogImporter.Transformations
{
    public class GeoLookupTransformation : ITransformation
    {
        private readonly IpLookupService lookupService;

        public GeoLookupTransformation(IpLookupService lookupService)
        {
            if (lookupService == null)
                throw new ArgumentNullException("lookupService");

            this.lookupService = lookupService;
        }

        public void Apply(LogEntry entry)
        {
            Country country = null;

            if (!string.IsNullOrWhiteSpace(entry.cIp))
                country = this.lookupService.GetCountry(entry.cIp.Trim());

            entry.CountryName = country != null ? country.Name : null;
            entry.CountryCode = country != null ? country.Code : null;
        }
    }
}
