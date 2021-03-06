﻿using GeoNerves.Models;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace GeoNerves.Tests.Integration
{
  public class BulkApiAgentTests
  {
    private BulkApiAgent _apiAgent;

    /// <summary>
    /// Test the API geolocation with a single accurate address in Cambridge MA
    /// </summary>
    [Fact]
    public void CanGeoCode1RealAddress()
    {
      _apiAgent = new BulkApiAgent();
      var testAddressList = new List<Address>
      {
        AddressParser.ParseAddressFromCsv("1,667 Massachusetts Avenue,Cambridge,MA,02139")
      };

      var result = _apiAgent.BulkGeocode(testAddressList);

      Assert.True(result[0].Latitude == 42.365723);
    }

    /// <summary>
    /// Test the API geolocation with 5 accurate addresses in MA
    /// </summary>
    [Fact]
    public void CanGeoCode5RealAddresses()
    {
      _apiAgent = new BulkApiAgent();
      var testAddressList = new List<Address>
      {
        AddressParser.ParseAddressFromCsv("1,667 Massachusetts Avenue,Cambridge,MA,02139"),
        AddressParser.ParseAddressFromCsv("2,30 Tyler Street,Boston,MA,02111"),
        AddressParser.ParseAddressFromCsv("3,216 Norfolk Street,Cambridge,MA,02139"),
        AddressParser.ParseAddressFromCsv("4,688 Concord Avenue,Belmont,MA,02478"),
        AddressParser.ParseAddressFromCsv("5,244 Elm St,Cambridge,MA,02139")
      };

      var result = _apiAgent.BulkGeocode(testAddressList);
      var compareAddress = result.First(addressResponse => addressResponse.Id == 1);

      Assert.True(compareAddress.Latitude == 42.365723);
    }

    /// <summary>
    /// Test the API geolocation with 1 nonexistent address in MA
    /// </summary>
    [Fact]
    public void CantGeoCode1BogusAddress()
    {
      _apiAgent = new BulkApiAgent();
      var testAddressList = new List<Address>
      {
        AddressParser.ParseAddressFromCsv("1,9999 Massachusetts Avenue,Cramburdge,MA,02139")
      };

      var result = _apiAgent.BulkGeocode(testAddressList);

      Assert.True(result[0].Match == false);
    }
  }
}