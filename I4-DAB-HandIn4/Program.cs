using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace I4DABHandIn4
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Loading json!");

            LoadJson();
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
