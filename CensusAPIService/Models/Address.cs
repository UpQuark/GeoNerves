using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CensusAPIService.Models
{
    [XmlRoot("Address")]
    public class Address
    {
        #region Properties

        [XmlElement("UniqueId")]
        public int UniqueId { get; set; }

        [XmlElement("Street")]
        public string Street { get; set; }

        [XmlElement("City")]
        public string City { get; set; }

        [XmlElement("State")]
        public string State { get; set; }

        [XmlElement("Zip")]
        public string Zip { get; set; }

        [XmlElement("Coordinates")]
        public Coordinates Coordinates { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor for empty address
        /// </summary>
        public Address()
        {

        }

        #endregion

        #region Public Factory Methods

        /// <summary>
        /// Generate an address object from a census-format CSV string
        /// </summary>
        /// <param name="addressCsvString">
        /// Address string with the following format:  with the following format:
        /// "UniqueId,StreetAddress,City,State,Zip"
        /// e.g. "1,667 Massachusetts Avenue,Cambridge,MA,02139"
        /// </param>
        public static Address ParseAddressFromCsvString(string addressCsvString)
        {
            return null;
        }

        public static Address ParseAddressFromJson()
        {
            return null;
        }

        public static Address ParseAddressFromXml(string addressXmlString)
        {
            var serializer = new XmlSerializer(typeof(Address));

            using (TextReader reader = new StringReader(addressXmlString))
            {
                return (Address)serializer.Deserialize(reader);
            }
        }

        #endregion
    }
}
