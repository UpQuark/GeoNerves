using GeoNerves.Models;
using Xunit;
using System;
using System.Linq;
using System.Text;

namespace GeoNerves.Tests.Integration
{
  public class CensusGeoLocatorTests
  {
    private CensusGeolocator _geoLocator;
    private Address          _testAddress1;

    #region Tests

    [Fact]
    public void CanGeoCodeAddressesFromCsv_1()
    {
      _geoLocator = new CensusGeolocator();

      _testAddress1 = new Address
      {
        Id  = 1,
        Street    = "667 Massachusetts Avenue",
        City      = "Cambridge",
        State     = "MA",
        Zip       = "02139",
        Latitude  = 42.365723,
        Longitude  = -71.104225
      };


      var addresses = _geoLocator.GeoCodeCsv(
        @"1,667 Massachusetts Avenue,Cambridge,MA,02139"
      );

      var compareAddress = addresses.First(address => address.Id == 1);
      Assert.True(compareAddress.Equals(_testAddress1));
    }
    
    [Fact]
    public void CanGeoCodeAddressesFromCsv_5()
    {
      _geoLocator = new CensusGeolocator();

      _testAddress1 = new Address
      {
        Id  = 1,
        Street    = "667 Massachusetts Avenue",
        City      = "Cambridge",
        State     = "MA",
        Zip       = "02139",
        Latitude  = 42.365723,
        Longitude  = -71.104225
      };


      var addresses = _geoLocator.GeoCodeCsv(
        @"1,667 Massachusetts Avenue,Cambridge,MA,02139
        2,30 Tyler Street,Boston,MA,02111
        3,216 Norfolk Street,Cambridge,MA,02139
        4,688 Concord Avenue,Belmont,MA,02478,
        5,244 Elm St,Cambridge,MA,02139"
      );

      var compareAddress = addresses.First(address => address.Id == 1);
      Assert.True(compareAddress.Equals(_testAddress1));
    }

    [Fact]
    public void CanGeoCodeAddressesFromCsv_2200()
    {
      _geoLocator = new CensusGeolocator();

      _testAddress1 = new Address
      {
        Id  = 1,
        Street    = "667 Massachusetts Avenue",
        City      = "Cambridge",
        State     = "MA",
        Zip       = "02139",
        Latitude  = 42.365723,
        Longitude  = -71.104225
      };

      // Generates the same address n times with different IDs. 2200 tests that the GeoLocator can
      // break up a block of addresses that where n % 1000 != 0
      var addresses      = _geoLocator.GeoCodeCsv(GenerateCsvInput(2200));
      var compareAddress = addresses.First(address => address.Id == 1);
      Assert.True(compareAddress.Equals(_testAddress1));
      Assert.True(addresses.Count == 2200);
    }

    [Fact]
    public void CanGeoCodeAddressesFromCsv_25500()
    {
      _geoLocator = new CensusGeolocator();

      _testAddress1 = new Address
      {
        Id  = 1,
        Street    = "667 Massachusetts Avenue",
        City      = "Cambridge",
        State     = "MA",
        Zip       = "02139",
        Latitude  = 42.365723,
        Longitude  = -71.104225
      };

      var addresses      = _geoLocator.GeoCodeCsv(GenerateCsvInput(25500));
      var compareAddress = addresses.First(address => address.Id == 1);
      Assert.True(compareAddress.Equals(_testAddress1));
      Assert.True(addresses.Count == 25500);
    }

    [Fact]
    public void CanGeoCodeAddressesFromXml_2()
    {
      _geoLocator = new CensusGeolocator();

      _testAddress1 = new Address
      {
        Id  = 1,
        Street    = "667 Massachusetts Avenue",
        City      = "Cambridge",
        State     = "MA",
        Zip       = "02139",
        Latitude  = 42.365723,
        Longitude  = -71.104225
      };

      var addresses = _geoLocator.GeoCodeXml(
        @"<Addresses>
	                <Address>
		                <UniqueId>1</UniqueId>
		                <Street>667 Massachusetts Avenue</Street>
		                <City>Cambridge</City>
		                <State>MA</State>
		                <Zip>02139</Zip>
	                </Address>
	                <Address>
		                <UniqueId>2</UniqueId>
		                <Street>675 Massachusetts Avenue</Street>
		                <City>Cambridge</City>
		                <State>MA</State>
		                <Zip>02139</Zip>
	                </Address>
                </Addresses>"
      );

      var compareAddress = addresses.First(address => address.Id == 1);
      Assert.True(compareAddress.Equals(_testAddress1));
    }

    [Fact]
    public void CanGeoCodeAddressesFromXml_2500()
    {
      _geoLocator = new CensusGeolocator();

      _testAddress1 = new Address
      {
        Id  = 1,
        Street    = "667 Massachusetts Avenue",
        City      = "Cambridge",
        State     = "MA",
        Zip       = "02139",
        Latitude  = 42.365723,
        Longitude  = -71.104225
      };

      var addresses      = _geoLocator.GeoCodeXml(GenerateXmlInput(2500));
      var compareAddress = addresses.First(address => address.Id == 1);
      Assert.True(compareAddress.Equals(_testAddress1));
      Assert.True(addresses.Count == 2500);
    }

    [Fact]
    public void CanGeoCodeAddressesFromJson_2()
    {
      _geoLocator = new CensusGeolocator();

      _testAddress1 = new Address
      {
        Id  = 1,
        Street    = "667 Massachusetts Avenue",
        City      = "Cambridge",
        State     = "MA",
        Zip       = "02139",
        Latitude  = 42.365723,
        Longitude  = -71.104225
      };

      var addresses = _geoLocator.GeoCodeJson(
        @"{
	                ""Addresses"": [{
			                ""UniqueId"": 1,
			                ""Street"": ""667 Massachusetts Avenue"",
			                ""City"": ""Cambridge"",
			                ""State"": ""MA"",
			                ""Zip"": ""02139""
		                },
		                {
			                ""UniqueId"": 2,
			                ""Street"": ""675 Massachusetts Avenue"",
			                ""City"": ""Cambridge"",
			                ""State"": ""MA"",
			                ""Zip"": ""02139""
		                }
	                ]
                }"
      );

      var compareAddress = addresses.First(address => address.Id == 1);
      Assert.True(compareAddress.Equals(_testAddress1));
    }

    [Fact]
    public void CanGeoCodeAddressesFromJson_2500()
    {
      _geoLocator = new CensusGeolocator();

      _testAddress1 = new Address
      {
        Id  = 1,
        Street    = "667 Massachusetts Avenue",
        City      = "Cambridge",
        State     = "MA",
        Zip       = "02139",
        Latitude  = 42.365723,
        Longitude  = -71.104225
      };

      var addresses      = _geoLocator.GeoCodeJson(GenerateJsonInput(2500));
      var compareAddress = addresses.First(address => address.Id == 1);
      Assert.True(compareAddress.Equals(_testAddress1));
      Assert.True(addresses.Count == 2500);
    }

    #endregion

    #region Private Methods

    private string GenerateCsvInput(int length)
    {
      var builder = new StringBuilder();

      for (int i = 0; i < length; i++)
      {
        var address = $"{i}, 667 Massachusetts Avenue, Cambridge, MA, 02139";
        if (i < length - 1)
        {
          address = string.Concat(address, Environment.NewLine);
        }

        builder.Append(address);
      }

      return builder.ToString();
    }

    private string GenerateXmlInput(int length)
    {
      // Could alternately do with a serializer, but it's already very simple
      var builder = new StringBuilder();

      for (int i = 0; i < length; i++)
      {
        builder.Append(
          $@" <Address>
		            <UniqueId>{i}</UniqueId>
		            <Street>667 Massachusetts Avenue</Street>
		            <City>Cambridge</City>
		            <State>MA</State>
		            <Zip>02139</Zip>
	            </Address>");
      }

      var xmlRoot =
        $@"<Addresses>
	          {builder}
           </Addresses>";

      return xmlRoot;
    }

    private string GenerateJsonInput(int length)
    {
      // Could alternately do with a serializer, but it's already very simple
      var builder = new StringBuilder();

      for (int i = 0; i < length; i++)
      {
        builder.Append(
          $@"{{
            ""UniqueId"": {i},
			      ""Street"": ""667 Massachusetts Avenue"",
			      ""City"": ""Cambridge"",
			      ""State"": ""MA"",
			      ""Zip"": ""02139""
		      }}");

        if (i < length - 1)
        {
          builder.Append(",");
        }
      }

      var jsonRoot = $@"{{
	                ""Addresses"": [{builder}]
                }}";

      return jsonRoot;
    }

    #endregion
  }
}