using CarcassoneAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarcassoneAPI.Services.Interface
{
    public interface IBoardService : IBaseService
    {
        Task<IEnumerable<Tile>> GetAllTilesOfBoard(int idBoard);
        Task<Tile> GetTile(int boardId, int x, int y);
        Task<bool> PutTile(Tile tile);
    }
}
