using System.Threading.Tasks;

namespace JunctionX.Services
{
    public interface IShareService
    {
        Task ShareLink(string message);
    }
}
