using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using GeoNerves.Models;
using Newtonsoft.Json;

namespace GeoNerves
{
  public class AddressDeserializer
  {
    public AddressList DeserializeCsv(string addresses)
    {
      var addressStrings = addresses.Split(new string[] {Environment.NewLine}, StringSplitOptions.None);
      var addressList    = new AddressList();

      addressStrings.ToList().ForEach(address => addressList.Addresses.Add(Address.ParseAddressFromCsv(address)));
      return addressList;
    }

    public AddressList DeserializeXml(string addresses)
    {
      var serializer  = new XmlSerializer(typeof(AddressList));
      var addressList = new AddressList();

      using (TextReader reader = new StringReader(addresses))
      {
        addressList = (AddressList) serializer.Deserialize(reader);
      }

      return addressList;
    }
    
    public AddressList DeserializeJson(string addresses)
    {
      return JsonConvert.DeserializeObject<AddressList>(addresses);
    }
    
  }
}