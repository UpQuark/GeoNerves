using System.Collections.Generic;
using GeoNerves.Models;

namespace GeoNerves
{
  public interface IBulkApiAgent
  {
    List<AddressApiResponse> BulkGeocode(List<Address> addresses, string returnType = "locations");
  }
}