using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lepidoptera
{
    public class Region
    {
        //Properties
        public double West { get; set; }
        public double South { get; set; }
        public double East { get; set; }
        public double North { get; set; }
        public int nX { get; set; }
        public int nY { get; set; }
        public int CellSize { get; set; }
        public string CRS { get; set; }
        public S2Cell[,] Cells { get; set; }


        //Methods

    }
}
