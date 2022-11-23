using System.Collections.Generic;
using Rhino;
using Rhino.Geometry;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Lepidoptera;

namespace Lepidoptera
{

    /// <summary>
    /// Tier Goo wrapper class, makes sure Tier can be used in Grasshopper.
    /// </summary>
    public class S2CellGoo : GH_Goo<S2Cell>
    {
        //Constructors
        public S2CellGoo()
        {
            this.Value = new S2Cell();
        }
        public S2CellGoo(S2Cell s2cell)
        {
            if (s2cell.IsValid == false)
            {
                s2cell = new S2Cell();
            }
            this.Value = s2cell;
        }

        public override IGH_Goo Duplicate()
        {
            return Duplicate();
        }
        public S2CellGoo DuplicateBoundaryGoo()
        {
            return new S2CellGoo(Value.IsValid == false ? new S2Cell() : (Lepidoptera.S2Cell)Value.Clone());
        }

        public override bool IsValid
        {
            get
            {
                if (Value.IsValid == false) { return false; }
                return Value.IsValid;
            }
        }

        public override string ToString()
        {
            if (Value.IsValid == false)
            {
                return "Null S2Cell";
            }
            else
            {
                return $"Feature: B1:{Value.NDVI}";
            }
        }
        public override string TypeName
        {
            get { return ("S2Cell"); }
        }
        public override string TypeDescription
        {
            get { return ("Defines a single Lepidoptera S2Cell"); }
        }
    }

}
