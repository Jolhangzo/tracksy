using System;
using System.Linq;

namespace JunctionX.Models
{
    public class Article
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Enclosure[] Enclosures { get; set; }
        public ulong Created { get; set; }
        public string Category { get; set; }
        public DateTime DateTime => new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(Created);
        public string ImageUrl => Enclosures.FirstOrDefault()?.Url;
    }

    public class Enclosure
    {
        public string Url { get; set; }
    }
}
