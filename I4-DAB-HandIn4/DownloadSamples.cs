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

        private static readonly string connString = "data source=10.29.0.29;initial catalog=F17I4DABH4Gr4;persist security info=True;user id=F17I4DABH4Gr4;password=F17I4DABH4Gr4;MultipleActiveResultSets=True;App=EntityFramework";



            //"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=I4DABHandIn4.SensorContext;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        [SqlProcedure]
        public List<SampleModel> GetSamplesForFlat(SqlString time1, SqlString time2, SqlInt64 number, SqlInt64 floor )
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmdAppartment = new SqlCommand();
                
                SqlParameter numberParam = new SqlParameter("@number", SqlDbType.BigInt);
                SqlParameter floorParam = new SqlParameter("@floor", SqlDbType.BigInt);

                cmdAppartment.Parameters.Add(numberParam);
                cmdAppartment.Parameters.Add(floorParam);

                numberParam.Value = number;
                floorParam.Value = floor;

                cmdAppartment.CommandText =
                    "SELECT  ApartmentCharacteristicsId FROM  dbo.ApartmentCharacteristics" +
                    " WHERE (No = @number AND Floor = @floor)";

                cmdAppartment.Connection = conn;
                SqlDataReader rdr = null;
                int appartmentId = -1;
                try
                {
                    conn.Open();

                    // Send command
                    rdr = cmdAppartment.ExecuteReader();

                    while (rdr.Read())
                        appartmentId = Convert.ToInt32(rdr["ApartmentCharacteristicsId"]);
                }
                finally
                {
                    // Close connection
                    rdr?.Close();
                    conn?.Close();
                }

                if (appartmentId == -1)
                    return null;

                SqlCommand cmdSample = new SqlCommand();

                SqlParameter time1Param = new SqlParameter("@time1", SqlDbType.NVarChar);
                SqlParameter time2Param = new SqlParameter("@time2", SqlDbType.NVarChar);
                SqlParameter idParam = new SqlParameter("@id", SqlDbType.BigInt);

                time1Param.Value = time1;
                time2Param.Value = time2;
                idParam.Value = appartmentId;

                cmdSample.Parameters.Add(time1Param);
                cmdSample.Parameters.Add(time2Param);
                cmdSample.Parameters.Add(idParam);

                cmdSample.CommandText =
                    "SELECT  Value, Timestamp,  Description, Unit  FROM  dbo.Samples" +
                    " INNER JOIN dbo.SensorCharacteristics ON SensorId = SensorCharacteristicsId" +
                    " WHERE (AppartmentId = @id  AND Timestamp >= @time1 AND Timestamp <= @time2)";


                cmdSample.Connection = conn;
                rdr = null;
                var list = new List<SampleModel>();
                try
                {
                    conn.Open();

                    // Send command
                    rdr = cmdSample.ExecuteReader();

                    while (rdr.Read())
                    {
                        var person = new SampleModel()
                        {
                            Value = Convert.ToInt32(rdr["Value"]),
                            Timestamp = rdr["Timestamp"].ToString(),
                            Description = rdr["Description"].ToString(),
                            Unit = rdr["Unit"].ToString()
                        };

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
