using System;
using System.Collections.Generic;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using Newtonsoft.Json;

namespace JunctionX.Models
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
        public DateTime BirthdateValue => new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(Birthdate);
        public string Location { get; set; }
        public string Owner { get; set; }
        public List<ITransformation> AnimalImageTransformations { get; } = new List<ITransformation>
        {
            new CircleTransformation(14, "#ffffff")
        };
    }
}
