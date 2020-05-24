using CarcassoneAPI.DTOs;
using CarcassoneAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarcassoneAPI.Services.Interface
{
    public interface IBoardService
    {
        Task<IEnumerable<TilePutted>> GetAllTilesOfBoard(int boardId);
        Task<Tile> GetTile(int boardId, int x, int y);
        Task<bool> PutTile(Tile tile);
        Task<TileToPut> GetTileToPut(int boardId);
        Task<bool> ValidateTerrain(int boardId, TilePutted tile);
    }
}
