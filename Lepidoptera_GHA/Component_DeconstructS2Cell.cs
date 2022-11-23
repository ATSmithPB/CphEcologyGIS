using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using Lepidoptera;
using Lepidoptera_IO_Rhino;
using Newtonsoft.Json;
using System.IO;


namespace Lepidoptera_GHA
{
    public class LO_DeconstructS2Cell : GH_Component
    {
        public LO_DeconstructS2Cell()
            : base(nameof(LO_DeconstructS2Cell), "dS2", "Deconstruct a Lepidoptera S2Cell (Sentinal-2 Pixel)", "Lepidoptera", "Regional Analysis")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("S2Cell", "S2", "A Lepidoptera S2Cell to Deconstruct", GH_ParamAccess.item);
        }

        private static int IN_S2Cell = 0;
        private static int OUT_B1 = 0;
        private static int OUT_B2 = 1;
        private static int OUT_B3 = 2;
        private static int OUT_B4 = 3;
        private static int OUT_B5 = 4;
        private static int OUT_B6 = 5;
        private static int OUT_B8 = 6;
        private static int OUT_B8A = 7;
        private static int OUT_B9 = 8;
        private static int OUT_B11 = 9;
        private static int OUT_B12 = 10;
        private static int OUT_EVI = 11;
        private static int OUT_NDVI = 12;
        private static int OUT_SCL = 13;
        private static int OUT_TCI_R = 14;
        private static int OUT_TCI_G = 15;
        private static int OUT_TCI_B = 16;
        private static int WVP = 17;
        private static int bare = 18;
        private static int built = 19;
        private static int crops = 20;
        private static int flooded = 21;
        private static int grass = 22;
        private static int label = 23;
        private static int shrub_and_scrub = 24;
        private static int snow_and_ice = 25;
        private static int trees = 26;
        private static int water = 27;
        private static int x = 28;
        private static int y = 29;

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Features", "F", "A list of Lepidoptera Features", GH_ParamAccess.list);
            pManager.AddTextParameter("Type", "T", "The FeatureCollection Type", GH_ParamAccess.item);
            pManager.AddBooleanParameter("IsValid", "iV", "True if FeatureCollection is Valid", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            LO_DeconstructFeatureCollection.DeonstructFeatureCollectionFromDA(DA);
        }

        protected override System.Drawing.Bitmap Icon => null;
        public override Guid ComponentGuid => new Guid("0dc7d4de-dca6-4953-a028-132a45cb4598");
        private static void DeonstructFeatureCollectionFromDA(IGH_DataAccess DA)
        {
            FeatureCollectionGoo fcGoo = new FeatureCollectionGoo();

            if (!DA.GetData<Lepidoptera.FeatureCollectionGoo>(IN_FeatureCollection, ref fcGoo)) { return; }

            DA.SetDataList(OUT_Features, fcGoo.Value.features);
            DA.SetData(OUT_Type, fcGoo.Value.type);
            DA.SetData(OUT_IsValid, fcGoo.Value.IsValid);

        }
    }
}