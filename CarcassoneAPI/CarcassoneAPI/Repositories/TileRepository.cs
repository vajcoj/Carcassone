using CarcassoneAPI.Data;
using CarcassoneAPI.Models;
using CarcassoneAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace CarcassoneAPI.Repositories
{
    public class TileRepository : Repository<Tile>, ITileRepository
    {
        public DataContext DataContext => _context as DataContext;

        public TileRepository(DataContext context) : base(context)
        {
                
        }

        public async Task<Tile> GetTileAt(int boardId, int x, int y)
        {
            var tile = await DataContext.Tiles
                .Include(t => t.TileType).ThenInclude(tt => tt.Components)
                .Include(t => t.TileType).ThenInclude(tt => tt.Terrains)
                .Include(t => t.Components)
                .FirstOrDefaultAsync(t => t.BoardId == boardId && t.X == x && t.Y == y);

            return tile;
        }
    }
}
