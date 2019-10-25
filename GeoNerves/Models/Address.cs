using System.Xml.Serialization;

namespace GeoNerves.Models
{
  /// <summary>
  /// A U.S. address 
  /// </summary>
  [XmlRoot("Address")]
  public class Address
  {
    [XmlElement("Id")]                public int?    Id                { get; set; }
    [XmlElement("Street")]            public string  Street            { get; set; }
    [XmlElement("City")]              public string  City              { get; set; }
    [XmlElement("State")]             public string  State             { get; set; }
    [XmlElement("Zip")]               public string  Zip               { get; set; }
    [XmlElement("Latitude")]          public double? Latitude          { get; set; }
    [XmlElement("Longitude")]         public double? Longitude         { get; set; }
    [XmlElement("Match")]             public bool?   Match             { get; set; }
    [XmlElement("MatchType")]         public string  MatchType         { get; set; }
    [XmlElement("NormalizedAddress")] public string  NormalizedAddress { get; set; }
    [XmlElement("TigerLineId")]       public long    TigerLineId       { get; set; }
    [XmlElement("TigerLineSide")]     public string  TigerLineSide     { get; set; }
    [XmlElement("StateId")]           public int     StateId           { get; set; }
    [XmlElement("CountyId")]          public int     CountyId          { get; set; }
    [XmlElement("CensusTractId")]     public int     CensusTractId     { get; set; }
    [XmlElement("BlockId")]           public int     BlockId           { get; set; }

    public string GeoId { get; set; }


    /// <summary>
    /// Equality override to compare values rather than reference
    /// </summary>
    /// <param name="obj">Obj to compare equality against</param>
    /// <returns>Bool of whether values are equal</returns>
    public override bool Equals(object obj)
    {
      var address = (Address) obj;

      if
      (address != null &&
       address.Id == Id &&
       address.Street == Street &&
       address.City == City &&
       address.State == State &&
       address.Zip == Zip &&
       address.Latitude == Latitude &&
       address.Longitude == Longitude)
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
      return (Id == null ? 0 : Id.GetHashCode()) ^
             (Street == null ? 0 : Street.GetHashCode()) ^
             (City == null ? 0 : City.GetHashCode()) ^
             (State == null ? 0 : State.GetHashCode()) ^
             (Zip == null ? 0 : Zip.GetHashCode()) ^
             (Latitude == null ? 0 : Latitude.GetHashCode()) ^
             (Longitude == null ? 0 : Longitude.GetHashCode());
    }

    /// <summary>
    /// Output as a CSV string consumable by the Census API
    /// </summary>
    /// <returns>CSV representation of address</returns>
    public string ToCsv()
    {
      return $"{Id},{Street},{City},{State},{Zip}\n";
    }
  }
}