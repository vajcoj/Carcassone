using CarcassoneAPI.Data;
using CarcassoneAPI.Services.Interface;
using System.Threading.Tasks;

namespace CarcassoneAPI.Services
{
    public abstract class BaseService : IBaseService
    {
        protected internal readonly DataContext _context;

        public BaseService(DataContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }
        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
