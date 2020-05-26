using CarcassoneAPI.Data;
using CarcassoneAPI.Services.Interface;
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

        public async Task<bool> SeedTileTypes()
        {
            var types = TileTypesSeed.GetTileTypes();

            _context.TileTypes.AddRange(types);

            return await _context.SaveChangesAsync() > 0;
        }

    }
}
