using AutoMapper;
using CarcassoneAPI.DTOs;
using CarcassoneAPI.Models;
using CarcassoneAPI.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CarcassoneAPI.Controllers
{
    [ApiController]
    [Route("[controller]/{id}")]
    public class BoardController : ControllerBase
    {
        private readonly IBoardService _boardService;
        private readonly ITileTypeService _tileTypeService;
        private readonly IPutTileService _putTileService;
        private readonly IMapper _mapper;

        public BoardController(IBoardService boardService, ITileTypeService tileTypeService, IPutTileService putTileService, IMapper mapper)
        {
            _boardService = boardService;
            _tileTypeService = tileTypeService;
            _putTileService = putTileService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetBoard(int id)
        {
            //await _tileTypeService.SeedTileTypes();

            var board = await _boardService.GetBoard(id);

            return Ok(board);
        }

        [HttpGet("tiles")]
        public async Task<IActionResult> GetAllTilesOfBoard(int id)
        {
            //await _tileTypeService.CreateTileType();

            var tiles = await _boardService.GetAllTilesOfBoard(id);

            return Ok(tiles);
        }

        [HttpPost("getNewTile")]
        public async Task<IActionResult> GetNextTile(int id)
        {
            var tileToPut = await _boardService.GetTileToPut(id);

            // return empty tile on game end

            tileToPut.BoardId = id;

            return Ok(tileToPut);

        }


        [HttpPost("put")]
        public async Task<IActionResult> PutTile(int id, TilePuttedDTO tile)
        {
            // TODO unauthorized
            // validate indices

            // validate tile.BoardId = id;

            var tileFromRepo = await _boardService.GetTile(id, tile.X, tile.Y);

            if (tileFromRepo != null)
            {
                return BadRequest("Tile at these coordinates already exists.");
            }

            if (!await _boardService.ValidateTerrain(id, tile))
            {
                return BadRequest("Cannot put here. Terrain not valid.");
            }

            Tile record = _mapper.Map<TilePuttedDTO, Tile>(tile);

            if (await _putTileService.PutTile(record))
            {
                return Ok(); // CreatedAt
            }

            // check components -> connect components



            // TODO global exception handler
            throw new Exception("Putting tile failed on save.");
        }




    }
}
