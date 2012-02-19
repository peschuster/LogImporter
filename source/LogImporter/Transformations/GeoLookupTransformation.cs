using System;
using LogImporter.GeoIp;

namespace LogImporter.Transformations
{
    public class GeoLookupTransformation : ITransformation
    {
        private readonly IIpLookupService lookupService;

        public GeoLookupTransformation(IIpLookupService lookupService)
        {
            if (lookupService == null)
                throw new ArgumentNullException("lookupService");

            this.lookupService = lookupService;
        }

        public void Apply(LogEntry entry)
        {
            if (entry == null)
                throw new ArgumentNullException("entry");
            
            if (!string.IsNullOrWhiteSpace(entry.cIp))
            {
                Country country = this.lookupService.GetCountry(entry.cIp.Trim());

                if (country != null)
                {
                    entry.CountryName = country.Name;
                    entry.CountryCode = country.Code;
                }
            }
        }
    }
}
