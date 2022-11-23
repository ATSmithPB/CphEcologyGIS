using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lepidoptera
{
    public class S2Cell : ICloneable
    {
        public bool IsValid { get; set; }
        public int AOT { get; set; }
        public int B1 { get; set; }
        public int B2 { get; set; }
        public int B3 { get; set; }
        public int B4 { get; set; }
        public int B5 { get; set; }
        public int B6 { get; set; }
        public int B7 { get; set; }
        public int B8 { get; set; }
        public int B8A { get; set; }
        public int B9 { get; set; }
        public int B11 { get; set; }
        public int B12 { get; set; }
        public float EVI { get; set; }
        public float NDVI { get; set; }
        public int SCL { get; set; }
        public int TCI_R { get; set; }
        public int TCI_G { get; set; }
        public int TCI_B { get; set; }
        public int WVP { get; set; }
        public float bare { get; set; }
        public float built { get; set; }
        public float crops { get; set; }
        public float flooded_vegitation { get; set; }
        public float grass { get; set; }
        public int label { get; set; }
        public float shrub_and_scrub { get; set; }
        public float snow_and_ice { get; set; }
        public float trees { get; set; }
        public float water { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public string type { get; set; }

        //Methods
        public object Clone()
        {
            //Shallow copy
            return (S2Cell)this.MemberwiseClone();
        }
    }

    
}
