using CarcassoneAPI.DTOs;
using CarcassoneAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarcassoneAPI.Services.Interface
{
    public interface IBoardService
    {
        Task<IEnumerable<TilePuttedDTO>> GetAllTilesOfBoard(int boardId);
        Task<Tile> GetTile(int boardId, int x, int y);
        Task<TileToPutDTO> GetTileToPut(int boardId);
        Task<bool> ValidateTerrain(int boardId, TilePuttedDTO tile);
        Task<bool> CreateBoard(Board board);
        Task<Board> GetBoard(int boardId);
    }
}
