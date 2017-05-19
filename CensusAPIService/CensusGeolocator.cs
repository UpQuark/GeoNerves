using CensusAPIService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace CensusAPIService
{
    public class CensusGeolocator
    {
        public List<Address> GeoCodeCsv(string addresses)
        {
            var apiAgent = new BulkApiAgent();
            var addressStrings = addresses.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            if (addressStrings.Count() > 1000)
            {
                throw new Exception("Exceeded limit of 1000 addresses per geocode request");
            }

            var addressList = new List<Address>();
            addressStrings.ToList().ForEach(address => addressList.Add(Address.ParseAddressFromCsv(address)));

            var addressResponse = apiAgent.BulkGeocode(addressList);
            return addressResponse.Select(response => response.Address).ToList();
        }

        public List<Address> GeoCodeXml(string addresses)
        {
            var apiAgent = new BulkApiAgent();

            var serializer = new XmlSerializer(typeof(AddressList));
            var addressList = new AddressList();

            using (TextReader reader = new StringReader(addresses))
            {
                addressList = (AddressList)serializer.Deserialize(reader);
            }

            var addressResponse = apiAgent.BulkGeocode(addressList.Addresses);
            return addressResponse.Select(response => response.Address).ToList();
        }

        public IEnumerable<Address> GeoCodeJson(string addresses)
        {
            return null;
        }

        public IEnumerable<Address> GeoCodeObjects(List<Address> addresses)
        {
            return null;
        }
    }
}
