using CarcassoneAPI.Models;
using System.Threading.Tasks;

namespace CarcassoneAPI.Services.Interface
{
    public interface ITileTypeService
    {
        Task<bool> SeedTileTypes();

    }
}
