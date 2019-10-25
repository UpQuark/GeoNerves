using GeoNerves.Models;
using Xunit;

namespace GeoNerves.Tests.Unit
{
  public class ApiResponseParserTests
  {
    [Fact]
    public void CanParseCensusCsv()
    {
      var addressResponseString =
        "\"3\",\"216 Norfolk Street, Cambridge, MA, 02139\",\"Match\",\"Exact\",\"216 NORFOLK ST, CAMBRIDGE, MA, 02139\",\"-71.098366,42.369587\",\"86869427\",\"R\"";
      var addressResponse = ApiResponseParser.ParseAddressFromApiResponse(addressResponseString);

      Assert.True(addressResponse.City == "Cambridge");
    }
  }
}