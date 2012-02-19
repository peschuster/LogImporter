using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using LogImporter.GeoIp;

namespace LogImporter
{
    public class IpLookupService : IDisposable
    {
        private readonly string geoIpDatePath;

        private readonly LookupService service;

        private bool disposed;

        private readonly IDictionary<string, Country> cache;

        public IpLookupService()
        {
            this.geoIpDatePath = Path.Combine(
                Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
                "GeoIP.dat");

            this.service = new LookupService(this.geoIpDatePath, LookupService.GeoipStandard);

            this.cache = new Dictionary<string, Country>();
        }

        public Country GetCountry(string ipAddress)
        {
            if (!this.cache.ContainsKey(ipAddress))
            {
                this.cache[ipAddress] = this.service.GetCountry(ipAddress);
            }

            return this.cache[ipAddress];
        }

        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool dispose)
        {
            if (dispose && !this.disposed)
            {
                if (this.service != null)
                    this.service.Dispose();

                this.cache.Clear();

                this.disposed = true;
            }
        }
    }
}
