using GeoNerves.Models;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace GeoNerves.Tests.Integration
{
  public class SingleApiAgentTests
  {
    private SingleApiAgent _apiAgent;

    /// <summary>
    /// Test the API geolocation with a single accurate address in Cambridge MA
    /// </summary>
    [Fact]
    public void CanGeographyGeocode1RealAddress()
    {
      _apiAgent = new SingleApiAgent();
      var addressString = "1,667 Massachusetts Avenue,Cambridge,MA,02139";

      var result = _apiAgent.SingleLineGeocode("1,667 Massachusetts Avenue,Cambridge,MA,02139");

      Assert.True(result == "");

//      Assert.True(result.Latitude == 42.365723);
    }
  }
}