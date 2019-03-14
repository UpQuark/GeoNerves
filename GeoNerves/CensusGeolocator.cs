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
    private const int CHUNK_SIZE = 10000;
    private readonly IBulkApiAgent _apiAgent;

    public CensusGeolocator()
    {
      _apiAgent = new BulkApiAgent();
    }
    
    public CensusGeolocator(IBulkApiAgent apiAgent)
    {
      _apiAgent = apiAgent;
    }

    /// <summary>
    /// GeoCode a list of addresses in CSV format:
    /// "UniqueId,StreetAddress,City,State,Zip,Latitude,Longitude"
    /// </summary>
    /// <param name="addresses">CSV list of addresses</param>
    /// <returns>GeoCoded list of Address objects</returns>
    public List<Address> GeoCodeCsv(string addresses)
    {
      var addressStrings = addresses.Split(new string[] {Environment.NewLine}, StringSplitOptions.None);
      var addressList = new AddressList();

      addressStrings.ToList().ForEach(address => addressList.Addresses.Add(Address.ParseAddressFromCsv(address)));

      // Split the list of addresses into <= CHUNK_SIZE chunks that the API can consume
      return BulkGeoCodeChunked(addressList);
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
        addressList = (AddressList) serializer.Deserialize(reader);
      }

      return BulkGeoCodeChunked(addressList);
    }

    /// <summary>
    /// GeoCode a list of addresses in JSON format
    /// </summary>
    /// <param name="addresses">JSON list of addresses</param>
    /// <returns>GeoCoded list of Address objects</returns>
    public List<Address> GeoCodeJson(string addresses)
    {
      var addressList = JsonConvert.DeserializeObject<AddressList>(addresses);
      return BulkGeoCodeChunked(addressList);
    }

    /// <summary>
    /// GeoCode a list of addresses from a list of Address objects
    /// </summary>
    /// <param name="addresses">List collection of Address objects</param>
    /// <returns>GeoCoded list of Address objects</returns>
    public List<Address> GeoCodeObjects(List<Address> addresses)
    {
      return BulkGeoCodeChunked(new AddressList() {Addresses = addresses});
    }

    /// <summary>
    /// Split the list of addresses into CHUNK_SIZE-or-fewer length chunks and send them to API
    /// </summary>
    /// <param name="addressList">AddressList of any size</param>
    /// <returns>List of geocoded Addresses</returns>
    /// TODO: This is a source of grief and is not working correctly
    private List<Address> BulkGeoCodeChunked(AddressList addressList)
    {
      var addressResponse = new List<AddressApiResponse>();
      var addressListChunked = AddressChunkGenerator.SplitAddressChunks(addressList, CHUNK_SIZE);
      
      Parallel.ForEach(addressListChunked, subList =>
      {
        var response = _apiAgent.BulkGeocode(subList);
        addressResponse.AddRange(response);
      });

      return addressResponse.Select(response => response.Address).ToList();
    }
  }
}