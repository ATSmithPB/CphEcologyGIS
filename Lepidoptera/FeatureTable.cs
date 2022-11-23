using GenericParsing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lepidoptera
{
    public class FeatureTable : ICloneable
    {
        //Properties
        public bool IsValid { get; set; }
        public DataTable Features { get; set; }
        public string Type { get; set; }

        //Constructor
        public FeatureTable()
        {
        }

        public FeatureTable(string name)
        {
            Type = name;
            Features = new DataTable(name);
            IsValid = true;
        }

        public FeatureTable(string name, string path)
        {
            Type = name;
            Features = DataTableFromCSV(name, path);
            IsValid = true;

        }

        public FeatureTable(string name, FeatureCollection fc)
        {
            Type = name;
            Features = DataTableFromFeatureCollection(name, fc);
            IsValid = true;
        }

        //Methods
        public static DataTable DataTableFromFeatureCollection(string name, FeatureCollection fc)
        {
            if (fc.features.Count < 1)
            {
                throw new ArgumentException("Error: FeatureCollection has no features");
            }

            GenericParser parser = new GenericParser();
            DataTable table = new DataTable(name);
            DataRow row;
            PopulateColumns(table);

            for (int i = 0; i < fc.features.Count; i++)
            {
                row = table.NewRow();
                row["system:index"] = i;
                row["B1"] = fc.features[i].properties.B1;
                row["B2"] = fc.features[i].properties.B2;
                row["B3"] = fc.features[i].properties.B3;
                row["B4"] = fc.features[i].properties.B4;
                row["B5"] = fc.features[i].properties.B5;
                row["B6"] = fc.features[i].properties.B6;
                row["B7"] = fc.features[i].properties.B7;
                row["B8"] = fc.features[i].properties.B8;
                row["B8A"] = fc.features[i].properties.B8A;
                row["B9"] = fc.features[i].properties.B9;
                row["B11"] = fc.features[i].properties.B11;
                row["B12"] = fc.features[i].properties.B12;
                row["AOT"] = fc.features[i].properties.AOT;
                row["EVI"] = fc.features[i].properties.EVI;
                row["NDVI"] = fc.features[i].properties.NDVI;
                row["SCL"] = fc.features[i].properties.SCL;
                row["TCI_R"] = fc.features[i].properties.TCI_R;
                row["TCI_G"] = fc.features[i].properties.TCI_G;
                row["TCI_B"] = fc.features[i].properties.TCI_B;
                row["WVP"] = fc.features[i].properties.WVP;
                row["bare"] = fc.features[i].properties.bare;
                row["built"] = fc.features[i].properties.built;
                row["crops"] = fc.features[i].properties.crops;
                row["flooded_vegitation"] = fc.features[i].properties.flooded_vegitation;
                row["grass"] = fc.features[i].properties.grass;
                row["shrub_and_scrub"] = fc.features[i].properties.shrub_and_scrub;
                row["snow_and_ice"] = fc.features[i].properties.snow_and_ice;
                row["trees"] = fc.features[i].properties.trees;
                row["water"] = fc.features[i].properties.water;
                row["label"] = fc.features[i].properties.label;
                row["x"] = fc.features[i].properties.x;
                row["y"] = fc.features[i].properties.y;
                row[".geo"] = $"POINT ({fc.features[i].geometry.coordinates[0]} {fc.features[i].geometry.coordinates[1]})";
                table.Rows.Add(row);
            }
            return table;
        }

        public static DataTable DataTableFromCSV(string name, string path)
        {
            if (!path.EndsWith(".csv"))
            {
                throw new ArgumentException("Error: FilePath must end with .csv");
            }

            GenericParser parser = new GenericParser();
            DataTable table = new DataTable(name);
            DataRow row;
            PopulateColumns(table);

            parser.SetDataSource(path);
            parser.ColumnDelimiter = ',';
            parser.FirstRowHasHeader = true;
            parser.MaxBufferSize = 4096;
            parser.MaxRows = 2000000;
            parser.TextQualifier = '"';

            while (parser.Read())
            {
                row = table.NewRow();
                AddRowData(parser, row);
                table.Rows.Add(row);
            }
            return table;
        }

        public static void AddColumn(DataTable table, string typeString, string columnName)
        {
            DataColumn column = new DataColumn();
            column.DataType = System.Type.GetType(typeString);
            column.ColumnName = columnName;
            column.AutoIncrement = false;
            column.ReadOnly = true;
            column.Unique = false;
            table.Columns.Add(column);
        }

        public static void PopulateColumns(DataTable table)
        {
            AddColumn(table, "System.Int32", "system:index");
            AddColumn(table, "System.Int32", "B1");
            AddColumn(table, "System.Int32", "B2");
            AddColumn(table, "System.Int32", "B3");
            AddColumn(table, "System.Int32", "B4");
            AddColumn(table, "System.Int32", "B5");
            AddColumn(table, "System.Int32", "B6");
            AddColumn(table, "System.Int32", "B7");
            AddColumn(table, "System.Int32", "B8");
            AddColumn(table, "System.Int32", "B8A");
            AddColumn(table, "System.Int32", "B9");
            AddColumn(table, "System.Int32", "B11");
            AddColumn(table, "System.Int32", "B12");
            AddColumn(table, "System.Int32", "AOT");
            AddColumn(table, "System.Single", "EVI");
            AddColumn(table, "System.Single", "NDVI");
            AddColumn(table, "System.Int32", "SCL");
            AddColumn(table, "System.Int32", "TCI_R");
            AddColumn(table, "System.Int32", "TCI_G");
            AddColumn(table, "System.Int32", "TCI_B");
            AddColumn(table, "System.Int32", "WVP");
            AddColumn(table, "System.Single", "bare");
            AddColumn(table, "System.Single", "built");
            AddColumn(table, "System.Single", "flooded_vegitation");
            AddColumn(table, "System.Single", "grass");
            AddColumn(table, "System.Single", "shrub_and_scrub");
            AddColumn(table, "System.Single", "snow_and_ice");
            AddColumn(table, "System.Single", "trees");
            AddColumn(table, "System.Single", "water");
            AddColumn(table, "System.Int32", "label");
            AddColumn(table, "System.Double", "x");
            AddColumn(table, "System.Double", "y");
            AddColumn(table, "System.String", ".geo");
        }

        public static void AddRowData(GenericParser parser, DataRow row)
        {
            row["system:index"] = parser["system:index"];
            row["B1"] = parser["B1"];
            row["B2"] = parser["B2"];
            row["B3"] = parser["B3"];
            row["B4"] = parser["B4"];
            row["B5"] = parser["B5"];
            row["B6"] = parser["B6"];
            row["B7"] = parser["B7"];
            row["B8"] = parser["B8"];
            row["B8A"] = parser["B8A"];
            row["B9"] = parser["B9"];
            row["B11"] = parser["B11"];
            row["B12"] = parser["B12"];
            row["AOT"] = parser["AOT"];
            row["EVI"] = parser["EVI"];
            row["NDVI"] = parser["NDVI"];
            row["SCL"] = parser["SCL"];
            row["TCI_R"] = parser["TCI_R"];
            row["TCI_G"] = parser["TCI_G"];
            row["TCI_B"] = parser["TCI_B"];
            row["WVP"] = parser["WVP"];
            row["bare"] = parser["bare"];
            row["built"] = parser["built"];
            row["crops"] = parser["crops"];
            row["flooded_vegitation"] = parser["flooded_vegitation"];
            row["grass"] = parser["grass"];
            row["shrub_and_scrub"] = parser["shrub_and_scrub"];
            row["snow_and_ice"] = parser["snow_and_ice"];
            row["trees"] = parser["trees"];
            row["water"] = parser["water"];
            row["label"] = parser["label"];
            row["x"] = parser["x"];
            row["y"] = parser["y"];
            row[".geo"] = parser[".geo"];
        }

        public object Clone()
        {
            //Shallow copy
            return (FeatureCollection)this.MemberwiseClone();
        }
    }
}
