using System.Threading.Tasks;
using JunctionX.Models;
using Refit;

namespace JunctionX.Services
{
    public interface IBackend
    {
        [Get("/animal")]
        Task<Animal[]> GetAnimals();

        [Get("/log/{id}")]
        Task<Log[]> GetLogs(int id);

        [Get("/getFeed")]
        Task<Article[]> GetArticles();
    }
}
