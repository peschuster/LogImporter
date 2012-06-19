using LogImporter.GeoIp;

namespace LogImporter
{
    public interface IIpLookupService
    {
        Country GetCountry(string ipAddress);
    }
}