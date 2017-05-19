using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CensusAPIService.Models;

namespace CensusAPIService.Tests
{
    [TestClass]
    public class CensusGeoLocatorTests
    {
        [TestMethod]
        public void CensusGeoLocator_GeoCodeCsv()
        {
            var correctAddress = new Address()
            {
                UniqueId = 1,
                Street = "667 Massachusetts Avenue",
                City = "Cambridge",
                State = "MA",
                Zip = "02139",
                Latitude = -71.104225,
                Longitude = 42.365723
            };

            var geoLocator = new CensusGeolocator();
            var addresses = geoLocator.GeoCodeCsv(@"1, 667 Massachusetts Avenue, Cambridge, MA, 02139
                                                    2,30 Tyler Street,Boston,MA,02111
                                                    3,216 Norfolk Street,Cambridge,MA,02139
                                                    4,88 Brattle Street,Cambridge,MA,02133
                                                    5,688 Concord Avenue,Belmont,MA,02478");

            var compareAddress = addresses.First(address => address.UniqueId == 1);
            Assert.IsTrue(compareAddress.Equals(correctAddress));
        }

        [TestMethod]
        public void CensusGeoLocator_GeoCodeXml()
        {
            var correctAddress = new Address()
            {
                UniqueId = 1,
                Street = "667 Massachusetts Avenue",
                City = "Cambridge",
                State = "MA",
                Zip = "02139",
                Latitude = -71.104225,
                Longitude = 42.365723
            };

            var geoLocator = new CensusGeolocator();
            var addresses = geoLocator.GeoCodeXml(
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
        }
    }
}