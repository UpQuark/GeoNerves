using System.Collections.Generic;
using GeoNerves.Models;

namespace GeoNerves
{
  public interface IBulkApiAgent
  {
    List<Address> BulkGeocode(List<Address> addresses, string returnType = "geographies");
  }
}