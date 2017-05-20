using System;
using System.Collections.Generic;
using CensusAPIService.Models;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Linq;
using System.Net;
using System.Text;

namespace CensusAPIService
{
    public class BulkApiAgent
    {
        #region Constants

        // Where {0} is returnType, 'locations' or 'geographies'
        const string EndPointRoot = "https://geocoding.geo.census.gov/geocoder/{0}/addressbatch";
        const string Benchmark = "Public_AR_Current";
        const string DefaultReturnType = "locations";

        #endregion

        #region Constructors



        #endregion

        #region Public Methods

        public List<AddressApiResponse> BulkGeocode(List<Address> addresses, string returnType = DefaultReturnType)
        {
            if (addresses.Count() > 1000)
            {
                throw new Exception("BulkApiAgent cannot geocode more than 1000 addresses per request");
            }

            // Move elsewhere
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                                 | SecurityProtocolType.Tls11
                                                 | SecurityProtocolType.Tls12
                                                 | SecurityProtocolType.Ssl3;

            string addressesCsv = "";
            addresses.ForEach(address => addressesCsv = String.Concat(addressesCsv, address.ToCsv()));

            // Convert addresses from list of strings to Byte array to bundle as "file" as required by API
            byte[] addressesAsBytes = Encoding.ASCII.GetBytes(addressesCsv);

            using (var client = new HttpClient())
            {
                try
                {
                    if (Uri.TryCreate(String.Format(EndPointRoot, returnType), UriKind.Absolute, out Uri endpointUrl))
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
                        string resultContent = result.Content.ReadAsStringAsync().Result;
                        var resultSplit = resultContent.Split('\n');

                        // Results return with an extra newline after the last entry, drop the last item
                        resultSplit = resultSplit.Take(resultSplit.Count() - 1).ToArray();

                        var resultAddresses = new List<AddressApiResponse>();
                        resultSplit.ToList().ForEach(addressString => resultAddresses.Add(AddressApiResponse.ParseAddressApiResponseFromCsv(addressString)));

                        return resultAddresses;
                    }
                    else
                    {
                        throw new Exception("Error forming Census endpoint URL");
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        #endregion
    }
}
