using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

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

        [XmlElement("Latitude")]
        public string Latitude { get; set; }

        [XmlElement("Longitude")]
        public string Longitude { get; set; }

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
            var splitAddress = addressCsvString.Split(',');
            return new Address()
            {
                UniqueId = Convert.ToInt32(splitAddress[0]),
                Street = splitAddress[1],
                City = splitAddress[2],
                State = splitAddress[3],
                Zip = splitAddress[4],

                // Lat and Lng will generally not be expected, but are supported for parsing
                Latitude = splitAddress.Length > 5 ? splitAddress[5] : null,
                Longitude = splitAddress.Length > 6 ? splitAddress[6] : null
            };
        }

        public static Address ParseAddressFromJson(string addressJsonString)
        {
            return JsonConvert.DeserializeObject<Address>(addressJsonString);
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

        public override bool Equals(object obj)
        {
            var address = (Address) obj;

            if
            (   address != null &&
                address.UniqueId == this.UniqueId &&
                address.Street == this.Street &&
                address.City == this.City &&
                address.State == this.State &&
                address.Zip == this.Zip &&
                address.Latitude == this.Latitude &&
                address.Longitude == this.Longitude )
            {
                return true;
            }

            return false;
        }
    }
}
