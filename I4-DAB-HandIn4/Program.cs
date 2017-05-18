﻿using System;
using System.Collections.Generic;
 using System.Data.Entity.Infrastructure;
 using System.IO;
 using System.Linq;
 using System.Net;
using Newtonsoft.Json;
using System.Threading;

namespace I4DABHandIn4
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            // Load static data into db
            // ----------------------------------
            // Load appartment & sensor information
            //LoadAllCharacteristicsFromSite();

            // Setup many to many
            //LoadAppartment2SensorFromSite();
            // ----------------------------------

            // Simulate dynamic data that is
            // saved into db
            // ----------------------------------
            /*for (int i = 1; i < 11801; i++)
            {  
                Console.WriteLine("Loading new sample");
                LoadSampleFromSite(i);
                Thread.Sleep(5000);
            }*/
            // ----------------------------------


            // Stored procedure download list
            // of samples between time
            //-----------------------------------

            /*Console.Write("Search will find entries between 2014-10-08T07:50:15 AND 2014-11-08T07:57:15\n > Enter appartment number: ");

            var no = Convert.ToInt32(Console.ReadLine());

            Console.Write(" > Enter floor: ");

            var floor = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("\n\nSearching...");

            var downloadSample = new DownloadSamples();
            // Burde kaldes fra SQL, da det er en stored procedure
            var list = downloadSample.GetSamplesForFlat("2014-10-08T07:50:15", "2014-11-08T07:57:15", no, floor);

            Console.WriteLine("Found in interval from flat\n----------------------");
            foreach (var item in list)
            {
                Console.WriteLine("Timestamp: {0},    Description: {1},   Value: {2},    Unit: {3}", item.Timestamp, item.Description, item.Value, item.Unit);
            }
            Console.WriteLine("-----------------------");*/
            // ----------------------------------


            // Sæt trigger og tilføj et nyt sample til databasen
            // Kræver at der findes en SensorCollection med PK på 1
            // -----------------------------------------------
            /*using (var db = new SensorContext())
            {
                DownloadSamples ds = new DownloadSamples();

                //ds.CreateTrigger();


                var samples = new List<Sample>();

                samples.Add(new Sample()
                {
                    SensorId = 1,
                    AppartmentId = 1,
                    SampleCollectionId = 1,
                    Timestamp = "testTimeSample",
                    Value = 10
                });

                // Burde kaldes fra SQL, da det er en stored procedure
                ds.AddSamplesForFlat(samples);
            }*/




            // Vis SQL for EF operation
            // -----------------------
            /*
            using (var db = new SensorContext())
            {
                // Query for all blogs with names starting with B 
                var sample = from s in db.Samples
                            where s.Timestamp.StartsWith("2")
                            select s;

                Console.WriteLine(sample);

            }*/
        }

        // Load sample from website
        public static void LoadSampleFromSite(int i)
        {
            using (var httpClient = new WebClient())
            {
                var json = httpClient.DownloadString("http://userportal.iha.dk/~jrt/i4dab/E14/HandIn4/dataGDL/data/" + i + ".json");

                ReadingRootObject root = JsonConvert.DeserializeObject<ReadingRootObject>(json);

                using (var db = new SensorContext())
                {
                    var samples = new List<Sample>(root.Reading.Count);

                    foreach (var item in root.Reading)
                    {
                        samples.Add(new Sample()
                        {
                            Timestamp = item.Timestamp,
                            AppartmentId = item.AppartmentId,
                            SensorId = item.SensorId,
                            Value = item.Value,
                            SampleCollectionId = i
                        });
                    }

                    // Create and save a sample collection 
                    Console.WriteLine("Id: " + i + ", Time: " + root.Timestamp + ", Version: " + root.Version);

                    var collection = new SampleCollection() { Id = i, Version = root.Version, Timestamp = root.Timestamp, Samples = samples};
                    db.SampleCollections.Add(collection);

                    try
                    {
                        db.SaveChanges();

                        //Console.WriteLine("--- Saved {0} ----\n", root.Timestamp);
                    }
                    catch (DbUpdateException e)
                    {
                        //Console.WriteLine("---- {0} Already there ----\n", root.Timestamp);
                    }
                }
            }
        }

        // Load characteristics from website
        public static void LoadAllCharacteristicsFromSite()
        {
            using (var httpClient = new WebClient())
            {
                var json = httpClient.DownloadString("http://userportal.iha.dk/~jrt/i4dab/E14/HandIn4/GFKSC002_original.txt");

                CharacteristicRootObject root = JsonConvert.DeserializeObject<CharacteristicRootObject>(json);

                Console.WriteLine("Time:" + root.Timestamp + " Version: " + root.Version);

                // Sensor part
                using (var db = new SensorContext())
                {
                    Console.WriteLine("SensorCharacteristic length - " + root.SensorCharacteristic.Count);

                    Thread.Sleep(3000);

                    var list = root.SensorCharacteristic;

                    for (int i = 0; i < list.Count; i++)
                    {
                        // Create and save a item
                        db.Sensors.Add(new SensorCharacteristics()
                        {
                            SensorCharacteristicsId = list[i].SensorId,
                            CalibrationCoeff = (string.IsNullOrWhiteSpace(list[i].CalibrationCoeff) ? null : list[i].CalibrationCoeff),
                            CalibrationEquation = (string.IsNullOrWhiteSpace(list[i].CalibrationEquation) ? null : list[i].CalibrationEquation),
                            CalibrationDate = list[i].CalibrationDate,
                            Description = list[i].Description,
                            ExternalRef = (string.IsNullOrWhiteSpace(list[i].ExternalRef) ? null : list[i].ExternalRef),
                            Unit = list[i].Unit
                        });

                        Console.WriteLine("Adding item - " + i);

                        try
                        {
                            db.SaveChanges();

                            Console.WriteLine("--- Saved {0} ----", i);
                        }
                        catch (DbUpdateException e)
                        {
                            Console.WriteLine("---- {0} Already there ----", i);
                        }

                    }

                    Console.WriteLine("--- Saving done for SensorCharacteristic ----");


                    // Appartment
                    Console.WriteLine("AppartmentCharacteristic length - " + root.AppartmentCharacteristic.Count);

                    Thread.Sleep(3000);

                    var list2 = root.AppartmentCharacteristic;

                    for (int i = 0; i < list2.Count; i++)
                    {
                        // Create and save a item
                        db.Apartments.Add(new ApartmentCharacteristics()
                        {
                            ApartmentCharacteristicsId = list2[i].AppartmentId,
                            Floor = list2[i].Floor,
                            Size = list2[i].Size,
                            No = list2[i].No
                        });

                        Console.WriteLine("Adding item - " + i);

                        try
                        {
                            db.SaveChanges();

                            Console.WriteLine("--- Saved {0} ----", i);
                        }
                        catch (DbUpdateException e)
                        {
                            Console.WriteLine("---- {0} Already there ----", i);
                        }

                    }

                    Console.WriteLine("--- Saving done for AppartmentCharacteristic ----");
                }
            }
        }

        // Load characteristics from website
        public static void LoadSensorInfoFromSite()
        {
            using (var httpClient = new WebClient())
            {

                var csv =
                    httpClient.DownloadString("http://userportal.iha.dk/~jrt/i4dab/E14/HandIn4/sensors_information.txt");

                var list = new List<SensorInfoObject>();

                string[] tempStr = csv.Split('\n');

                for (int i = 0; i < tempStr.Length; i++)
                {
                    if (i != 0 && !string.IsNullOrWhiteSpace(tempStr[i]))
                    {
                        Console.WriteLine(tempStr[i]);

                        string[] temp = tempStr[i].Split(';');

                        list.Add(new SensorInfoObject()
                        {
                            Description = temp[2],
                            Timestamp = temp[1],
                            SensorID = int.Parse(temp[0]),
                            Units = temp[3]
                        });
                    }
                }


                
            }
        }

        // Load sensor2appartment from website
        public static void LoadAppartment2SensorFromSite()
        {
            using (var httpClient = new WebClient())
            {

                var csv = httpClient.DownloadString("http://userportal.iha.dk/~jrt/i4dab/E14/HandIn4/apartment-to-sensor_NotJSONCorrupt.txt");

                var list = new List<Apartment2SensorObject>();

                string[] tempStr = csv.Split('\n');

                for (int i = 0; i < tempStr.Length; i++)
                {
                    if (i != 0 && !string.IsNullOrWhiteSpace(tempStr[i]))
                    {
                        Console.WriteLine(tempStr[i]);

                        string[] temp = tempStr[i].Split(';');

                        list.Add(new Apartment2SensorObject()
                        {
                            ApartmentID = (temp[1] != "\r" ? int.Parse(temp[1]) : 0),
                            SensorID = int.Parse(temp[0]),
                        });
                    }
                }

                using (var db = new SensorContext())
                {
                    Console.WriteLine("List length - " + list.Count);

                     Thread.Sleep(3000);

                     for (int i = 0; i < list.Count; i++)
                     {
                        var sensor = db.Sensors.Find(list[i].SensorID);

                        var appartment = db.Apartments.Find(list[i].ApartmentID);

                         try
                         {
                            appartment.SensorCharacteristics.Add(sensor);
                            sensor.ApartmentCharacteristics.Add(appartment);
                         }
                        catch (Exception e) { continue; }    

                         Console.WriteLine("Adding item - " + i);

                         try
                         {
                             db.SaveChanges();

                            Console.WriteLine("--- Saved {0} ----", i);
                        }
                         catch (DbUpdateException e)
                         {
                            Console.WriteLine("---- {0} Already there ----", i);
                        }
                        
                    }

                    Console.WriteLine("--- Saving Done ----");
                }
            }
        }
    }


    //Sample 
    public class Reading
    {
        public int SensorId { get; set; }
        public int AppartmentId { get; set; }
        public double Value { get; set; }
        public string Timestamp { get; set; }
    }

    public class ReadingRootObject
    {
        public string Timestamp { get; set; }
        public List<Reading> Reading { get; set; }
        public int Version { get; set; }
    }

    //Characteristics 
    public class AppartmentCharacteristic
    {
        public int No { get; set; }
        public double Size { get; set; }
        public int Floor { get; set; }
        public int AppartmentId { get; set; }
    }

    public class SensorCharacteristic
    {
        public string CalibrationCoeff { get; set; }
        public string Description { get; set; }
        public string CalibrationDate { get; set; }
        public string ExternalRef { get; set; }
        public int SensorId { get; set; }
        public string Unit { get; set; }
        public string CalibrationEquation { get; set; }
    }

    public class CharacteristicRootObject
    {
        public List<AppartmentCharacteristic> AppartmentCharacteristic { get; set; }
        public string Timestamp { get; set; }
        public int Version { get; set; }
        public List<SensorCharacteristic> SensorCharacteristic { get; set; }
    }

	//SensorInfo
	public class SensorInfoObject
    {
        public string Timestamp { get; set; }
        public string Description { get; set; }
        public string Units { get; set; }
        public int SensorID { get; set; }
    }

	// Apartment2Sensor
	public class Apartment2SensorObject
    {

        public int SensorID { get; set; }
        public int ApartmentID { get; set; }
    }
}
