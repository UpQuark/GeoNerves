using CensusAPIService.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace CensusAPIService.Tests
{
    [TestClass]
    public class CensusGeoLocatorTests
    {
        private CensusGeolocator _geoLocator;
        private Address _testAddress1;

        [TestInitialize]
        public void Initialize()
        {
            _geoLocator = new CensusGeolocator();

            _testAddress1 = new Address()
            {
                UniqueId = 1,
                Street = "667 Massachusetts Avenue",
                City = "Cambridge",
                State = "MA",
                Zip = "02139",
                Latitude = -71.104225,
                Longitude = 42.365723
            };
        }

        [TestMethod]
        public void CensusGeoLocator_GeoCodeCsv_5()
        {
            var addresses = _geoLocator.GeoCodeCsv(@"1, 667 Massachusetts Avenue, Cambridge, MA, 02139
                                                    2,30 Tyler Street,Boston,MA,02111
                                                    3,216 Norfolk Street,Cambridge,MA,02139
                                                    4,88 Brattle Street,Cambridge,MA,02133
                                                    5,688 Concord Avenue,Belmont,MA,02478");

            var compareAddress = addresses.First(address => address.UniqueId == 1);
            Assert.IsTrue(compareAddress.Equals(_testAddress1));
        }

        [TestMethod]
        public void CensusGeoLocator_GeoCodeCsv_2200()
        {
            var addressesCsv = "";
            for (int i = 0; i < 2200; i++)
            {
                var address = String.Format("{0}, 667 Massachusetts Avenue, Cambridge, MA, 02139", i);
                if (i < 2199)
                {
                    address = String.Concat(address, Environment.NewLine);
                }

                addressesCsv = String.Concat(addressesCsv, address);
            }

            var addresses = _geoLocator.GeoCodeCsv(addressesCsv);
            var compareAddress = addresses.First(address => address.UniqueId == 1);
            Assert.IsTrue(compareAddress.Equals(_testAddress1));
            Assert.IsTrue(addresses.Count == 2200);
        }

        [TestMethod]
        public void CensusGeoLocator_GeoCodeXml()
        {
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

            var compareAddress = addresses.First(address => address.UniqueId == 1);
            Assert.IsTrue(compareAddress.Equals(_testAddress1));
        }

        [TestMethod]
        public void CensusGeoLocator_GeoCodeJson()
        {
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

            var compareAddress = addresses.First(address => address.UniqueId == 1);
            Assert.IsTrue(compareAddress.Equals(_testAddress1));
        }
    }
}