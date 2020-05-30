using CarcassoneAPI.Data;
using CarcassoneAPI.Models;
using CarcassoneAPI.Repositories.Interface;
using CarcassoneAPI.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassoneAPI.Services
{
    public class PutTileService : IPutTileService
    {
        private readonly DataContext _context;
        private readonly ITileRepository _tileRepository;
        private readonly IBoardComponentRepository _boardComponentRepository;

        public PutTileService(DataContext context, ITileRepository tileRepository, IBoardComponentRepository boardComponentRepository)
        {
            _context = context;
            _tileRepository = tileRepository;
            _boardComponentRepository = boardComponentRepository;
        }

        public async Task<bool> PutTile(Tile tile)
        {
            // construct Tile object
            await CreateTileComponents(tile);

            // connect to board components via neighbouring tiles
            await ConnectToNeihbours(tile);
       
            // create new board components for those components of current tile, which were not connecteed to any board component
            CreateBoardComponentsForOrphans(tile);

            await CheckClosedMonasteries(tile.BoardId);

            return await _context.SaveChangesAsync() > 0;
        }

        private async Task CreateTileComponents(Tile tile)
        {
            var type = await _context.TileTypes
                .Include(t => t.Components).ThenInclude(c => c.Terrains)
                .Include(t => t.Terrains)
                .FirstOrDefaultAsync(w => w.TileTypeId == tile.TileTypeId);

            tile.TileType = type;
            tile.Components = new List<TileComponent>();

            foreach (var typeComponent in type.Components)
            {
                tile.Components.Add(new TileComponent
                {
                    Tile = tile,
                    TileTypeComponent = typeComponent,
                    TerrainType = typeComponent.TerrainType
                });
            }

            _context.Tiles.Add(tile);
        }

        private async Task ConnectToNeihbours(Tile tile)
        {
            var neighbourTop = await _tileRepository.GetTileAt(tile.BoardId, tile.X, tile.Y - 1);
            var neighbourRight = await _tileRepository.GetTileAt(tile.BoardId, tile.X + 1, tile.Y);
            var neighbourBottom = await _tileRepository.GetTileAt(tile.BoardId, tile.X, tile.Y + 1);
            var neighbourLeft = await _tileRepository.GetTileAt(tile.BoardId, tile.X - 1, tile.Y);

            if (neighbourTop != null) { await ConnectComponents(TilePosition.Top, tile, neighbourTop); }
            if (neighbourRight != null) { await ConnectComponents(TilePosition.Right, tile, neighbourRight); }
            if (neighbourBottom != null) { await ConnectComponents(TilePosition.Bottom, tile, neighbourBottom); }
            if (neighbourLeft != null) { await ConnectComponents(TilePosition.Left, tile, neighbourLeft); }
        }

        private async Task ConnectComponents(TilePosition position, Tile tile, Tile neighbour)
        {
            var neighbourPosition = position.GetOpposite();

            // connect components in middle of tile side
            var neighbourComponent = neighbour.GetComponentAt(neighbourPosition);
            var component = tile.GetComponentAt(position);
            GetMergedComponent(neighbourComponent, component, out BoardComponent bcMerged);

            // get terrain type on connected side
            var terrain = neighbour.GetTerrainAt(neighbourPosition);
          
            if (terrain == TerrainType.Road)
            {
                CheckIsRoadClosed(bcMerged);

                // connect first field beside the road
                var tcNeighbourLeft = neighbour.GetComponentAt(neighbourPosition.GetPositionLeftOfMiddle());
                var tcTileRight = tile.GetComponentAt(position.GetPositionRightOfMiddle());
                GetMergedComponent(tcNeighbourLeft, tcTileRight, out _);

                // connect second field beside the road
                var tcNeighbourRight = neighbour.GetComponentAt(neighbourPosition.GetPositionRightOfMiddle());
                var tcTileLeft = tile.GetComponentAt(position.GetPositionLeftOfMiddle());
                GetMergedComponent(tcNeighbourRight, tcTileLeft, out _);

            }
            else if (terrain == TerrainType.Castle)
            {
                await CheckIsCastleClosed(bcMerged, tile.X, tile.Y);
            }
        }

        private void GetMergedComponent(TileComponent neighbourComponent, TileComponent component, out BoardComponent merged)
        {
            var bcComponent = _boardComponentRepository.Get(component.BoardComponentId) ?? component.BoardComponent;
            var bcNeighbour = _boardComponentRepository.Get(neighbourComponent.BoardComponentId) ?? neighbourComponent.BoardComponent;
            merged = bcNeighbour;

            // join new component to existing bc
            if (bcComponent != null)
            {
                merged = MergeBoardComponents(bcComponent, bcNeighbour);
            }

            merged.Components.Add(component);
            component.BoardComponent = merged;
            component.BoardComponentId = merged.BoardComponentId;
        }

        private BoardComponent MergeBoardComponents(BoardComponent bcTile, BoardComponent bcNeighbour)
        {
            if (bcTile.BoardComponentId == bcNeighbour.BoardComponentId)
            {
                // Components are alredy connected.
                return bcNeighbour;
            }

            var tcs = new List<TileComponent>();
            tcs.AddRange(bcTile.Components);
            tcs.AddRange(bcNeighbour.Components);

            // TODO: - don't create new bc - use one of the two components instead
            var merged = new BoardComponent
            {
                BoardId = bcNeighbour.BoardId,
                TerrainType = bcNeighbour.TerrainType,
                Components = tcs,
                IsOpen = true,
                Points = 0,
            };

            _boardComponentRepository.Add(merged);
            _boardComponentRepository.Remove(bcTile);
            _boardComponentRepository.Remove(bcNeighbour);

            return merged;
        }

        public void CheckIsRoadClosed(BoardComponent bc)
        {
            if (bc.TerrainType != TerrainType.Road) return;

            // road is closed when it has two components with only one terrain (road end)
            var groupRoadsWithOneEnd = bc.Components
                .Select(c => c.TileTypeComponent.Terrains.Count())
                .GroupBy(count => count)
                .FirstOrDefault(w => w.Key == 1);
                
            if (groupRoadsWithOneEnd == null) { return;  }

            var countOfComponentsWithRoadEnd = groupRoadsWithOneEnd.Count();

            if (countOfComponentsWithRoadEnd > 1)
            {
                bc.Points = bc.Components.Count(); // should be tiles count
            }
        }

        public async Task CheckIsCastleClosed(BoardComponent bc, int currentTileX, int currentTileY)
        {
            if (bc.TerrainType != TerrainType.Castle) return;

            // castle is closed if each component has a neighbouring tile on each tile side
            foreach (var component in bc.Components)
            {
                var tile = component.Tile;

                if (tile.TileId != 0)
                {
                    await _tileRepository.GetTileWithTileType(tile.TileId);
                }

                if (tile.GetComponentAt(TilePosition.Top).TileTypeComponentId == component.TileTypeComponentId)
                {
                    bool existNeighbour = await _tileRepository.ExistTile(bc.BoardId, tile.X, tile.Y - 1);
                    bool isClosedByCurrentTile = tile.X == currentTileX && tile.Y - 1 == currentTileY;
                    if (!existNeighbour && !isClosedByCurrentTile) return;
                }

                if (tile.GetComponentAt(TilePosition.Right).TileTypeComponentId == component.TileTypeComponentId)
                {
                    bool existNeighbour = await _tileRepository.ExistTile(bc.BoardId, tile.X + 1, tile.Y);
                    bool isClosedByCurrentTile = tile.X + 1 == currentTileX && tile.Y == currentTileY;
                    if (!existNeighbour && !isClosedByCurrentTile) return;
                }

                if (tile.GetComponentAt(TilePosition.Bottom).TileTypeComponentId == component.TileTypeComponentId)
                {
                    bool existNeighbour = await _tileRepository.ExistTile(bc.BoardId, tile.X, tile.Y + 1);
                    bool isClosedByCurrentTile = tile.X == currentTileX && tile.Y + 1 == currentTileY;
                    if (!existNeighbour && !isClosedByCurrentTile) return;
                }

                if (tile.GetComponentAt(TilePosition.Left).TileTypeComponentId == component.TileTypeComponentId)
                {
                    bool existNeighbour = await _tileRepository.ExistTile(bc.BoardId, tile.X - 1, tile.Y);
                    bool isClosedByCurrentTile = tile.X - 1 == currentTileX && tile.Y == currentTileY;
                    if (!existNeighbour && !isClosedByCurrentTile) return;
                }
            }

            // castle is closed
            bc.IsOpen = false;
            bc.Points = bc.Components.Count(); // should be tiles count
        }

        public async Task CheckClosedMonasteries(int boardId)
        {
            var monasteries = await _boardComponentRepository.GetOpenMonasteries(boardId);

            foreach (var monastery in monasteries)
            {
                var tile = monastery.Components.FirstOrDefault().Tile;

                int count = await _tileRepository.GetCountOfSurrondingTiles(tile);
                if (count >= 8)
                {
                    monastery.IsOpen = false;
                    monastery.Points = 9;
                }
            }
        }

        private void CreateBoardComponentsForOrphans(Tile tile)
        {
            var orphanComponents = tile.Components.Where(c => c.BoardComponent == null);
            foreach (var tc in orphanComponents)
            {
                var boardComponent = new BoardComponent
                {
                    BoardId = tile.BoardId,
                    IsOpen = true,
                    Points = 0,
                    TerrainType = tc.TileTypeComponent.TerrainType,
                    Components = new List<TileComponent> { tc }
                };
                _boardComponentRepository.Add(boardComponent);
            }
        }

    }
}
