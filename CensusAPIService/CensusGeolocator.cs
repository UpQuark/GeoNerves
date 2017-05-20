using CensusAPIService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace CensusAPIService
{
    public class CensusGeolocator
    {
        #region Public methods

        public List<Address> GeoCodeCsv(string addresses)
        {
            var apiAgent = new BulkApiAgent();
            var addressStrings = addresses.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

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

        public List<Address> GeoCodeJson(string addresses)
        {
            var apiAgent = new BulkApiAgent();
            var addressList = JsonConvert.DeserializeObject<AddressList>(addresses);

            var addressResponse = apiAgent.BulkGeocode(addressList.Addresses);
            return addressResponse.Select(response => response.Address).ToList();
        }

        public List<Address> GeoCodeObjects(List<Address> addresses)
        {
            var apiAgent = new BulkApiAgent();

            var addressResponse = apiAgent.BulkGeocode(addresses);
            return addressResponse.Select(response => response.Address).ToList();
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
