using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GeoNerves.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace GeoNerves
{
  public static class ApiResponseParser
  {
    /// <summary>
    /// Parse object from the raw CSV response from location search API
    /// </summary>
    /// <param name="addressApiResponse">Raw CSV from API response</param>
    /// <returns>AddressApiResponse object populated with values from CSV</returns>
    public static Address ParseAddressApiResponseFromCsv(string addressApiResponse)
    {
      var addressAttributes = TrimAndSplitCsv(addressApiResponse);
      var isMatch = addressAttributes[2] == "Match";
      
      // Match values to properties by index, because they are unlabeled
      var coords = addressAttributes.Count > 5 ? addressAttributes[5].Split(',') : null;

      var address = AddressParser.ParseAddressFromCsv(addressAttributes[1], Convert.ToInt32(addressAttributes[0]));
      address.Match = addressAttributes[2] == "Match";

      if (address.Match == true)
      {
        
      }
        
        
//      var response = new Address
//      {
//        MatchType = addressAttributes.Count > 3 ? addressAttributes[3] : null,
//        AddressNormalized = addressAttributes.Count > 4
//          ? Address.ParseAddressFromCsv(addressAttributes[4], Convert.ToInt32(addressAttributes[0]))
//          : null,
//        TigerLineId   = addressAttributes.Count > 6 ? addressAttributes[6] : null,
//        TigerLineSide = addressAttributes.Count > 7 ? addressAttributes[7] : null
//      };

      if (coords == null) return response;
      response.Address.Latitude            = Convert.ToDouble(coords[0]);
      response.Address.Longitude           = Convert.ToDouble(coords[1]);
      response.AddressNormalized.Latitude  = Convert.ToDouble(coords[0]);
      response.AddressNormalized.Longitude = Convert.ToDouble(coords[1]);

      return response;
    }

    // Split CSV and clean it of delimiters
    private static List<string> TrimAndSplitCsv(string csv)
    {
      var split      = csv.Split(new[] {",\""}, StringSplitOptions.None);
      var splitClean = new List<string>();
      split.ToList().ForEach(item => splitClean.Add(item.Replace("\\", "").Replace("\"", "")));
      return splitClean;
    }
  }
}