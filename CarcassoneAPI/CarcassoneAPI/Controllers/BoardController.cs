using CarcassoneAPI.Services.Interface;
using Microsoft.AspNetCore.Mvc;
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


    }
}
