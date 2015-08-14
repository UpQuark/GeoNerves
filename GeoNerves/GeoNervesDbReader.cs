using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace GeoNerves
{
    /// <summary>
    /// DataBase reader for retrieving lists of addresses and associated info from a SQL table
    /// </summary>
    public class GeoNervesDbReader
    {
        #region properties and fields

        private const String   DefaultConnectionString = "";
        private const String   DefaultAddressTableName = "";

        private Dictionary<string, string> DefaultAddressColumns = new Dictionary<string, string> {
                                                                        {"AddressID", "[AddressID]"},
                                                                        {"Street1", "[nppes_provider_street1]"},
                                                                        {"City", "[nppes_provider_city]"},
                                                                        {"State", "[nppes_provider_state]"},
                                                                        {"Zip", "[nppes_provider_zip]"}
                                                                    };

        public String ConnectionString   { get; set; }
        public String AddressTableName   { get; set; }
        public Dictionary<string, string> AddressColumnNames { get; set; }

        #endregion


        #region constructors

        /// <summary>
        /// DbReader constructor, all parameters are optional and will revert to default if not supplied
        /// </summary>
        /// <param name="addressTableName">Custom name of source SQL address table</param>
        /// <param name="connectionString">Custom DB connection string</param>
        /// <param name="addressColumnNames">An ordinal array of column names corresponding with AddressUniqueID, StreetAddress 1, City, State, Zip Code</param>
        public GeoNervesDbReader(string addressTableName = DefaultConnectionString, string connectionString = DefaultAddressTableName, Dictionary<string, string> addressColumnNames = null)
        {
            AddressTableName = addressTableName;
            ConnectionString = connectionString;
            AddressColumnNames = addressColumnNames == null ? DefaultAddressColumns : addressColumnNames;
        }

        #endregion


        #region public methods

        /// <summary>
        /// Gets the number of rows in the SQL address table. Only returns the row count, does not look for distinct addresses
        /// </summary>
        /// <returns>Number of rows in the AddressTable</returns>
        public int GetCountFromDb()
        {
            SqlDataAdapter da = null;
            var data = new DataTable();
            var countQuery = String.Format("SELECT COUNT(*) FROM {0}", AddressTableName);
            try
            {
                SqlConnection conn = new SqlConnection(ConnectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand(countQuery, conn);
                da = new SqlDataAdapter(cmd);
                da.Fill(data);
                conn.Close();
            }
            catch (Exception e)
            {
                System.Diagnostics.EventLog.WriteEntry("Application", string.Format("db error {0}", e.Message));
                if (da != null)
                    da.Dispose();
                throw (e);
            }
            
            try 
            {
                return int.Parse(data.Rows[0].ItemArray[0].ToString());
            }
            catch (Exception e)
            {
                System.Diagnostics.EventLog.WriteEntry("Application", string.Format("Error parsing row count of AddressTable. Error: {0}", e.Message));
                return -1;
            }
        }

        /// <summary>
        ///  Gets addresses from provided DB and table whose unordered row numbers fall within the given range
        /// </summary>
        /// <param name="firstIndex">Beginning of range to retrieve (inclusive)</param>
        /// <param name="lastIndex">End of range to retrieve (inclusive)</param>
        /// <returns>A DataTable object of rows in the given range</returns>

        public DataTable GetAddressesFromDb(int firstIndex, int lastIndex)
        {
            SqlDataAdapter da = null;
            var data = new DataTable();

            var SqlQueryTemplate = @"SELECT {0}, {1}, {2}, {3}, {4}, {5} FROM (SELECT ROW_NUMBER() 
                                   OVER(ORDER BY (select NULL as noorder)) AS RowNum, * FROM {6}) as alias 
                                   WHERE RowNum BETWEEN {7} AND 8} AND lat IS NULL";

            String[] SqlQueryData = {
                                   AddressColumnNames["AddressID"],
                                   AddressColumnNames["Street1"],
                                   AddressColumnNames["City"],
                                   AddressColumnNames["State"],
                                   AddressColumnNames["Zip"],
                                   DefaultAddressTableName,
                                   firstIndex.ToString(),
                                   lastIndex.ToString()
                               };

            var addressBatchQuery = String.Format(SqlQueryTemplate, SqlQueryData);

            try
            {
                SqlConnection conn = new SqlConnection(DefaultConnectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand(addressBatchQuery, conn);
                da = new SqlDataAdapter(cmd);
                da.Fill(data);
                conn.Close();
            }
            catch (Exception e)
            {
                System.Diagnostics.EventLog.WriteEntry("Application", string.Format("db error {0}", e.Message));
                if (da != null)
                    da.Dispose();

                throw (e);
            }
            return data;
        }

        #endregion
    }
}
