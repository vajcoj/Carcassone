using CarcassoneAPI.Models;
using System.Threading.Tasks;

namespace CarcassoneAPI.Services.Interface
{
    public interface IPutTileService
    {
        Task<bool> PutTile(Tile tile);
    }
}