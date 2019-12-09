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
  public class BulkApiAgent : IBulkApiAgent
  {
    // Where {0} is returnType, 'locations' or 'geographies'
    // See https://geocoding.geo.census.gov/geocoder/Geocoding_Services_API.pdf
    private const string ENDPOINT_ROOT       = "https://geocoding.geo.census.gov/geocoder/{0}/addressbatch";
    private const string BENCHMARK           = "Public_AR_Current";
    private const string VINTAGE             = "Current_Current";
    private const string DEFAULT_RETURN_TYPE = "geographies";
    private const int    CHUNK_SIZE          = 10000;


    /// <summary>
    /// Geocode a list of addresses using Census geocoding API
    /// </summary>
    /// <param name="addresses">List of addresses where length is less than or equal to 1000</param>
    /// <param name="returnType">Whether to hit Locations or Geographies API (only location is supported at present)</param>
    /// <returns></returns>
    public List<Address> BulkGeocode(List<Address> addresses, string returnType = DEFAULT_RETURN_TYPE)
    {
      if (addresses.Count > CHUNK_SIZE)
      {
        throw new Exception($"BulkApiAgent cannot geocode more than {CHUNK_SIZE} addresses per request");
      }

      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

      using (var client = new HttpClient())
      {
        if (Uri.TryCreate(string.Format(ENDPOINT_ROOT, returnType), UriKind.Absolute, out Uri endpointUrl))
        {
          client.BaseAddress = endpointUrl;

          var addressesAsBytes = AddressesToBytes(addresses);
          var fileContent      = BuildFakeAddressFile(addressesAsBytes);
          var benchmarkContent = BuildBenchmarkContent();
          var vintageContent   = BuildVintageContent();

          var content = new MultipartFormDataContent
          {
            fileContent,
            benchmarkContent,
            vintageContent
          };

          var result          = client.PostAsync("", content).Result;
          var resultAddresses = ReadAddressesFromResponse(result.Content);

          if (result.StatusCode != HttpStatusCode.OK)
          {
            throw new Exception($"Geolocation API responded with code other than OK: {result.StatusCode}");
          }

          return resultAddresses;
        }

        throw new Exception("Error encountered during bulk geocode");
      }
    }

    /// <summary>
    /// Convert a list of addresses to a ByteArray
    /// </summary>
    /// <param name="addresses"></param>
    /// <returns></returns>
    private byte[] AddressesToBytes(List<Address> addresses)
    {
      var addressesCsv = "";
      addresses.ForEach(address => addressesCsv = string.Concat(addressesCsv, address.ToCsv()));

      // Convert addresses from list of strings to Byte array to bundle as "file" as required by API
      return Encoding.ASCII.GetBytes(addressesCsv);
    }

    /// <summary>
    /// Build a "file" from a ByteArray and return it as an octet-stream header expected by census API
    /// </summary>
    /// <param name="addressesAsBytes"></param>
    /// <returns></returns>
    private ByteArrayContent BuildFakeAddressFile(byte[] addressesAsBytes)
    {
      var fileContent = new ByteArrayContent(addressesAsBytes);
      fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
      {
        Name     = "addressFile",
        FileName = "addresses.csv"
      };
      fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

      return fileContent;
    }

    /// <summary>
    /// Build the Benchmark content header expected by census
    /// TODO: I don't really know what's going on with this
    /// </summary>
    /// <param name="benchmark"></param>
    /// <returns></returns>
    private StringContent BuildBenchmarkContent(string benchmark = BENCHMARK)
    {
      // Not a FormUrlEncodedContent class due to an ostensible bug in census API that
      // rejects key/value formatting and requires 'benchmark' in a 'name' field
      var benchmarkContent = new StringContent(benchmark);
      benchmarkContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
      {
        Name = "benchmark"
      };

      return benchmarkContent;
    }

    /// <summary>
    /// Build the Vintage content header expected by census for geographies 
    /// TODO: I don't really know what's going on with this either. See PDF docs.
    /// </summary>
    /// <param name="benchmark"></param>
    /// <returns></returns>
    private StringContent BuildVintageContent(string vintage = VINTAGE)
    {
      // Not a FormUrlEncodedContent class due to an ostensible bug in census API that
      // rejects key/value formatting and requires 'benchmark' in a 'name' field
      var benchmarkContent = new StringContent(vintage);
      benchmarkContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
      {
        Name = "vintage"
      };

      return benchmarkContent;
    }

    /// <summary>
    /// Parse raw response CSV into a list of AddressApiResponse objects
    /// </summary>
    /// <param name="responseContent"></param>
    /// <returns></returns>
    private List<Address> ReadAddressesFromResponse(HttpContent responseContent)
    {
      var resultContent = responseContent.ReadAsStringAsync().Result;
      var resultSplit   = resultContent.Split('\n');

      // Results return with an extra newline after the last entry, drop the last item
      resultSplit = resultSplit.Take(resultSplit.Count() - 1).ToArray();

      var resultAddresses = new List<Address>();
      resultSplit.ToList().ForEach(addressString =>
        resultAddresses.Add(ApiResponseParser.ParseAddressFromApiResponse(addressString))
      );

      return resultAddresses;
    }
  }
}