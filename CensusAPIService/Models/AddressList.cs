using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CensusAPIService.Models
{
    [XmlRoot("Addresses")]
    public class AddressList
    {
        [XmlElement("Address")]
        public List<Address> Addresses {get; set;}
    }
}
