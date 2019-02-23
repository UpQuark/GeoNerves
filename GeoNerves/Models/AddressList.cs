using System.Collections.Generic;
using System.Xml.Serialization;

namespace GeoNerves.Models
{
  /// <summary>
  /// Trivial wrapper class for XML/JSON deserialization
  /// </summary>
  [XmlRoot("Addresses")]
  public class AddressList
  {
    public AddressList()
    {
      Addresses = new List<Address>();
    }

    [XmlElement("Address")] public List<Address> Addresses { get; set; }
  }
}