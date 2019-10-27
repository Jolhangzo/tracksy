using System;
using System.Linq;
using System.Numerics;

namespace movement_simulator
{
    public class SimulatedAnimal
    {
        private static readonly Random Random = new Random();
        public Animal Animal { get; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2[] KnownPlaces { get; set; }
        public Vector2 TargetPlace { get; set; }
        public int stepsUntilNextDecision;

        public SimulatedAnimal(Animal animal)
        {
            Animal = animal;
            Position = new Vector2(animal.Coordinate.Latitude, animal.Coordinate.Longitude);
            KnownPlaces = GenerateKnownPlaces();
            //foreach (var knownPlace in KnownPlaces)
            //    Console.WriteLine($"{knownPlace.X.ToString(CultureInfo.InvariantCulture)}, {knownPlace.Y.ToString(CultureInfo.InvariantCulture)}");
            TargetPlace = Position;
        }

        private Vector2[] GenerateKnownPlaces()
        {
            var num = Random.Next(3) + 3;
            return Enumerable.Range(0, num).Select(x =>
            {
                var randomDistance = Random.Next(9) + 1;
                return Position + Vector2.Normalize(new Vector2(Random.Next(-360, 360), Random.Next(-360, 360))) * randomDistance;
            }).Concat(new[] {Position}).ToArray();
        }
    }
}