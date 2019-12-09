using System;
using System.Collections.Generic;
using System.Linq;
using GeoNerves.Models;

namespace GeoNerves
{
  /// <summary>
  /// Parser for raw CSV text responses from the U.S. census geolocation API
  /// </summary>
  public static class BulkApiResponseParser
  {
    /// <summary>
    /// Parse object from the raw CSV response from location search API
    /// </summary>
    /// <param name="addressApiResponse">Raw CSV from API response</param>
    /// <returns>AddressApiResponse object populated with values from CSV</returns>
    public static Address ParseAddressFromApiResponse(string addressApiResponse)
    {
      var addressAttributes = TrimAndSplitCsv(addressApiResponse);

      var address = new Address
                    {
                      Id    = int.Parse(addressAttributes[0]),
                      Match = addressAttributes[2] == "Match"
                    };

      if (address.Match == true)
      {
        var coords = addressAttributes[5].Split(',');

        address.MatchType         = addressAttributes[3];
        address.NormalizedAddress = addressAttributes[4];
        address.Latitude          = Convert.ToDouble(coords[1]);
        address.Longitude         = Convert.ToDouble(coords[0]);
        address.TigerLineId       = long.Parse(addressAttributes[6]);
        address.TigerLineSide     = addressAttributes[7];
        address.StateId           = int.Parse(addressAttributes[8]);
        address.CountyId          = int.Parse(addressAttributes[9]);
        address.CensusTractId     = int.Parse(addressAttributes[10]);
        address.BlockId           = int.Parse(addressAttributes[11]);

        var splitAddress = address.NormalizedAddress.Split(',');

        address.Street = splitAddress[0];
        address.City   = splitAddress[1];
        address.State  = splitAddress[2];
        address.Zip    = splitAddress[3];
      }

      return address;
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