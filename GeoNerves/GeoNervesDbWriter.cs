using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace GeoNerves
{
    public class GeoNervesDbWriter
    {
        private const String defaultConnectionString = @"Data Source=MyComputer\SQLEXPRESS;Database=AddressDB;User Id=myUser;Password=Password;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;"
        private const String defaultAddressTableName = "";

        public String WriteGeolocationToDB(string csv)
        {
            DataTable dt = CsvToDataTable(csv);

            SqlDataAdapter da = null;
            String connString = defaultConnectionString;

            foreach (DataRow drow in dt.Rows)
            {
                string AddressID = (string)drow[0];
                Boolean Match = (string)drow[2] == "Match" ? true : false;

                if (Match)
                {
                    Double lat = Double.Parse((string)drow[5]);
                    Double lng = Double.Parse((string)drow[6]);

                    try
                    {
                        using (SqlConnection con = new SqlConnection(connString))
                        {
                            using (SqlCommand cmd = new SqlCommand("spUpdateAddressesWithGeocode", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@lng", SqlDbType.Float).Value = lng;
                                cmd.Parameters.Add("@lat", SqlDbType.Float).Value = lat;
                                cmd.Parameters.Add("@AddressID", SqlDbType.VarChar).Value = AddressID;

                                con.Open();
                                cmd.ExecuteNonQuery();
                                //da = new SqlDataAdapter(cmd);
                                // Query DB and fill DataTable
                                //da.Fill(data);
                                con.Close();
                            }
                        }
                    }
                    catch (Exception exp)
                    {
                        System.Diagnostics.EventLog.WriteEntry("Application", string.Format("Error writing inbound data to DB  {0}", exp.Message));

                        if (da != null)
                            da.Dispose();

                        throw (exp);
                    }
                }
            }
            return "";
        }




        private DataTable CsvToDataTable(string s)
        {
            //var dt = new DataRow[];
            DataTable dt = new DataTable();

            dt.Columns.Add("addressid");
            dt.Columns.Add("address");
            dt.Columns.Add("match");
            dt.Columns.Add("matchtype");
            dt.Columns.Add("correctedAddress");
            dt.Columns.Add("lat");
            dt.Columns.Add("lng");
            dt.Columns.Add("zip");
            dt.Columns.Add("side");

            try
            {
                string[] tableData = s.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string row in tableData)
                {
                    DataRow drow = dt.NewRow();
                    var stringRow = row.Split(new string[] { "\",\"" }, StringSplitOptions.None);
                    drow["addressid"] = stringRow[0].Replace("\"", "");// != null ? stringRow[0] : null;
                    drow["address"] = stringRow[1] != null ? stringRow[1] : null; ;
                    drow["match"] = stringRow[2] != null ? stringRow[2] : null; ;
                    if (stringRow.Length > 3)
                    {
                        drow["matchtype"] = stringRow[3] != null ? stringRow[3] : null;
                        drow["correctedAddress"] = stringRow[4] != null ? stringRow[4] : null;
                        var latLng = stringRow[5].Split(',');
                        drow["lat"] = latLng[0] != null ? latLng[0] : null;
                        drow["lng"] = latLng[1] != null ? latLng[1] : null;
                        drow["zip"] = stringRow[6] != null ? stringRow[6] : null;
                        drow["side"] = stringRow[7] != null ? stringRow[7] : null;
                    }

                    dt.Rows.Add(drow);
                }
            }
            catch (Exception exp)
            {
                System.Diagnostics.EventLog.WriteEntry("Application", string.Format("Error writing inbound data to DataTable {0}", exp.Message));

                if (dt != null)
                    dt.Dispose();

                throw (exp);
            }



            return dt;
        }
    }
}
