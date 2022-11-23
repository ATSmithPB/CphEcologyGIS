using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lepidoptera
{
    public class Geometry
    {
        public List<double> coordinates { get; set; }
        public name crs { get; set; }
        public bool geodesic { get; set; }
        public string type { get; set; }
    }
}
