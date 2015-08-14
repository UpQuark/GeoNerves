using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoNerves
{
    class Program
    {
        static void Main(string[] args)
        {
            var bulkGeoLocator = new BulkGeoLocator();
            bulkGeoLocator.BulkGeoCodeAddressDb();
        }
    }
}