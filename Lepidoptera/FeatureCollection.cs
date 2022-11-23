using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLitePCL;
using GenericParsing;
using System.Security.Cryptography.X509Certificates;
using System.Data;

namespace Lepidoptera
{
    public class FeatureCollection : ICloneable
    {
        //Properties
        public bool IsValid { get; set; }
        public string type { get; set; }
        public List<Feature> features { get; set; }

        //Methods
        public static FeatureCollection FromJSON(string path)
        {
            StreamReader file = File.OpenText(path);
            JsonSerializer serializer = new JsonSerializer();
            FeatureCollection fc = (FeatureCollection)serializer.Deserialize(file, typeof(FeatureCollection));
            return fc;
        }

        public object Clone()
        {
            //Shallow copy
            return (FeatureCollection)this.MemberwiseClone();
        }
    }
}
