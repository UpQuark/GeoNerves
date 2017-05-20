using System.Collections.Generic;
using System.Xml.Serialization;

namespace CensusAPIService.Models
{
    /// <summary>
    /// Trivial wrapper class for XML/JSON deserialization
    /// </summary>
    [XmlRoot("Addresses")]
    public class AddressList
    {
        [XmlElement("Address")]
        public List<Address> Addresses {get; set;}
    }
}
