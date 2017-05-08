using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Net;

namespace I4DABHandIn4
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Loading json from website!");

            //LoadJson(); local

            LoadFromSiteAsync(); // web
        }

        public static void LoadFromSiteAsync()
        {

            using (var httpClient = new WebClient())
            {
                var json = httpClient.DownloadString("http://userportal.iha.dk/~jrt/i4dab/E14/HandIn4/dataGDL/data/1.json");

                RootObject root = JsonConvert.DeserializeObject<RootObject>(json);


                Console.WriteLine("Time:" + root.timestamp + " Version: " + root.version);

            }
        }



        public static void LoadJson()
        {
			string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);


			using (StreamReader r = new StreamReader(path + "/1.json"))
			{
				string json = r.ReadToEnd();
				RootObject root = JsonConvert.DeserializeObject<RootObject>(json);


				Console.WriteLine("Time:" + root.timestamp + " Version: " + root.version);

			}
        }



    }

    public class Reading
    {
        public int sensorId { get; set; }
        public int appartmentId { get; set; }
        public double value { get; set; }
        public string timestamp { get; set; }
    }

    public class RootObject
    {
        public string timestamp { get; set; }
        public List<Reading> reading { get; set; }
        public int version { get; set; }
    }
}
