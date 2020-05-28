using CarcassoneAPI.Models;
using System.Threading.Tasks;

namespace CarcassoneAPI.Repositories.Interface
{
    public interface ITileRepository
    {
        Task<Tile> GetTileAt(int boardId, int x, int y);
    }
}
