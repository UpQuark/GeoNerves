using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;

namespace GeoNerves
{
    /// <summary>
    /// Packager class for converting a DataTable of addresses into a CSV file on the filesystem
    /// </summary>
    public class GeoNervesFilePackager
    {
        #region public methods

        /// <summary>
        /// Path of address CSV file
        /// </summary>
        public String FilePath { get; private set; }

        /// <summary>
        /// Default constructor. Sets FilePath to "addresses.csv"
        /// </summary>
        public GeoNervesFilePackager()
        {
            FilePath = @"addresses.csv";
        }
        
        /// <summary>
        /// Custom constructor, allows supplying custom file path
        /// </summary>
        /// <param name="filePath">Relative file path of addresses CSV file</param>
        public GeoNervesFilePackager(string filePath)
        {
            this.FilePath = filePath;
        }

        /// <summary>
        /// Convert a DataTable object of addresses to CSV and save it to an external file
        /// </summary>
        /// <param name="table">Any DataTable object</param>
        public void ExportRowsToFile(DataTable table)
        {
            // Convert rows to CSV format and write to file
            var csv = ToCSV(table);
            File.WriteAllText(FilePath, csv);
        }

        /// <summary>
        /// Delete external addresses CSV file if one exists
        /// </summary>
        public void DeleteExistingFile()
        {
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
        }

        #endregion


        #region private methods

        /// <summary>
        /// Convert rows of a DataTable to a string in CSV format
        /// </summary>
        /// <param name="table">A DataTable object</param>
        /// <returns>A CSV string of the supplied DataTable's rows</returns>
        private String ToCSV(DataTable table)
        {
            var result = new StringBuilder();

            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    result.Append(row[i].ToString().Replace("#", "").Replace(",", ""));
                    result.Append(i == table.Columns.Count - 1 ? "\n" : ",");
                }
            }
            return result.ToString();
        }

        #endregion
    }
}
