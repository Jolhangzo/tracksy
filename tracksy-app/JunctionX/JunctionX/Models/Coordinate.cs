using System;
using MvvmCross.Plugin.Location;
using Newtonsoft.Json;

namespace JunctionX.Models
{
    public class Coordinate
    {
        public Coordinate()
        {

        }
        public Coordinate(float latitude, float longitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }

        public Coordinate(MvxCoordinates mvxCoordinates)
        {
            Longitude = (float)mvxCoordinates.Longitude;
            Latitude = (float)mvxCoordinates.Latitude;
        }

        [JsonProperty("lng")]
        public float Longitude { get; set; }
        [JsonProperty("lat")]
        public float Latitude { get; set; }
    }
}