using CarcassoneAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarcassoneAPI.Services.Interface
{
    public interface IBoardService
    {
        Task<IEnumerable<Tile>> GetAllTilesOfBoard(int idBoard);
    }
}
