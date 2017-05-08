using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Threading;

namespace I4DABHandIn4
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Loading json here!");

            //LoadCharacteristicsFromSite();

            //LoadSensorInfoFromSite();

            //LoadApartment2SensorFromSite();

            for (int i = 1; i < 11801; i++)
            {
                LoadSampleFromSite(i);
                Thread.Sleep(2000);
            }

        }

        // Load sample from website
        public static void LoadSampleFromSite(int i)
        {
            using (var httpClient = new WebClient())
            {
                var json = httpClient.DownloadString("http://userportal.iha.dk/~jrt/i4dab/E14/HandIn4/dataGDL/data/" + i + ".json");

                ReadingRootObject root = JsonConvert.DeserializeObject<ReadingRootObject>(json);

                Console.WriteLine("Time:" + root.Timestamp + " Version: " + root.Version);

                // Put into database here!
            }
        }

        // Load sample from desktop DONT USE!
        public static void LoadSampleFromLocalJson()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            using (StreamReader r = new StreamReader(path + "/1.json"))
            {
                string json = r.ReadToEnd();
                ReadingRootObject root = JsonConvert.DeserializeObject<ReadingRootObject>(json);

                Console.WriteLine("Time:" + root.Timestamp + " Version: " + root.Version);

                // Put into database here!
            }
        }

        // Load characteristics from website
        public static void LoadCharacteristicsFromSite()
        {
            using (var httpClient = new WebClient())
            {
                var json = httpClient.DownloadString("http://userportal.iha.dk/~jrt/i4dab/E14/HandIn4/GFKSC002_original.txt");

                CharacteristicRootObject root = JsonConvert.DeserializeObject<CharacteristicRootObject>(json);

                Console.WriteLine("Time:" + root.Timestamp + " Version: " + root.Version);

                // Put into database here!
            }
        }

        // Load characteristics from website
        public static void LoadSensorInfoFromSite()
        {
            using (var httpClient = new WebClient())
            {

                var csv = httpClient.DownloadString("http://userportal.iha.dk/~jrt/i4dab/E14/HandIn4/sensors_information.txt");

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


                // Do something with list here!
            }
        }

        // Load characteristics from website
        public static void LoadApartment2SensorFromSite()
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


                // Do something with list here!
            }
        }
    }

    /* Sample */
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

    /* Characteristics */
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

    /* SensorInfo */
    public class SensorInfoObject
    {
        public string Timestamp { get; set; }
        public string Description { get; set; }
        public string Units { get; set; }
        public int SensorID { get; set; }
    }

    /* Apartment2Sensor */
    public class Apartment2SensorObject
    {

        public int SensorID { get; set; }
        public int ApartmentID { get; set; }
    }
}
