using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LabShortestRouteFinder.Model
{
    public class Route
    {
        //[JsonPropertyName("Start")]
        //public string StartName { get; set; } = string.Empty;

        //[JsonPropertyName("Destination")]
        //public string DestinationName { get; set; } = string.Empty;
        //[JsonPropertyName("Start")]
        public required CityNode Start { get; set; }

        //[JsonPropertyName("Destination")]
        public required CityNode Destination { get; set; }

        //[JsonPropertyName("DrivingDistance")]
        public int DrivingDistance { get; set; }

        //[JsonPropertyName("StraightLineDistance")]
        public double StraightLineDistance { get; set; }

        //[JsonPropertyName("Cost")]
        public int Cost { get; set; }  
    }
}
