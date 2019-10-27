using System.Threading.Tasks;
using Refit;

namespace movement_simulator
{
    public interface IBackend
    {
        [Get("/animal")]
        Task<Animal[]> GetAnimals();

        [Post("/animal/{id}")]
        Task AddOrUpdateAnimal(int id, Animal animal);

        [Post("/log/{id}")]
        Task AddLog(int id, Log log);
    }
}