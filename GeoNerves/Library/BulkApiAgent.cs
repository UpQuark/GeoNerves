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
    private const string ENDPOINT_ROOT       = "https://geocoding.geo.census.gov/geocoder/{0}/addressbatch";
    private const string BENCHMARK           = "Public_AR_Current";
    private const string DEFAULT_RETURN_TYPE = "locations";

    /// <summary>
    /// Geocode a list of addresses using Census geocoding API
    /// </summary>
    /// <param name="addresses">List of addresses where length is less than or equal to 1000</param>
    /// <param name="returnType">Whether to hit Locations or Geographies API (only location is supported at present)</param>
    /// <returns></returns>
    public List<AddressApiResponse> BulkGeocode(List<Address> addresses, string returnType = DEFAULT_RETURN_TYPE)
    {
      if (addresses.Count > 1000)
      {
        throw new Exception("BulkApiAgent cannot geocode more than 1000 addresses per request");
      }

      //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

      using (var client = new HttpClient())
      {
        if (Uri.TryCreate(string.Format(ENDPOINT_ROOT, returnType), UriKind.Absolute, out Uri endpointUrl))
        {
          client.BaseAddress = endpointUrl;

          var addressesAsBytes = AddressesToBytes(addresses);
          var fileContent      = BuildFakeAddressFile(addressesAsBytes);
          var benchmarkContent = BuildBenchmarkContent(BENCHMARK);

          var content = new MultipartFormDataContent
          {
            fileContent,
            benchmarkContent
          };

          var result = client.PostAsync("", content).Result;
          var resultAddresses = ReadAddressesFromResponse(result.Content);

          return resultAddresses;
        }

        throw new Exception("Error forming Census endpoint URL");
      }
    }

    /// <summary>
    /// 
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
    /// 
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
    /// 
    /// </summary>
    /// <param name="benchmark"></param>
    /// <returns></returns>
    private StringContent BuildBenchmarkContent(string benchmark)
    {
      // Not a FormUrlEncodedContent class due to an ostensible bug in census API that
      // rejects key/value formatting and requires 'benchmark' in a 'name' field
      var benchmarkContent = new StringContent(BENCHMARK);
      benchmarkContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
      {
        Name = "benchmark"
      };

      return benchmarkContent;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="responseContent"></param>
    /// <returns></returns>
    private List<AddressApiResponse> ReadAddressesFromResponse(HttpContent responseContent)
    {
      var resultContent = responseContent.ReadAsStringAsync().Result;
      var resultSplit   = resultContent.Split('\n');

      // Results return with an extra newline after the last entry, drop the last item
      resultSplit = resultSplit.Take(resultSplit.Count() - 1).ToArray();

      var resultAddresses = new List<AddressApiResponse>();
      resultSplit.ToList().ForEach(addressString =>
        resultAddresses.Add(AddressApiResponse.ParseAddressApiResponseFromCsv(addressString))
      );

      return resultAddresses;
    }
  }
}