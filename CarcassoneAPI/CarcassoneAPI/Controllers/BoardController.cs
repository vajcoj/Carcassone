using CarcassoneAPI.Helpers;
using CarcassoneAPI.Models;
using CarcassoneAPI.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Drawing;
using System.Threading.Tasks;

namespace CarcassoneAPI.Controllers
{
    [ApiController]
    [Route("[controller]/{id}")]
    public class BoardController : ControllerBase
    {
        private readonly IBoardService _boardService;

        public BoardController(IBoardService boardService)
        {
            _boardService = boardService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTiles(int id)
        {
            var tiles = await _boardService.GetAllTilesOfBoard(id);
            return Ok(tiles);
        }

        [HttpPost("put")]
        public async Task<IActionResult> PutTile(int id, Tile tile)
        {
            // TODO unauthorized

            tile.Color = ColorsHelper.GetRandomKnownColor();
            tile.BoardId = id;

            var tileFromRepo = await _boardService.GetTile(id, tile.X, tile.Y);

            if (tileFromRepo != null)
            {
                return BadRequest("Tile at these coordinates already exists.");
            }

            if (await _boardService.PutTile(tile))
            {
                return Ok(tile.Color);
            }

            // TODO global exception handler
            throw new Exception("Putting tile failed on save.");
        }




    }
}
