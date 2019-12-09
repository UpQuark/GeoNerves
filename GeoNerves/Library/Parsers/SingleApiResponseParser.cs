using GeoNerves.Models;
using Newtonsoft.Json.Linq;

namespace GeoNerves
{
  public static class SingleApiResponseParser
  {
    public static Address ParseAddressFromApiResponse(string addressApiResponse)
    {
      var addressDynamic = JObject.Parse(addressApiResponse);
      
      var address = new Address()
      {
        
      };
      return address;
    }
  }
}