using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using GeoNerves.Models;
using Newtonsoft.Json;

namespace GeoNerves
{
  public class AddressParser
  {
    /// <summary>
    /// Generate an address object from a census-format CSV string
    /// </summary>
    /// <param name="addressCsvString">
    /// Address string with the following format:  with the following format:
    /// "UniqueId,StreetAddress,City,State,Zip,Latitude,Longitude"
    /// </param>
    public static Address ParseAddressFromCsv(string addressCsvString)
    {
      var splitAddress        = addressCsvString.Split(',');
      var splitAddressTrimmed = new List<string>();

      splitAddress.ToList().ForEach(address => splitAddressTrimmed.Add(address.Trim()));

      return new Address
      {
        Id = Convert.ToInt32(splitAddressTrimmed[0]),
        Street   = splitAddressTrimmed[1],
        City     = splitAddressTrimmed[2],
        State    = splitAddressTrimmed[3],
        Zip      = splitAddressTrimmed[4]
      };
    }

    public static Address ParseAddressFromCsv(string addressCsvString, int uniqueId)
    {
      // Trim extra spaces

      var splitAddress        = addressCsvString.Split(',');
      var splitAddressTrimmed = new List<string>();

      splitAddress.ToList().ForEach(address => splitAddressTrimmed.Add(address.Trim()));

      return new Address
      {
        Id = uniqueId,
        Street   = splitAddressTrimmed[0],
        City     = splitAddressTrimmed[1],
        State    = splitAddressTrimmed[2],
        Zip      = splitAddressTrimmed[3]
      };
    }

    /// <summary>
    /// Generate an address object from a JSON string
    /// </summary>
    /// <param name="addressJsonString">JSON address</param>
    /// <returns></returns>
    public static Address ParseAddressFromJson(string addressJsonString)
    {
      return JsonConvert.DeserializeObject<Address>(addressJsonString);
    }

    /// <summary>
    /// Generate an address object from an XML string
    /// </summary>
    /// <param name="addressXmlString">address as XML</param>
    /// <returns></returns>
    public static Address ParseAddressFromXml(string addressXmlString)
    {
      var serializer = new XmlSerializer(typeof(Address));

      using (TextReader reader = new StringReader(addressXmlString))
      {
        return (Address) serializer.Deserialize(reader);
      }
    }
  }
}