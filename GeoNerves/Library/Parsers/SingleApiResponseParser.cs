using System;
using System.Text.RegularExpressions;
using GeoNerves.Models;
using Newtonsoft.Json.Linq;

namespace GeoNerves
{
  public static class SingleApiResponseParser
  {
    public static Address ParseAddressFromApiResponse(string responseJson)
    {
      var responseObject = JObject.Parse(responseJson);
      var matches        = responseObject["result"]["addressMatches"];
      var bestMatch      = matches[0];

      var address = new Address
                    {
                      Id = 1,
                      Street = bestMatch["addressComponents"]["streetName"] + " " +
                               bestMatch["addressComponents"]["suffixType"],
                      City              = bestMatch["addressComponents"]["city"].ToString(),
                      State             = bestMatch["addressComponents"]["state"].ToString(),
                      Zip               = bestMatch["addressComponents"]["zip"].ToString(),
                      Latitude          = Convert.ToDouble(bestMatch["coordinates"]["y"]),
                      Longitude         = Convert.ToDouble(bestMatch["coordinates"]["x"]),
                      Match             = true,
                      MatchType         = null,
                      NormalizedAddress = bestMatch["matchedAddress"].ToString(),
                      TigerLineId       = null,
                      TigerLineSide     = null,
                      StateId           = 0,
                      CountyId          = 0,
                      CensusTractId     = 0,
                      BlockId           = 0
                    };
      return address;
    }
  }
}