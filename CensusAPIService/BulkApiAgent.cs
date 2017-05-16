using System;
using System.Collections.Generic;
using CensusAPIService.Models;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Linq;
using System.Net;

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

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="addresses"></param>
        /// <param name="returnType"></param>
        /// <returns></returns>
        public List<String> BulkGeocode(List<string> addresses, string returnType = DefaultReturnType)
        {
            // Move elsewhere
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                                | SecurityProtocolType.Tls11
                                                | SecurityProtocolType.Tls12
                                                | SecurityProtocolType.Ssl3;

            // Append newline characters to each addresses so API can distinguish
            addresses.ForEach(address => address = String.Concat(address, Environment.NewLine));

            // Convert addresses from list of strings to Byte array to bundle as "file" as required by API
            byte[] addressesAsBytes = addresses
              .SelectMany(s => System.Text.Encoding.ASCII.GetBytes(s))
              .ToArray();

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
                        //return resultContent.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

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


            return null;
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
