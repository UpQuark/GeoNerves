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
        #region Properties and Fields

        BulkApiAgent _apiAgent;

        #endregion

        #region Constructors

        public CensusGeolocator()
        {
            _apiAgent = new BulkApiAgent();
        }

        #endregion

        #region Public Methods

        public List<Address> GeoCodeCsv(string addresses)
        {
            var addressStrings = addresses.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            var addressList = new AddressList();

            addressStrings.ToList().ForEach(address => addressList.Addresses.Add(Address.ParseAddressFromCsv(address)));

            var addressResponse = new List<AddressApiResponse>();

            // Split the list of addresses into < 1000-length chunks that the API can consume
            for (int i = 0; i < addressList.Addresses.Count(); i += 1000)
            {
                var newList = new AddressList();
                var remainder = addressList.Addresses.Count % 1000;

                var offset = (remainder != 0 && i < addressList.Addresses.Count - remainder) ? 1000 : remainder;

                var response = _apiAgent.BulkGeocode(addressList.Addresses.GetRange(i, offset));
                addressResponse.AddRange(response);
            }

            return addressResponse.Select(response => response.Address).ToList();
        }

        public List<Address> GeoCodeXml(string addresses)
        {
            var serializer = new XmlSerializer(typeof(AddressList));
            var addressList = new AddressList();

            using (TextReader reader = new StringReader(addresses))
            {
                addressList = (AddressList)serializer.Deserialize(reader);
            }

            var addressResponse = _apiAgent.BulkGeocode(addressList.Addresses);
            return addressResponse.Select(response => response.Address).ToList();
        }

        public List<Address> GeoCodeJson(string addresses)
        {
            var addressList = JsonConvert.DeserializeObject<AddressList>(addresses);

            var addressResponse = _apiAgent.BulkGeocode(addressList.Addresses);
            return addressResponse.Select(response => response.Address).ToList();
        }

        public List<Address> GeoCodeObjects(List<Address> addresses)
        {
            var addressResponse = _apiAgent.BulkGeocode(addresses);
            return addressResponse.Select(response => response.Address).ToList();
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
