using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace LogImporter.GeoIp
{
    public class GeoIpLookupService : IDisposable, IIpLookupService
    {
        private readonly string geoIpDatePath;

        private readonly LookupService service;

        private bool disposed;

        private readonly IDictionary<string, Country> cache;

        public GeoIpLookupService()
        {
            this.geoIpDatePath = Path.Combine(
                Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
                "GeoIP.dat");

            this.service = new LookupService(this.geoIpDatePath, LookupService.GeoipMemoryCache);

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
