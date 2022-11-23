using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Lepidoptera;
using System.IO;

namespace Lepidoptera_DebugConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Deserializing JSON...");
            
            JsonSerializer serializer = new JsonSerializer();
            FeatureCollection fc = FeatureCollection.FromJSON("C:\\Users\\ATSmi\\Desktop\\DATA_25832.json");
            Console.WriteLine("JSON Deserialized Successfully");
            Console.WriteLine($"B1: {fc.features[0].properties.B1}, B2: {fc.features[0].properties.B2}, B3: {fc.features[0].properties.B3}");
            Console.ReadKey();
            
        }
    }
}
