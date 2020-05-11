using CarcassoneAPI.Data;
using CarcassoneAPI.Models;
using CarcassoneAPI.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassoneAPI.Services
{
    public class BoardService : IBoardService
    {
        private readonly DataContext _context;

        public BoardService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tile>> GetAllTilesOfBoard(int idBoard)
        {
            return await _context.Tiles.Where(w => w.Board.Id == idBoard).ToListAsync();   
        }

    }
}
