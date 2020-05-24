using CarcassoneAPI.Data;
using CarcassoneAPI.Models;
using CarcassoneAPI.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassoneAPI.Services
{
    public class TileTypeService : ITileTypeService
    {
        private readonly DataContext _context;

        public TileTypeService(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateTileType()
        {
            var types = TileTypesSeed.GetTileTypes();

            _context.TileTypes.AddRange(types);

            return await _context.SaveChangesAsync() > 0;
        }



        public async  Task<TileType> GetFirst()
        {
            return await _context.TileTypes
                .Include(t => t.Terrains)
                .Include(t => t.Components)
                .FirstOrDefaultAsync();
        }
    }
}
