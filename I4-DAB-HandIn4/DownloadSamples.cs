using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace I4DABHandIn4
{
    class DownloadSamples
    {

        private static readonly string connString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=I4DABHandIn4.SensorContext;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        [SqlProcedure]
        public static List<SampleModel> GetSamplesForFlat(SqlString time1, SqlString time2, SqlInt32 number, SqlInt32 floor )
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmdAppartment = new SqlCommand();
                
                SqlParameter numberParam = new SqlParameter("@number", SqlDbType.NVarChar);
                SqlParameter floorParam = new SqlParameter("@floor", SqlDbType.NVarChar);

                cmdAppartment.Parameters.Add(numberParam);
                cmdAppartment.Parameters.Add(floorParam);
                numberParam.Value = number;
                floorParam.Value = floor;

                SqlCommand cmdSample = new SqlCommand();

                SqlParameter time1Param = new SqlParameter("@time1", SqlDbType.NVarChar);
                SqlParameter time2Param = new SqlParameter("@time2", SqlDbType.NVarChar);

                time1Param.Value = time1;
                time2Param.Value = time2;

                cmdSample.Parameters.Add(time1Param);
                cmdSample.Parameters.Add(time2Param);


                cmdSample.CommandText =
                    "INSERT Sales.Currency (CurrencyCode, Name, ModifiedDate)" +
                    " VALUES(@CurrencyCode, @Name, GetDate())";

                cmdSample.Connection = conn;
                SqlDataReader rdr = null;
                var list = new List<SampleModel>();
                try
                {
                    conn.Open();

                    // Send command
                    rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        var person = new SampleModel() { Value = Convert.ToInt32(rdr["Value"]), Timestamp = rdr["Timestamp"].ToString()};

                        list.Add(person);
                    }
                }
                finally
                {
                    // Close connection
                    rdr?.Close();
                    conn?.Close();
                }

                return list;
            }
        }
    }

}
