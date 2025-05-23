using MaxMind.GeoIP2;

namespace AdTechAPI.Services
{
    public class GeoIPService(string dbPath)
    {
        private readonly DatabaseReader _reader = new DatabaseReader(dbPath);

        public string? GetCountryIso(string ip)
        {
            if (ip == "127.0.0.1") return "AE";

            try
            {
                var response = _reader.Country(ip);
                return response?.Country?.IsoCode;
            }
            catch
            {
                return null;
            }
        }

    }
}
