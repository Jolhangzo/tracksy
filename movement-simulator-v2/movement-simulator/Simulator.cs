using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Refit;

namespace movement_simulator
{
    public class Simulator
    {
        private readonly IBackend backend;
        private SimulatedAnimal[] animals;
        private static readonly Random Random = new Random();

        public Simulator()
        {
            backend = RestService.For<IBackend>("https://us-central1-tracksynew.cloudfunctions.net", new RefitSettings
            {
                ContentSerializer = new JsonContentSerializer(new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    }
                })
            });
        }

        public async Task Initialize()
        {
            await AddDefaultAnimals();
            animals = (await backend.GetAnimals()).Select(x => new SimulatedAnimal(x)).ToArray();
        }

        private async Task AddDefaultAnimals()
        {
            foreach (var a in AnimalStatic.Get())
                await backend.AddOrUpdateAnimal(a.AnimalId, a);
        }

        private int step;
        public async Task Step()
        {
            foreach (var animal in animals)
            {
                try
                {
                    UpdateAnimal(animal);
                    await backend.AddLog(animal.Animal.AnimalId, new Log
                    {
                        Coordinate = new Coordinate(animal.Position.X, animal.Position.Y),
                        AnimalId = animal.Animal.AnimalId,
                        DateTime = (ulong)(DateTime.UtcNow.AddYears(-2).AddHours(6*step) - DateTime.UnixEpoch).TotalMilliseconds
                    });
                    //Console.WriteLine($"{animal.Position.X.ToString(CultureInfo.InvariantCulture)}, {animal.Position.Y.ToString(CultureInfo.InvariantCulture)}");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            step++;
        }

        private void UpdateAnimal(SimulatedAnimal animal)
        {
            var selectNewTargetPlace = IsReachedTargetPlace(animal);
            if (selectNewTargetPlace)
            {
                animal.TargetPlace = animal.KnownPlaces.Except(new[] {animal.TargetPlace})
                    .ElementAt(Random.Next(animal.KnownPlaces.Length - 1));
            }

            if (animal.stepsUntilNextDecision == 0)
            {
                var randomSpeed = (float) Random.NextDouble() * 0.3f + 0.01f;
                if (Random.Next(6) == 0)
                    animal.Velocity = Vector2.Normalize(new Vector2(Random.Next(-360, 360), Random.Next(-360, 360))) * randomSpeed;
                else
                    animal.Velocity = Vector2.Normalize(animal.TargetPlace - animal.Position) * randomSpeed;
                animal.stepsUntilNextDecision = 1;
            }

            animal.stepsUntilNextDecision--;
            animal.Position += animal.Velocity;
        }

        private static bool IsReachedTargetPlace(SimulatedAnimal animal)
        {
            return Vector2.Distance(animal.TargetPlace, animal.Position) < 0.01;
        }
    }
}