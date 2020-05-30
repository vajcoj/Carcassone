using CarcassoneAPI.Models;
using System.Threading.Tasks;

namespace CarcassoneAPI.Repositories.Interface
{
    public interface ITileRepository : IRepository<Tile>
    {
        public Task<Tile> GetTileWithTileType(int id);
        Task<Tile> GetTileAt(int boardId, int x, int y);
        Task<bool> ExistTile(int boardId, int x, int y);
        Task<int> GetCountOfSurrondingTiles(Tile tile);
    }
}
