using AutoMapper;
using CarcassoneAPI.Data;
using CarcassoneAPI.DTOs;
using CarcassoneAPI.Models;
using CarcassoneAPI.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassoneAPI.Services
{
    public class BoardService : IBoardService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public BoardService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TilePuttedDTO>> GetAllTilesOfBoard(int boardId)
        {
            var tiles =  await _context.Tiles
                .Include(t => t.TileType)
                .Include(t => t.TileType.Terrains)
                .Where(w => w.Board.BoardId == boardId)
                .Select(tile => _mapper.Map<Tile, TilePuttedDTO>(tile))
                .AsNoTracking()
                .ToListAsync();

            return tiles;
        }

        public async Task<Tile> GetTile(int boardId, int x, int y)
        {
            var tile = await _context.Tiles
                .Include(t => t.TileType.Components)
                .Include(t => t.TileType.Terrains)
                .Include(t => t.Components)
                .FirstOrDefaultAsync(t => t.Board.BoardId == boardId && t.X == x && t.Y == y);

            return tile;
        }

        public async Task<TileToPutDTO> GetTileToPut(int boardId)
        {
            var availableTypes = GetAvailableTileTypes(boardId);

            var type = await availableTypes
                .OrderBy(r => Guid.NewGuid()) // bude mit board vlastni guid??? resilo by to? - nebo jen ulozeny guid nekde
                .FirstOrDefaultAsync(); 

            // TODO: check null

            var toPut = _mapper.Map<TileType, TileToPutDTO>(type);
            

            if (toPut == null)
            {
                toPut = new TileToPutDTO
                {
                    TileTypeId = -1,
                    ImageUrl = "void"
                };
            }

            toPut.BoardId = boardId;

            return toPut;
        }

        // return TileTypes which weren't used yet or there are less tiles on the board of the type than type.Count
        private IQueryable<TileType> GetAvailableTileTypes(int boardId)
        {
            var usedTypeIds = _context.Tiles
                            .Where(w => w.BoardId == boardId)
                            .Select(t => t.TileTypeId);

            var availableTypes = _context.TileTypes
                .Include(type => type.Terrains)
                .Where(type =>
                    !usedTypeIds.Contains(type.TileTypeId)
                    ||
                    usedTypeIds.GroupBy(i => i)
                        .Where(g => g.Key == type.TileTypeId)
                        .Select(g => g.Count()).FirstOrDefault() < type.Count
                 )
                .AsNoTracking();

            return availableTypes;
        }

        public async Task<bool> PutTile(Tile tile)
        {
            var type = await _context.TileTypes
                .Include(t => t.Components)
                .FirstOrDefaultAsync(w => w.TileTypeId == tile.TileTypeId);

            tile.Components = new List<TileComponent>();

            foreach (var typeComponent in type.Components)
            {
                tile.Components.Add(new TileComponent
                {
                    Tile = tile,
                    TileTypeComponent = typeComponent,
                    IsOpen = true
                });     
            }

            _context.Tiles.Add(tile);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ValidateTerrain(int boardId, TilePuttedDTO tile)
        {
            var x = tile.X;
            var y = tile.Y;

            var tileTop = await _context.Tiles.Include(t => t.TileType).Include(t => t.TileType.Terrains)
                .Where(t => t.BoardId == boardId && t.X == x && t.Y == y - 1)
                .FirstOrDefaultAsync();

            var tileRight = await _context.Tiles.Include(t => t.TileType).Include(t => t.TileType.Terrains)
                .Where(t => t.BoardId == boardId && t.X == x + 1 && t.Y == y)
                .FirstOrDefaultAsync();

            var tileBottom = await _context.Tiles.Include(t => t.TileType).Include(t => t.TileType.Terrains)
                .Where(t => t.BoardId == boardId && t.X == x && t.Y == y + 1)
                .FirstOrDefaultAsync();

            var tileLeft = await _context.Tiles.Include(t => t.TileType).Include(t => t.TileType.Terrains)
                .Where(t => t.BoardId == boardId && t.X == x - 1 && t.Y == y)
                .FirstOrDefaultAsync();

            var terrainOfTop = tileTop?.GetTerrain(TilePosition.Bottom) ?? TerrainType.Void;
            var terrainOfRight = tileRight?.GetTerrain(TilePosition.Left) ?? TerrainType.Void;
            var terrainOfBottom = tileBottom?.GetTerrain(TilePosition.Top) ?? TerrainType.Void;
            var terrainOfLeft = tileLeft?.GetTerrain(TilePosition.Right) ?? TerrainType.Void;

            if (terrainOfTop != tile.Top && terrainOfTop != TerrainType.Void) return false;
            if (terrainOfRight != tile.Right && terrainOfRight != TerrainType.Void) return false;
            if (terrainOfBottom != tile.Bottom && terrainOfBottom != TerrainType.Void) return false;
            if (terrainOfLeft != tile.Left && terrainOfLeft != TerrainType.Void) return false;

            return true;
        }

        public async Task<bool> CreateBoard(Board board)
        {
            _context.Boards.Add(board);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Board> GetBoard(int boardId)
        {
            return await _context.Boards.FirstOrDefaultAsync(b => b.BoardId == boardId);
        }
    }
}
