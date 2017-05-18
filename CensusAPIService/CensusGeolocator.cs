using CensusAPIService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace CensusAPIService
{
    public class CensusGeolocator
    {
        public IEnumerable<Address> GeoCodeCsv(string addresses)
        {
            var apiAgent = new BulkApiAgent();
            var addressStrings = addresses.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            var addressList = new List<Address>();
            addressStrings.ToList().ForEach(address => addressList.Add(Address.ParseAddressFromCsvString(address)));

            apiAgent.BulkGeocode(addressList);

            return addressList;
        }

        public IEnumerable<Address> GeoCodeCsv(FileStream addresses)
        {
            return null;
        }

        public IEnumerable<Address> GeoCodeXml(string addresses)
        {
            return null;
        }

        public IEnumerable<Address> GeoCodeXml(FileStream addresses)
        {
            return null;
        }

        public IEnumerable<Address> GeoCodeJson(string addresses)
        {
            return null;
        }

        public IEnumerable<Address> GeoCodeJson(FileStream addresses)
        {
            return null;
        }

        public IEnumerable<Address> GeoCodeObjects(List<Address> addresses)
        {
            return null;
        }

        
    }
}
