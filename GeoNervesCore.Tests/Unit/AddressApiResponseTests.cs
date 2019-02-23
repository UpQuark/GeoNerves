using GeoNerves.Models;
using Xunit;

namespace GeoNerves.Tests
{
  public class AddressApiResponseTests
  {
    [Fact]
    public void CanParseCensusCsv()
    {
      var addressResponseString =
        "\"3\",\"216 Norfolk Street, Cambridge, MA, 02139\",\"Match\",\"Exact\",\"216 NORFOLK ST, CAMBRIDGE, MA, 02139\",\"-71.098366,42.369587\",\"86869427\",\"R\"";
      var addressResponse = AddressApiResponse.ParseAddressApiResponseFromCsv(addressResponseString);

      Assert.True(addressResponse.Address.City == "Cambridge");
    }
  }
}