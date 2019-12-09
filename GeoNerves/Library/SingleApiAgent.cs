using System;
using System.Net;
using System.Net.Http;
using GeoNerves.Models;

namespace GeoNerves
{
  public class SingleApiAgent
  {
    // Where {0} is returnType: 'locations' or 'geographies'
    // Where {1} is searchType: 'onelineaddress' or 'address' or 'coordinates'
    // See https://geocoding.geo.census.gov/geocoder/Geocoding_Services_API.pdf
    private const string ENDPOINT_ROOT       = "https://geocoding.geo.census.gov/geocoder/{0}/{1}?parameters";
    private const string BENCHMARK           = "Public_AR_Current";
    private const string VINTAGE             = "Current_Current";
    private const string DEFAULT_RETURN_TYPE = "geographies";
    private const string DEFAULT_SEARCH_TYPE = "onelineaddress";

    public Address SingleLineGeocode( string addressString, string returnType = DEFAULT_RETURN_TYPE)
    {
      using (var client = new HttpClient())
      {
        if (Uri.TryCreate(string.Format(ENDPOINT_ROOT, returnType, "onelineaddress"), UriKind.Absolute,
          out Uri endpointUrl))
        {
          client.BaseAddress = endpointUrl;

          var result = client.GetAsync(
            $"?benchmark={BENCHMARK}" +
            $"&vintage={VINTAGE}" +
            $"&format=json" +
            $"&address={addressString}").Result;

          if (result.StatusCode != HttpStatusCode.OK)
          {
            throw new Exception($"Geolocation API responded with code other than OK: {result.StatusCode}");
          }

          var resultString = result.Content.ReadAsStringAsync().Result;
          return SingleApiResponseParser.ParseAddressFromApiResponse(resultString);
        }

        throw new Exception("Error encountered during single geocode");
      }
    }
  }
}