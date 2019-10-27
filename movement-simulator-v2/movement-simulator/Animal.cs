using System;

namespace movement_simulator
{
    public class Animal
    {
        public Coordinate Coordinate { get; set; }
        public int AnimalId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string ImageUrl { get; set; }
        public string[] PhotoUrls { get; set; }
        public ulong Birthdate { get; set; }
        public DateTime BirthdateValue => new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(Birthdate);
        public string Location { get; set; }
        public string Owner { get; set; }
    }
}