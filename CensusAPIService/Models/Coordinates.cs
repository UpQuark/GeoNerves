using System.Xml.Serialization;

namespace CensusAPIService.Models
{
    [XmlRoot("Coordinates")]
    public class Coordinates
    {
        [XmlAttribute("Latitude")]
        public double Latitude { get; set; }

        [XmlAttribute("Longitude")]
        public double Longitude { get; set; }
    }
}
