using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lepidoptera
{
    public class Feature : ICloneable
    {
        //Properties
        public bool IsValid { get; set; }
        public Geometry geometry { get; set; }
        public S2Cell properties { get; set; }
        public string type { get; set; }
        
        //Methods
        public object Clone()
        {
            //Shallow copy
            return (FeatureCollection)this.MemberwiseClone();
        }
    }
}
