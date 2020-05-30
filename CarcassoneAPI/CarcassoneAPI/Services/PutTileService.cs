using CarcassoneAPI.Data;
using CarcassoneAPI.Models;
using CarcassoneAPI.Repositories.Interface;
using CarcassoneAPI.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System;
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
            await CreateTileComponents(tile);

            await ConnectToNeihbours(tile);
       
            CreateBoardComponentsForOrphans(tile);

            // check closed monasteries

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

            var bcComponent = _boardComponentRepository.Get(component.BoardComponentId) ?? component.BoardComponent;
            var bcNeighbour = _boardComponentRepository.Get(neighbourComponent.BoardComponentId) ?? neighbourComponent.BoardComponent;
            var bcMerged = bcNeighbour;

            // join new component to existing bc
            if (bcComponent != null) // TODO out variable???
            {
                bcMerged = MergeBoardComponents(bcComponent, bcNeighbour);
            }

            string pos = Enum.GetName(typeof(TilePosition), position);
            System.Console.WriteLine($"Adding component to merged bc: {bcMerged.BoardComponentId} from {pos} \n");

            bcMerged.Components.Add(component);
            component.BoardComponent = bcMerged;
            component.BoardComponentId = bcMerged.BoardComponentId;
     
            // get terrain type on connected side
            var terrain = neighbour.GetTerrainAt(neighbourPosition);

            // connect fields besides the road
            if (terrain == TerrainType.Road)
            {
                bool isClosed = IsRoadClosed(bcMerged);
                bcMerged.IsOpen = !isClosed;

                if (isClosed)
                {
                    bcMerged.Points = bcMerged.Components.Count(); // should be tiles count
                }

                // connect first field - neighbour right from the road and new tile left from the road
                var tcNeighbourLeft = neighbour.GetComponentAt(neighbourPosition.GetPositionLeftOfMiddle());
                var tcTileRight = tile.GetComponentAt(position.GetPositionRightOfMiddle());

                var bcTileRight = _boardComponentRepository.Get(tcTileRight.BoardComponentId) ?? tcTileRight.BoardComponent;
                var bcNeighbourLeft = _boardComponentRepository.Get(tcNeighbourLeft.BoardComponentId) ?? tcNeighbourLeft.BoardComponent;
                var bcMerged1 = bcNeighbourLeft;

                // join new component to existing bc
                if (bcTileRight != null) 
                {
                    bcMerged1 = MergeBoardComponents(bcTileRight, bcNeighbourLeft);
                }

                bcMerged1.Components.Add(tcTileRight);
                tcTileRight.BoardComponent = bcMerged1;
                tcTileRight.BoardComponentId = bcMerged1.BoardComponentId;


                // connect second field - neighbour left from the road and new tile right from the road
                var tcNeighbourRight = neighbour.GetComponentAt(neighbourPosition.GetPositionRightOfMiddle());
                var tcTileLeft = tile.GetComponentAt(position.GetPositionLeftOfMiddle());

                var bcTileLeft = _boardComponentRepository.Get(tcTileLeft.BoardComponentId) ?? tcTileLeft.BoardComponent;
                var bcNeighbourRight = _boardComponentRepository.Get(tcNeighbourRight.BoardComponentId) ?? tcNeighbourRight.BoardComponent;
                var bcMerged2 = bcNeighbourRight;

                // join new component to existing bc
                if (bcTileLeft != null)
                {
                    bcMerged2 = MergeBoardComponents(bcTileLeft, bcNeighbourRight);
                }

                bcMerged2.Components.Add(tcTileLeft);
                tcTileLeft.BoardComponent = bcMerged2;
                tcTileLeft.BoardComponentId = bcMerged2.BoardComponentId;

            }
            else if (terrain == TerrainType.Castle)
            {
                bool isClosed = await IsCastleClosed(bcMerged, tile.X, tile.Y);

                bcMerged.IsOpen = !isClosed;

                if (isClosed)
                {
                    bcMerged.Points = bcMerged.Components.Count(); // should be tiles count
                }
            }

        }

        private BoardComponent MergeBoardComponents(BoardComponent bcTile, BoardComponent bcNeighbour)
        {
            if (bcTile.BoardComponentId == bcNeighbour.BoardComponentId)
            {
                System.Console.WriteLine($"Components alredy connected. {bcTile.BoardComponentId}");
                return bcNeighbour;
            }

            System.Console.WriteLine($"Joining two global components {bcTile.BoardComponentId} and {bcNeighbour.BoardComponentId}");

            var tcs = new List<TileComponent>();
            tcs.AddRange(bcTile.Components);
            tcs.AddRange(bcNeighbour.Components);
            bcTile.Components = null;
            bcNeighbour.Components = null;

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

        public bool IsRoadClosed(BoardComponent bc)
        {
            if (bc.TerrainType != TerrainType.Road) return false;

            // road is closed when it has two components with only one terrain (road end)
            var countOfComponentsWithRoadEnd = bc.Components
                .Select(c => c.TileTypeComponent.Terrains.Count())
                .GroupBy(count => count)
                .FirstOrDefault(w => w.Key == 1)
                .Count();

            return countOfComponentsWithRoadEnd > 1;
        }

        public async Task<bool> IsCastleClosed(BoardComponent bc, int currentTileX, int currentTileY)
        {
            if (bc.TerrainType != TerrainType.Castle) return false;

            // castle is closed if each component has a neighbouring tile on each tile side
            foreach (var component in bc.Components)
            {
                var tile = component.Tile;

                if (tile.TileId != 0)
                {
                    await _tileRepository.GetTileWithTileType(tile.TileId);
                }

                Console.WriteLine($"Closing caste on {tile.X}, {tile.Y}");

                if (tile.GetComponentAt(TilePosition.Top).TileTypeComponentId == component.TileTypeComponentId)
                {
                    bool existNeighbour = await _tileRepository.ExistTile(bc.BoardId, tile.X, tile.Y - 1);
                    bool isClosedByCurrentTile = tile.X == currentTileX && tile.Y - 1 == currentTileY;
                    if (!existNeighbour && !isClosedByCurrentTile) return false;
                }

                if (tile.GetComponentAt(TilePosition.Right).TileTypeComponentId == component.TileTypeComponentId)
                {
                    bool existNeighbour = await _tileRepository.ExistTile(bc.BoardId, tile.X + 1, tile.Y);
                    bool isClosedByCurrentTile = tile.X + 1 == currentTileX && tile.Y == currentTileY;
                    if (!existNeighbour && !isClosedByCurrentTile) return false;
                }

                if (tile.GetComponentAt(TilePosition.Bottom).TileTypeComponentId == component.TileTypeComponentId)

                {
                    bool existNeighbour = await _tileRepository.ExistTile(bc.BoardId, tile.X, tile.Y + 1);
                    bool isClosedByCurrentTile = tile.X == currentTileX && tile.Y + 1 == currentTileY;
                    if (!existNeighbour && !isClosedByCurrentTile) return false;
                }

                if (tile.GetComponentAt(TilePosition.Left).TileTypeComponentId == component.TileTypeComponentId)
                {
                    bool existNeighbour = await _tileRepository.ExistTile(bc.BoardId, tile.X - 1, tile.Y);
                    bool isClosedByCurrentTile = tile.X - 1 == currentTileX && tile.Y == currentTileY;
                    if (!existNeighbour && !isClosedByCurrentTile) return false;
                }
            }

            return true;
        }

        // if any tile component was not connected to a board component, create new board component
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
