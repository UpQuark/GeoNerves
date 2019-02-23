using System;
using System.Collections.Generic;
using GeoNerves.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Linq;
using System.Net;
using System.Text;

namespace GeoNerves
{
  /// <summary>
  /// API client for the Census Location bulk API
  /// </summary>
  public class BulkApiAgent
  {
    #region Constants

    // Where {0} is returnType, 'locations' or 'geographies'
    const string EndPointRoot = "https://geocoding.geo.census.gov/geocoder/{0}/addressbatch";
    const string Benchmark = "Public_AR_Current";
    const string DefaultReturnType = "locations";

    #endregion

    /// <summary>
    /// Geocode a list of addresses using Census geocoding API
    /// </summary>
    /// <param name="addresses">List of addresses where length is less than or equal to 1000</param>
    /// <param name="returnType">Whether to hit Locations or Geographies API (only location is supported at present)</param>
    /// <returns></returns>
    public List<AddressApiResponse> BulkGeocode(List<Address> addresses, string returnType = DefaultReturnType)
    {
      if (addresses.Count() > 1000)
      {
        throw new Exception("BulkApiAgent cannot geocode more than 1000 addresses per request");
      }

      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

      var addressesCsv = "";
      addresses.ForEach(address => addressesCsv = string.Concat(addressesCsv, address.ToCsv()));

      // Convert addresses from list of strings to Byte array to bundle as "file" as required by API
      byte[] addressesAsBytes = Encoding.ASCII.GetBytes(addressesCsv);

      using (var client = new HttpClient())
      {
        try
        {
          if (Uri.TryCreate(string.Format(EndPointRoot, returnType), UriKind.Absolute, out Uri endpointUrl))
          {
            client.BaseAddress = endpointUrl;

            // Set up form arguments for POST request
            var content = new MultipartFormDataContent();

            // Fake a file to pass to endpoint
            var fileContent = new ByteArrayContent(addressesAsBytes);
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
              Name = "addressFile",
              FileName = "addresses.csv"
            };
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            content.Add(fileContent);

            // Not a FormUrlEncodedContent class due to an ostensible bug in census API that
            // rejects key/value formatting and requires 'benchmark' in a 'name' field
            var benchMarkContent = new StringContent(Benchmark);
            benchMarkContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
              Name = "benchmark"
            };
            content.Add(benchMarkContent);

            var result = client.PostAsync("", content).Result;
            var resultContent = result.Content.ReadAsStringAsync().Result;
            var resultSplit = resultContent.Split('\n');

            // Results return with an extra newline after the last entry, drop the last item
            resultSplit = resultSplit.TakeLast(resultSplit.Count() - 1).ToArray();

            var resultAddresses = new List<AddressApiResponse>();
            resultSplit.ToList().ForEach(addressString =>
              resultAddresses.Add(AddressApiResponse.ParseAddressApiResponseFromCsv(addressString)));

            return resultAddresses;
          }
          else
          {
            throw new Exception("Error forming Census endpoint URL");
          }
        }
        catch (Exception e)
        {
          throw e;
        }
      }
    }
  }
}