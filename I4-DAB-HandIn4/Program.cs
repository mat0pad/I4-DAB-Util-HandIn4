using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Net;

namespace I4DABHandIn4
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Loading json from website!");

            //LoadCharacteristicsFromSite();

            //LoadSensorInfoFromSite();

            LoadApartment2SensorFromSite();

            /*for (int i = 1; i < 11801; i++)
            {
                LoadSampleFromSite(i);
            }*/

        }

        // Load sample from website
        public static void LoadSampleFromSite(int i)
        {

            using (var httpClient = new WebClient())
            {
                var json = httpClient.DownloadString("http://userportal.iha.dk/~jrt/i4dab/E14/HandIn4/dataGDL/data/" + i + ".json");

                ReadingRootObject root = JsonConvert.DeserializeObject<ReadingRootObject>(json);

                Console.WriteLine("Time:" + root.timestamp + " Version: " + root.version);

                // Put into database here!
            }
        }

        // Load sample from desktop
        public static void LoadSampleFromLocalJson()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);


            using (StreamReader r = new StreamReader(path + "/1.json"))
            {
                string json = r.ReadToEnd();
                ReadingRootObject root = JsonConvert.DeserializeObject<ReadingRootObject>(json);


                Console.WriteLine("Time:" + root.timestamp + " Version: " + root.version);
            }
        }

        // Load characteristics from website
        public static void LoadCharacteristicsFromSite()
        {
            using (var httpClient = new WebClient())
            {
                var json = httpClient.DownloadString("http://userportal.iha.dk/~jrt/i4dab/E14/HandIn4/GFKSC002_original.txt");

                CharacteristicRootObject root = JsonConvert.DeserializeObject<CharacteristicRootObject>(json);

                Console.WriteLine("Time:" + root.timestamp + " Version: " + root.version);

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
                            description = temp[2],
                            timestamp = temp[1],
                            sensorID = int.Parse(temp[0]),
                            units = temp[3]
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
                            apartmentID = (temp[1] != "\r" ? int.Parse(temp[1]) : 0),
                            sensorID = int.Parse(temp[0]),
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
        public int sensorId { get; set; }
        public int appartmentId { get; set; }
        public double value { get; set; }
        public string timestamp { get; set; }
    }

    public class ReadingRootObject
    {
        public string timestamp { get; set; }
        public List<Reading> reading { get; set; }
        public int version { get; set; }
    }

    /* Characteristics */

    public class AppartmentCharacteristic
    {
        public int No { get; set; }
        public double Size { get; set; }
        public int Floor { get; set; }
        public int appartmentId { get; set; }
    }

    public class SensorCharacteristic
    {
        public string calibrationCoeff { get; set; }
        public string description { get; set; }
        public string calibrationDate { get; set; }
        public string externalRef { get; set; }
        public int sensorId { get; set; }
        public string unit { get; set; }
        public string calibrationEquation { get; set; }
    }

    public class CharacteristicRootObject
    {
        public List<AppartmentCharacteristic> appartmentCharacteristic { get; set; }
        public string timestamp { get; set; }
        public int version { get; set; }
        public List<SensorCharacteristic> sensorCharacteristic { get; set; }
    }

    /* SensorInfo */

    public class SensorInfoObject
    {
        public string timestamp { get; set; }
        public string description { get; set; }
        public string units { get; set; }
        public int sensorID { get; set; }
    }

    /* Apartment2Sensor */

    public class Apartment2SensorObject
    {

        public int sensorID { get; set; }
        public int apartmentID { get; set; }
    }
}
