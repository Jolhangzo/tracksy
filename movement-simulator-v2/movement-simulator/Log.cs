using System;

namespace movement_simulator
{
    public class Log
    {
        public Coordinate Coordinate { get; set; }
        public int AnimalId { get; set; }
        public ulong DateTime { get; set; }
        public DateTime DateTimeValue => new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(DateTime);
    }
}
