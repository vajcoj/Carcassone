using CarcassoneAPI.Data;
using CarcassoneAPI.Models;
using CarcassoneAPI.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassoneAPI.Services
{
    // TODO: generic repository
    public class BoardService : BaseService, IBoardService
    {
        public BoardService(DataContext context) :base(context)
        {
        }

        public async Task<IEnumerable<Tile>> GetAllTilesOfBoard(int idBoard)
        {
            return await _context.Tiles.Where(w => w.Board.BoardId == idBoard).ToListAsync();   
        }

        public async Task<Tile> GetTile(int boardId, int x, int y)
        {
            var tile = await _context.Tiles.FirstOrDefaultAsync(t => t.Board.BoardId == boardId && t.X == x && t.Y == y);
            return tile;
        }

        public async Task<bool> PutTile(Tile tile)
        {
            // Check if available - has neighbors

            Add(tile);
            return await SaveAll();
        }
    }
}
