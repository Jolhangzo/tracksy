using Newtonsoft.Json;

namespace movement_simulator
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

        [JsonProperty("lng")]
        public float Longitude { get; set; }
        [JsonProperty("lat")]
        public float Latitude { get; set; }
    }
}