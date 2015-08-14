using System;
using GeoNerves;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace GeoNerves.Tests
{
    [TestClass]
    public class GeoNervesTests
    {
        [TestMethod]
        public void TestAddressCountDbCall()
        {
            var dbReader = new GeoNervesDbReader();
            var CountResponse = dbReader.GetCountFromDb();
            Assert.IsTrue(CountResponse > 0);
        }

        [TestMethod]
        public void TestAddressBatchDbCall()
        {
            var dbReader = new GeoNervesDbReader();
            DataTable Addresses = dbReader.GetAddressesFromDb(1, 999);
            Assert.IsTrue(Addresses.Rows.Count > 0);
        }

        [TestMethod]
        public void TestAddressCsvPackaging()
        {
            var dbReader = new GeoNervesDbReader();
            DataTable Addresses = dbReader.GetAddressesFromDb(1, 999);
            var filePackager = new GeoNervesFilePackager();
            filePackager.ExportRowsToFile(Addresses);
            Assert.IsTrue(File.Exists(filePackager.FilePath));
        }

        [TestMethod]
        public void TestAddressCsvSendToApi()
        {
            var dbReader = new GeoNervesDbReader();
            DataTable Addresses = dbReader.GetAddressesFromDb(1, 999);
            var filePackager = new GeoNervesFilePackager();
            filePackager.ExportRowsToFile(Addresses);

            var apiAgent = new GeoNervesApiAgent();
            string result = apiAgent.SendAddressesToApi(filePackager.FilePath);
            Assert.IsTrue(result[0].Equals('\"'));
        }

        [TestMethod]
        public void TestGeocodeToDB()
        {
            var dbReader = new GeoNervesDbReader();
            DataTable Addresses = dbReader.GetAddressesFromDb(1, 2);
            var filePackager = new GeoNervesFilePackager();
            filePackager.ExportRowsToFile(Addresses);

            var apiAgent = new GeoNervesApiAgent();
            string result = apiAgent.SendAddressesToApi(filePackager.FilePath);

            var dbWriter = new GeoNervesDbWriter();
            dbWriter.WriteGeolocationToDB(result);
            //Assert.IsTrue(result[0].Equals('\"'));
        }
    }
}