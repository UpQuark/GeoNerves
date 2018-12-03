using GeoNerves.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace GeoNerves
{
    /// <summary>
    /// Friendly geolocation interface that can be passed multiple formats of address objects for geolocation
    /// </summary>
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

        /// <summary>
        /// GeoCode a list of addresses in CSV format:
        /// "UniqueId,StreetAddress,City,State,Zip,Latitude,Longitude"
        /// </summary>
        /// <param name="addresses">CSV list of addresses</param>
        /// <returns>GeoCoded list of Address objects</returns>
        public List<Address> GeoCodeCsv(string addresses)
        {
            var addressStrings = addresses.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            var addressList = new AddressList();

            addressStrings.ToList().ForEach(address => addressList.Addresses.Add(Address.ParseAddressFromCsv(address)));

            // Split the list of addresses into < 1000-length chunks that the API can consume
            return BulkGeoCodeSplit(addressList);
        }

        /// <summary>
        /// GeoCode a list of addressess in XML format
        /// </summary>
        /// <param name="addresses">XML list of addresses</param>
        /// <<returns>GeoCoded list of Address objects</returns>
        public List<Address> GeoCodeXml(string addresses)
        {
            var serializer = new XmlSerializer(typeof(AddressList));
            var addressList = new AddressList();

            using (TextReader reader = new StringReader(addresses))
            {
                addressList = (AddressList)serializer.Deserialize(reader);
            }

            return BulkGeoCodeSplit(addressList);
        }

        /// <summary>
        /// GeoCode a list of addresses in JSON format
        /// </summary>
        /// <param name="addresses">JSON list of addresses</param>
        /// <returns>GeoCoded list of Address objects</returns>
        public List<Address> GeoCodeJson(string addresses)
        {
            var addressList = JsonConvert.DeserializeObject<AddressList>(addresses);
            return BulkGeoCodeSplit(addressList);
        }

        /// <summary>
        /// GeoCode a list of addresses from a list of Address objects
        /// </summary>
        /// <param name="addresses">List collection of Address objects</param>
        /// <returns>GeoCoded list of Address objects</returns>
        public List<Address> GeoCodeObjects(List<Address> addresses)
        {
            return BulkGeoCodeSplit(new AddressList() { Addresses = addresses }); ;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Split the list of addresses into 1000-or-fewer length chunks and send them to API
        /// </summary>
        /// <param name="addressList">AddressList of any size</param>
        /// <returns>List of geocoded Addresses</returns>
        private List<Address> BulkGeoCodeSplit(AddressList addressList)
        {
            var addressResponse = new List<AddressApiResponse>();
            var addressListSplit = new List<List<Address>>();

            for (int i = 0; i < addressList.Addresses.Count(); i += 500)
            {
                var newList = new AddressList();
                var remainder = addressList.Addresses.Count % 500;

                var offset = (remainder != 0 && i < addressList.Addresses.Count - remainder) ? 500 : remainder;

                addressListSplit.Add(new List<Address>(addressList.Addresses.GetRange(i, offset)));
            }

            Parallel.ForEach(addressListSplit, subList =>
            {
                var response = _apiAgent.BulkGeocode(subList);
                addressResponse.AddRange(response);
            });

            return addressResponse.Select(response => response.Address).ToList();
        }

        #endregion
    }
}
