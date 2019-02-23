using System;
using System.Collections.Generic;
using System.Linq;

namespace GeoNerves.Models
{
  /// <summary>
  /// A Census API response for Location search
  /// (Geography search is unsupported at the moment)
  /// </summary>
  public class AddressApiResponse
  {
    public Address Address           { get; set; }
    public Address AddressNormalized { get; set; }
    public bool    Match             { get; set; }
    public string  MatchType         { get; set; }
    public string  TigerLineId       { get; set; }
    public string  TigerLineSide     { get; set; }

    /// <summary>
    /// Parse object from the raw CSV response from location search API
    /// </summary>
    /// <param name="addressApiResponse">Raw CSV from API response</param>
    /// <returns>AddressApiResponse object populated with values from CSV</returns>
    public static AddressApiResponse ParseAddressApiResponseFromCsv(string addressApiResponse)
    {
      // Split CSV and clean it of delimiters
      var split      = addressApiResponse.Split(new string[] {",\""}, StringSplitOptions.None);
      var splitClean = new List<string>();
      split.ToList().ForEach(item => splitClean.Add(item.Replace("\\", "").Replace("\"", "")));

      // Match values to properties by index, because they are unlabeled
      var coords = splitClean.Count > 5 ? splitClean[5].Split(',') : null;

      var response = new AddressApiResponse()
      {
        Address   = Address.ParseAddressFromCsv(splitClean[1], Convert.ToInt32(splitClean[0])),
        Match     = splitClean[2] == "Match" ? true : false,
        MatchType = splitClean.Count > 3 ? splitClean[3] : null,
        AddressNormalized = splitClean.Count > 4
          ? Address.ParseAddressFromCsv(splitClean[4], Convert.ToInt32(splitClean[0]))
          : null,
        TigerLineId   = splitClean.Count > 6 ? splitClean[6] : null,
        TigerLineSide = splitClean.Count > 7 ? splitClean[7] : null
      };

      if (coords != null)
      {
        response.Address.Latitude            = Convert.ToDouble(coords[0]);
        response.Address.Longitude           = Convert.ToDouble(coords[1]);
        response.AddressNormalized.Latitude  = Convert.ToDouble(coords[0]);
        response.AddressNormalized.Longitude = Convert.ToDouble(coords[1]);
      }

      return response;
    }
  }
}