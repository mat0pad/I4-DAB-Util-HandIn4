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
			int i = 1;
            string json;

			var webRequest = WebRequest.Create(@"http://userportal.iha.dk/~jrt/i4dab/E14/HandIn4/dataGDL/data/" + i + ".json");

			using (var response = webRequest.GetResponse())
			using (var content = response.GetResponseStream())
			using (var reader = new StreamReader(content))
			{
				json = reader.ReadToEnd();
			}


            RootObject root = JsonConvert.DeserializeObject<RootObject>(json);


                Console.WriteLine("Time:" + root.timestamp + " Version: " + root.version);
            Console.WriteLine("Id:" + root.reading[0].sensorId + " Version: " + root.version);
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
