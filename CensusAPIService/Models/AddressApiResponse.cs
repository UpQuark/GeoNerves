using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CensusAPIService.Models
{
    public class AddressApiResponse
    {
        public Address Address { get; set; }
        public Address AddressCorrected { get; set; }
        public bool Match { get; set; }
        public string MatchType { get; set; }

        // The API returns two fields, example values "86869879","R" which aren't documented
        public string TigerLineId { get; set; }
        public string Side { get; set; }

        public static AddressApiResponse ParseAddressApiResponseFromCsv(string addressApiResponse)
        {
            var split = addressApiResponse.Split(new string[] { ",\"" }, StringSplitOptions.None);

            var splitClean = new List<string>();
            split.ToList().ForEach(item => splitClean.Add(item.Replace("\\", "").Replace("\"", "")));

            var coords = splitClean.Count > 5 ? splitClean[5].Split(',') : null;

            var response = new AddressApiResponse()
            {
                Address = Address.ParseAddressFromCsv(splitClean[1], Convert.ToInt32(splitClean[0])),
                Match = splitClean[2] == "Match" ? true : false,
                MatchType = splitClean.Count > 3 ? splitClean[3] : null,
                AddressCorrected = splitClean.Count > 4 ? Address.ParseAddressFromCsv(splitClean[4], Convert.ToInt32(splitClean[0])) : null,
                TigerLineId = splitClean.Count > 6 ? splitClean[6] : null,
                Side = splitClean.Count > 7 ? splitClean[7]: null
            };

            if (coords != null)
            {
                response.Address.Latitude = Convert.ToDouble(coords[0]);
                response.Address.Longitude = Convert.ToDouble(coords[1]);
                response.AddressCorrected.Latitude = Convert.ToDouble(coords[0]);
                response.AddressCorrected.Longitude = Convert.ToDouble(coords[1]);
            }
            
            return response;
        }
    }
}
