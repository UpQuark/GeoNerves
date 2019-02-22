using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace GeoNerves.Models
{
    /// <summary>
    /// A U.S. address 
    /// </summary>
    [XmlRoot("Address")]
    public class Address
    {
        #region Properties

        [XmlElement("UniqueId")]
        public int? UniqueId { get; set; }

        [XmlElement("Street")]
        public string Street { get; set; }

        [XmlElement("City")]
        public string City { get; set; }

        [XmlElement("State")]
        public string State { get; set; }

        [XmlElement("Zip")]
        public string Zip { get; set; }

        [XmlElement("Latitude")]
        public double? Latitude { get; set; }

        [XmlElement("Longitude")]
        public double? Longitude { get; set; }

        #endregion

        #region Factory Methods

        /// <summary>
        /// Generate an address object from a census-format CSV string
        /// </summary>
        /// <param name="addressCsvString">
        /// Address string with the following format:  with the following format:
        /// "UniqueId,StreetAddress,City,State,Zip,Latitude,Longitude"
        /// </param>
        public static Address ParseAddressFromCsv(string addressCsvString)
        {
            var splitAddress = addressCsvString.Split(',');
            var splitAddressTrimmed = new List<string>();

            splitAddress.ToList().ForEach(address => splitAddressTrimmed.Add(address.Trim()));

            return new Address()
            {
                UniqueId = Convert.ToInt32(splitAddressTrimmed[0]),
                Street = splitAddressTrimmed[1],
                City = splitAddressTrimmed[2],
                State = splitAddressTrimmed[3],
                Zip = splitAddressTrimmed[4]
            };
        }

        public static Address ParseAddressFromCsv(string addressCsvString, int uniqueId)
        {
            // Trim extra spaces

            var splitAddress = addressCsvString.Split(',');
            var splitAddressTrimmed = new List<string>();

            splitAddress.ToList().ForEach(address => splitAddressTrimmed.Add(address.Trim()));

            return new Address()
            {
                UniqueId = uniqueId,
                Street = splitAddressTrimmed[0],
                City = splitAddressTrimmed[1],
                State = splitAddressTrimmed[2],
                Zip = splitAddressTrimmed[3]
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
                return (Address)serializer.Deserialize(reader);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Equality override to compare values rather than reference
        /// </summary>
        /// <param name="obj">Obj to compare equality against</param>
        /// <returns>Bool of whether values are equal</returns>
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

        /// <summary>
        /// Get hash code based on values of address
        /// </summary>
        /// <returns>Hash code for object</returns>
        public override int GetHashCode()
        {
            // TODO: Refactor to use answer from http://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode
            return ((UniqueId == null ? 0 : UniqueId.GetHashCode()) ^
                    (Street == null ? 0 : Street.GetHashCode()) ^
                    (City == null ? 0 : City.GetHashCode()) ^
                    (State == null ? 0 : State.GetHashCode()) ^
                    (Zip == null ? 0 : Zip.GetHashCode()) ^
                    (Latitude == null ? 0 : Latitude.GetHashCode()) ^
                    (Longitude == null ? 0 : Longitude.GetHashCode()));
        }

        /// <summary>
        /// Output as a CSV string consumable by the Census API
        /// </summary>
        /// <returns>CSV representation of address</returns>
        public string ToCsv()
        {
            return $"{UniqueId},{Street},{City},{State},{Zip}\n";
        }

        #endregion
    }
}
