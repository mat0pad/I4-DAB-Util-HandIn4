using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
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

        private static readonly string connString = "Data Source=10.29.0.29;Initial Catalog=F17I4DABH4Gr4;Persist Security Info=True;User ID=F17I4DABH4Gr4;Password=F17I4DABH4Gr4;Connect Timeout=30;";


        //"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=I4DABHandIn4.SensorContext;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        [SqlProcedure]
        public List<SampleModel> GetSamplesForFlat(SqlString time1, SqlString time2, SqlInt64 number, SqlInt64 floor)
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

        public void CreateTrigger()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    var cmdStr = "CREATE TRIGGER manipulationsLogger " +
                                 "ON dbo.Samples " +
                                 "FOR UPDATE, INSERT, DELETE " +
                                 "AS " +
                                 "DECLARE @LogString1 VARCHAR(100) " +
                                 "DECLARE @LogString2 VARCHAR(100) " +
                                 "" +
                                 "IF EXISTS(SELECT * FROM Inserted) " +
                                 "  IF EXISTS(SELECT * FROM Deleted) " +
                                 "  BEGIN " +
                                 "    SELECT @LogString1 = (SELECT CONVERT(nvarchar, SensorId) + ' ' + CONVERT(nvarchar, AppartmentId) + ' ' + CONVERT(nvarchar, Value) + ' ' + Timestamp + ' ' + CONVERT(nvarchar, SampleCollectionId) FROM " +
                                 "    Deleted)" +
                                 "" +
                                 "    SELECT @LogString2 = (SELECT CONVERT(nvarchar, SensorId) + ' ' + CONVERT(nvarchar, AppartmentId) + ' ' + CONVERT(nvarchar, Value) + ' ' + Timestamp + ' ' + CONVERT(nvarchar, SampleCollectionId) FROM " +
                                 "    Inserted) " +
                                 "    INSERT INTO dbo.LogManipulations(Operation, LogEntry1, LogEntry2) VALUES " +
                                 "    ('Updated', @LogString1, @LogString2); " +
                                 "   END " +
                                 "" +
                                 "ELSE " +
                                 "  IF EXISTS(SELECT * FROM Deleted) " +
                                 "  BEGIN " +
                                 "    SELECT @LogString1 = (SELECT CONVERT(nvarchar, SensorId) + ' ' + CONVERT(nvarchar, AppartmentId) + ' ' + CONVERT(nvarchar, Value) + ' ' + Timestamp + ' ' + CONVERT(nvarchar, SampleCollectionId) FROM " +
                                 "    Deleted) " +
                                 "" +
                                 "    INSERT INTO dbo.LogManipulations(Operation, LogEntry1) VALUES " +
                                 "    ('Deleted', @LogString1); " +
                                 "  END " +
                                 "ELSE " +
                                 "   BEGIN " +
                                 "    SELECT @LogString1 = (SELECT CONVERT(nvarchar, SensorId) + ' ' + CONVERT(nvarchar, AppartmentId) + ' ' + CONVERT(nvarchar, Value) + ' ' + Timestamp + ' ' + CONVERT(nvarchar, SampleCollectionId) FROM " +
                                 "    Inserted) " +
                                 "" +
                                 "    INSERT INTO dbo.LogManipulations(Operation, LogEntry1) VALUES " +
                                 "    ('Inserted', @LogString1); " +
                                 "   END";

                    new SqlCommand(cmdStr, conn).ExecuteNonQuery();
                }
                finally
                {
                    conn.Close();
                }
                
            }

        }

        [SqlProcedure]
        public void AddSamplesForFlat(List<Sample> samples)
        {
            using (var conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    foreach (var sample in samples)
                    {
                        SqlCommand command = new SqlCommand("INSERT INTO dbo.Samples VALUES (@sensorId, @appartmentId, @value, @timeStamp, @sampleCollectionId)", conn);

                        SqlParameter id = new SqlParameter("@sensorId", SqlDbType.Int);
                        SqlParameter aId = new SqlParameter("@appartmentId", SqlDbType.Int);
                        SqlParameter value = new SqlParameter("@value", SqlDbType.Float);
                        SqlParameter timeStamp = new SqlParameter("@timeStamp", SqlDbType.NVarChar);
                        SqlParameter sampleCollectionId = new SqlParameter("@sampleCollectionId", SqlDbType.Int);

                        id.Value = sample.SensorId;
                        aId.Value = sample.AppartmentId;
                        value.Value = sample.Value;
                        timeStamp.Value = sample.Timestamp;
                        sampleCollectionId.Value = sample.SampleCollectionId;

                        command.Parameters.Add(id);
                        command.Parameters.Add(aId);
                        command.Parameters.Add(value);
                        command.Parameters.Add(timeStamp);
                        command.Parameters.Add(sampleCollectionId);

                        command.ExecuteNonQuery();
                    }
                }
                finally
                {
                    // Close connection
                    conn.Close();
                }
            }
        }


        public List<SampleModel> GetSampleThroughProcedure(SqlString time1, SqlString time2, SqlInt64 id)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (var command = new SqlCommand("GetInterval", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Connection = conn;

                    SqlDataReader rdr = null;

                    SqlParameter time1Param = new SqlParameter("@time1", SqlDbType.NVarChar);
                    SqlParameter time2Param = new SqlParameter("@time2", SqlDbType.NVarChar);
                    SqlParameter idParam = new SqlParameter("@id", SqlDbType.BigInt);

                    time1Param.Value = time1;
                    time2Param.Value = time2;
                    idParam.Value = id;

                    command.Parameters.Add(time1Param);
                    command.Parameters.Add(time2Param);
                    command.Parameters.Add(idParam);

                    var list = new List<SampleModel>();
                    try
                    {
                        conn.Open();
                        // Send command
                        rdr = command.ExecuteReader();

                        while (rdr.Read())
                        {
                            var model = new SampleModel()
                            {
                                Value = Convert.ToInt32(rdr["Value"]),
                                Timestamp = rdr["Timestamp"].ToString(),
                                Description = rdr["Description"].ToString(),
                                Unit = rdr["Unit"].ToString()
                            };

                            list.Add(model);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
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
        public void AddSamplesForFlat(Samples samples)
        {
            // construct sql connection and sql command objects.
            using (SqlConnection sqlcon = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("AddSamplesForFlat", sqlcon))
                {
                    // add the table-valued-parameter. 
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Items", SqlDbType.Structured).Value = samples.GetItemsAsDataTable();

                    // execute
                    sqlcon.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

    public class Samples : List<Sample>
    {
        /// <summary>
        /// Returns the samples as a DataTable.
        /// </summary>
        /// <returns><c>DataTable</c></returns>
        public DataTable GetItemsAsDataTable()
        {
            // construct the empty DataTable object with columns.
            DataTable table = new DataTable();
            table.Columns.Add("timeStamp", typeof(string));
            table.Columns.Add("appartmentId", typeof(int));
            table.Columns.Add("value", typeof(float));
            table.Columns.Add("sensorId", typeof(int));
            table.Columns.Add("sampleCollectionId", typeof(int));

            // add a single row for each item in the collection.
            foreach (var item in this)
            {
                table.Rows.Add(item.Timestamp, item.AppartmentId, item.Value, item.SensorId, item.SampleCollectionId);
            }

            return table;
        }
    }
}
