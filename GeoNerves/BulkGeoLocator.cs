using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace GeoNerves
{
    public class BulkGeoLocator
    {
        public void BulkGeoCodeAddressDb()
        {
            var dbReader = new GeoNervesDbReader();
            var filePackager = new GeoNervesFilePackager();
            var apiAgent = new GeoNervesApiAgent();
            var dbWriter = new GeoNervesDbWriter();

            // Get the number of entries in the DB
            var dbTableSize = dbReader.GetCountFromDb();

            // Get the remainder. Census API accepts 1000 addresses at a time, exact remainder number prevents overflow.
            var remainder = dbTableSize % 1000;

            /*var blockset = Enumerable.Range(1, dbTableSize / 1000);
            Task.Factory.StartNew(() =>
            Parallel.ForEach(blockset, i =>
            {
                int k = i * 1000;
                Console.WriteLine(String.Format("Processing rows {0} to {1}", k.ToString(), (k + 999).ToString()));

                // Get 1000 size range of addresses
                DataTable Addresses = dbReader.GetAddressesFromDb(k, k + 999);

                // Export addresses to external CSV
                filePackager.ExportRowsToFile(Addresses);

                // Send addresses to census API and store result
                String result = apiAgent.SendAddressesToApi(filePackager.FilePath);

                // Save lat / lngs to DB
                dbWriter.WriteGeolocationToDB(result);

                // Dispose of local address file
                filePackager.DeleteExistingCsv();
                Console.WriteLine(String.Format("Finished rows {0} to {1}", k.ToString(), (k + 999).ToString()));
            });
            )
            );*/

            for (int i = 0; i < dbTableSize - remainder; i += 1000)
            {
                Console.WriteLine(String.Format("Processing rows {0} to {1}", i.ToString(), (i + 999).ToString()) + " : " + DateTime.UtcNow);

                // Get 1000 size range of addresses
                DataTable Addresses = dbReader.GetAddressesFromDb(i, i + 999);

                // Export addresses to external CSV
                filePackager.ExportRowsToFile(Addresses);

                // Send addresses to census API and store result in memory
                String result = apiAgent.SendAddressesToApi(filePackager.FilePath);

                // Save results lat / lngs to DB
                dbWriter.WriteGeolocationToDB(result);

                // Dispose of local address file
                filePackager.DeleteExistingFile();
                Console.WriteLine(String.Format("Finished rows {0} to {1}", i.ToString(), (i + 999).ToString()));
            }

            // Perform above operation for remainder
            if (remainder > 0)
            {
                int rangeStart = dbTableSize - remainder;
                int rangeEnd = dbTableSize;

                DataTable Addresses = dbReader.GetAddressesFromDb(rangeStart, rangeEnd);
                filePackager.ExportRowsToFile(Addresses);
                String result = apiAgent.SendAddressesToApi(filePackager.FilePath);
                dbWriter.WriteGeolocationToDB(result);
                filePackager.DeleteExistingFile();
            }
        }
    }
}
