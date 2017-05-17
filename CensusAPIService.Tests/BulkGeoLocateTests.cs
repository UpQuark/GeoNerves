using System;
using System.Collections.Generic;
using CensusAPIService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CensusAPIService.Tests
{
    [TestClass]
    public class BulkGeoLocateTests
    {
        private BulkApiAgent _apiAgent;

        [TestInitialize]
        public void Initialize()
        {
            _apiAgent = new BulkApiAgent();
        }
       
        /// <summary>
        /// Test the API geolocation with a single accurate address in Cambridge MA
        /// </summary>
        [TestMethod]
        public void BulkApiAgent_GeoCode1CorrectAddress()
        {
            var testAddressList = new List<string>()
            {
                "1,667 Massachusetts Avenue,Cambridge,MA,02139"
            };

            _apiAgent.BulkGeocode(testAddressList);
        }

        /// <summary>
        /// Test the API geolocation with 5 accurate addresses in MA
        /// </summary>
        [TestMethod]
        public void BulkApiAgent_GeoCode5CorrectAddresses()
        {
            var testAddressList = new List<string>
            {
                "1,667 Massachusetts Avenue,Cambridge,MA,02139",
                "2,30 Tyler Street,Boston,MA,02111",
                "3,216 Norfolk Street,Cambridge,MA,02139",
                "4,88 Brattle Street,Cambridge,MA,02133",
                "5,688 Concord Avenue,Belmont,MA,02478",
            };

            var result = _apiAgent.BulkGeocode(testAddressList);
        }

        /// <summary>
        /// Test the API geolocation with 1 nonexistant address in MA
        /// </summary>
        [TestMethod]
        public void BulkApiAgent_GeoCode1BogusAddress()
        {
            var testAddressList = new List<string>
            {
                "1,9999 Massssachusetts Avenue,Cambridge,MA,02139",
            };

            _apiAgent.BulkGeocode(testAddressList);
        }
    }
}
