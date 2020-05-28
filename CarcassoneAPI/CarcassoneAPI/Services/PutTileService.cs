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

        public PutTileService(DataContext context, ITileRepository tileRepository)
        {
            _context = context;
            _tileRepository = tileRepository;
        }

        public async Task<bool> PutTile(Tile tile)
        {
            //var board = await GetBoardWithTiles(tile.BoardId);

            await CreateTileComponents(tile);

            // connect to neoghbouring tiles
            var neighbourTop = await _tileRepository.GetTileAt(tile.BoardId, tile.X, tile.Y - 1);
            var neighbourRight = await _tileRepository.GetTileAt(tile.BoardId, tile.X + 1, tile.Y);
            var neighbourBottom = await _tileRepository.GetTileAt(tile.BoardId, tile.X, tile.Y + 1);
            var neighbourLeft = await _tileRepository.GetTileAt(tile.BoardId, tile.X - 1, tile.Y);

            if (neighbourTop != null) {  ConnectComponents(TilePosition.Top, tile, neighbourTop); }
            if (neighbourRight != null) { ConnectComponents(TilePosition.Right, tile, neighbourRight); }
            if (neighbourBottom != null) { ConnectComponents(TilePosition.Bottom, tile, neighbourBottom); }
            if (neighbourLeft != null) { ConnectComponents(TilePosition.Left, tile, neighbourLeft); }

            // if any tile component was not connected to a bord component, create new board component
            var orphanComponents = tile.Components.Where(c => c.BoardComponentId == null);
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
                _context.BoardComponents.Add(boardComponent);
            }

            return await _context.SaveChangesAsync() > 0;
        }

        private async Task ConnectComponents(TilePosition position, Tile tile, Tile neighbour)
        {
            var neighbourPosition = position.GetOpposite();

            // connect components in middle of tile side
            var neighbourComponent = neighbour.GetComponentAt(neighbourPosition);
            var component = tile.GetComponentAt(position);

            if (component.BoardComponentId == null)
            {
                component.BoardComponentId = neighbourComponent.BoardComponentId;
            }
            else
            {
                await MergeBoardComponents(component.BoardComponentId, neighbourComponent.BoardComponentId);
            }

            // get terrain type on connected side
            var terrain = neighbour.GetTerrainAt(neighbourPosition);

            // connect fields besides the road
            if (terrain == TerrainType.Road)
            {
                // connect first field
                var fieldNeighbourLeft = neighbour.GetComponentAt(neighbourPosition.GetPositionLeftOfMiddle());
                var fieldTileRight = tile.GetComponentAt(position.GetPositionRightOfMiddle());

                if (fieldTileRight.BoardComponentId == null)
                {
                    fieldTileRight.BoardComponentId = fieldNeighbourLeft.BoardComponentId;
                }
                else
                {
                    await MergeBoardComponents(fieldTileRight.BoardComponentId, fieldNeighbourLeft.BoardComponentId);
                }

                // connect second field
                var fieldNeighbourRight = neighbour.GetComponentAt(neighbourPosition.GetPositionRightOfMiddle());
                var fieldTileLeft = tile.GetComponentAt(position.GetPositionLeftOfMiddle());

                if (fieldTileLeft.BoardComponentId == null)
                {
                    fieldTileLeft.BoardComponentId = fieldNeighbourRight.BoardComponentId;
                }
                else
                {
                    await MergeBoardComponents(fieldTileLeft.BoardComponentId, fieldNeighbourRight.BoardComponentId);
                }
 
            }
            
        }

        private async Task MergeBoardComponents(int? boardComponentId1, int? boardComponentId2)
        {
            System.Console.WriteLine($"joining two global components {boardComponentId1} and {boardComponentId2}");

            //var component1 = await _context.BoardComponents.SingleOrDefaultAsync(c => c.BoardComponentId == boardComponentId1);
            //var component2 = await _context.BoardComponents.SingleOrDefaultAsync(c => c.BoardComponentId == boardComponentId2);

            //// validation for board id and terrain ???

            //component2.Components.ToList().AddRange(component1.Components);

            //component1.Components = null;

            //_context.BoardComponents.Remove(component1);
        }

        private async Task CreateTileComponents(Tile tile)
        {
            var type = await _context.TileTypes
                .Include(t => t.Components)
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
                    IsOpen = true,
                    TerrainType = typeComponent.TerrainType
                });
            }

            _context.Tiles.Add(tile);
        }
    }
}
