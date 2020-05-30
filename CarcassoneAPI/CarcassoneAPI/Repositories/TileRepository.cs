using CarcassoneAPI.Data;
using CarcassoneAPI.Models;
using CarcassoneAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassoneAPI.Repositories
{
    public class TileRepository : Repository<Tile>, ITileRepository
    {
        public DataContext DataContext => _context as DataContext;

        public TileRepository(DataContext context) : base(context)
        {
                
        }

        public async Task<Tile> GetTileWithTileType(int id)
        {
            var tile = await _entities
                .Include(t => t.TileType).ThenInclude(tt => tt.Components)
                .Include(t => t.TileType).ThenInclude(tt => tt.Terrains)
                .Include(t => t.Components)
                .SingleOrDefaultAsync(t => t.TileId == id);

            return tile;
        }

        public async Task<Tile> GetTileAt(int boardId, int x, int y)
        {
            var tile = await _entities
                .Include(t => t.TileType).ThenInclude(tt => tt.Components)
                .Include(t => t.TileType).ThenInclude(tt => tt.Terrains)
                .Include(t => t.Components)
                .FirstOrDefaultAsync(t => t.BoardId == boardId && t.X == x && t.Y == y);

            return tile;
        }

        public async Task<bool> ExistTile(int boardId, int x, int y)
        {
            var existsTile = await _entities
                .AnyAsync(t => t.Board.BoardId == boardId && t.X == x && t.Y == y);

            return existsTile;
        }

        public async Task<int> GetCountOfSurrondingTiles(Tile tile)
        {
            var count = await _entities
                .Where(w => w.BoardId == tile.BoardId &&
                    w.X >= tile.X - 1 &&
                    w.X <= tile.X + 1 &&
                    w.Y >= tile.Y - 1 &&
                    w.Y <= tile.Y + 1)
                .CountAsync();

            return count;
        }
    }
}
