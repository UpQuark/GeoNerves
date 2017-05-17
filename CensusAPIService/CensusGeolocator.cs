using CensusAPIService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace CensusAPIService
{
    public class CensusGeolocator
    {
        public IEnumerable<Address> GeoCode(List<string> addresses)
        {
            return null;
        }

        public IEnumerable<Address> GeoCode(List<Address> addresses)
        {
            return null;
        }

        public IEnumerable<Address> GeoCode(FileStream addresses)
        {
            return null;
        }
    }
}
