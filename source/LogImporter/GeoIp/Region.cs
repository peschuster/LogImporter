using System;

namespace LogImporter.GeoIp
{
    public class Region
    {
        public Region()
        {
        }

        public Region(String countryCode, String countryName, String region)
        {
            this.CountryCode = countryCode;
            this.CountryName = countryName;
            this.PRegion = region;
        }

        public String CountryCode { get; set; }

        public String CountryName { get; set; }

        public String PRegion { get; set; }
    }
}