using GeoNerves.Models;
using System.Collections.Generic;
using System.Linq;
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
    private readonly AddressDeserializer _deserializer;
    
    public CensusGeolocator(IBulkApiAgent apiAgent = null)
    {
      if (apiAgent == null)
        apiAgent = new BulkApiAgent();
      
      _apiAgent = apiAgent;
      _deserializer = new AddressDeserializer();
    }

    /// <summary>
    /// GeoCode a list of addresses in CSV format:
    /// "UniqueId,StreetAddress,City,State,Zip,Latitude,Longitude"
    /// </summary>
    /// <param name="addresses">CSV list of addresses</param>
    /// <returns>GeoCoded list of Address objects</returns>
    public List<Address> GeoCodeCsv(string addresses)
    {
      var list = _deserializer.DeserializeCsv(addresses);
      return BulkGeoCodeChunked(list);
    }

    /// <summary>
    /// Return the number of addresses in a string
    /// </summary>
    /// <param name="addresses"></param>
    /// <returns></returns>
    public int CountCsv(string addresses)
    {
      return _deserializer.DeserializeCsv(addresses).Addresses.Count;
    }

    /// <summary>
    /// GeoCode a list of addresses in XML format
    /// </summary>
    /// <param name="addresses">XML list of addresses</param>
    /// <<returns>GeoCoded list of Address objects</returns>
    public List<Address> GeoCodeXml(string addresses)
    {
      var list = _deserializer.DeserializeXml(addresses);
      return BulkGeoCodeChunked(list);
    }
    
    /// <summary>
    /// Return the number of addresses in a string
    /// </summary>
    /// <param name="addresses"></param>
    /// <returns></returns>
    public int CountXml(string addresses)
    {
      return _deserializer.DeserializeXml(addresses).Addresses.Count;
    }

    /// <summary>
    /// GeoCode a list of addresses in JSON format
    /// </summary>
    /// <param name="addresses">JSON list of addresses</param>
    /// <returns>GeoCoded list of Address objects</returns>
    public List<Address> GeoCodeJson(string addresses)
    {
      var list = _deserializer.DeserializeJson(addresses);
      return BulkGeoCodeChunked(list);
    }
    
    /// <summary>
    /// Return the number of addresses in a string
    /// </summary>
    /// <param name="addresses"></param>
    /// <returns></returns>
    public int CountJson(string addresses)
    {
      return _deserializer.DeserializeJson(addresses).Addresses.Count;
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
    private List<Address> BulkGeoCodeChunked(AddressList addressList)
    {
      var addressResponse = new List<Address>();
      var addressListChunked = AddressChunkGenerator.SplitAddressChunks(addressList, CHUNK_SIZE);
      
      Parallel.ForEach(addressListChunked, subList =>
      {
        var response = _apiAgent.BulkGeocode(subList);
        addressResponse.AddRange(response);
      });

      return addressResponse;
    }
  }
}