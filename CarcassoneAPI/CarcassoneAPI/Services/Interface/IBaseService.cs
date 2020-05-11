using System.Threading.Tasks;

namespace CarcassoneAPI.Services.Interface
{
    public interface IBaseService
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
    }
}
