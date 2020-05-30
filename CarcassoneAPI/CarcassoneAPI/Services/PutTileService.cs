﻿using CarcassoneAPI.Data;
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

            // connect to neoghbouring tiles
            var neighbourTop = await _tileRepository.GetTileAt(tile.BoardId, tile.X, tile.Y - 1);
            var neighbourRight = await _tileRepository.GetTileAt(tile.BoardId, tile.X + 1, tile.Y);
            var neighbourBottom = await _tileRepository.GetTileAt(tile.BoardId, tile.X, tile.Y + 1);
            var neighbourLeft = await _tileRepository.GetTileAt(tile.BoardId, tile.X - 1, tile.Y);

            if (neighbourTop != null) { ConnectComponents(TilePosition.Top, tile, neighbourTop); }
            if (neighbourRight != null) { ConnectComponents(TilePosition.Right, tile, neighbourRight); }
            if (neighbourBottom != null) { ConnectComponents(TilePosition.Bottom, tile, neighbourBottom); }
            if (neighbourLeft != null) { ConnectComponents(TilePosition.Left, tile, neighbourLeft); }

            // if any tile component was not connected to a bord component, create new board component
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

            return await _context.SaveChangesAsync() > 0;
        }

        private void ConnectComponents(TilePosition position, Tile tile, Tile neighbour)
        {
            var neighbourPosition = position.GetOpposite();

            // connect components in middle of tile side
            var neighbourComponent = neighbour.GetComponentAt(neighbourPosition);
            var component = tile.GetComponentAt(position);

            var bcComponent = _boardComponentRepository.Get(component.BoardComponentId);
            var bcNeighbour = _boardComponentRepository.Get(neighbourComponent.BoardComponentId);
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
            {/*
                // connect first field
                var fieldNeighbourLeft = neighbour.GetComponentAt(neighbourPosition.GetPositionLeftOfMiddle());
                var fieldTileRight = tile.GetComponentAt(position.GetPositionRightOfMiddle());

                var bcTileRightField = _boardComponentRepository.Get(fieldTileRight.BoardComponentId);
                var bcNeighbourLeftField = _boardComponentRepository.Get(fieldNeighbourLeft.BoardComponentId);

                // join new component to existing bc
                

                // connect second field
                var fieldNeighbourRight = neighbour.GetComponentAt(neighbourPosition.GetPositionRightOfMiddle());
                var fieldTileLeft = tile.GetComponentAt(position.GetPositionLeftOfMiddle());

                var bcTileLeftField = _boardComponentRepository.Get(fieldTileLeft.BoardComponentId);
                var bcNeighbourRightField = _boardComponentRepository.Get(fieldNeighbourRight.BoardComponentId);

                // join new component to existing bc
                */
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
