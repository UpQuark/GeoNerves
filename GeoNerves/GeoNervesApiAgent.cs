using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Net.Http.Headers;

namespace GeoNerves
{
    /// <summary>
    /// Agent for sending of address CSV files to census API and retrieving results
    /// </summary>
    public class GeoNervesApiAgent
    {
        const String CensusBatchGeoCodeUri = "http://geocoding.geo.census.gov/geocoder/locations/addressbatch";

        public string SendAddressesToApi(string csvPath)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(CensusBatchGeoCodeUri);
                var content = new MultipartFormDataContent();

                var fileContent = new ByteArrayContent(System.IO.File.ReadAllBytes("addresses.csv"));
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "addressFile",
                    FileName = "addresses.csv"
                };
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                content.Add(fileContent);

                // Not a FormUrlEncodedContent class due to an ostensible bug in census API that
                // rejects key/value formatting and requires 'benchmark' in a 'name' field
                var benchMarkContent = new StringContent("9");
                benchMarkContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "benchmark"
                };
                content.Add(benchMarkContent);

                var result = client.PostAsync("", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                return resultContent;
            }
        }
    }
}
