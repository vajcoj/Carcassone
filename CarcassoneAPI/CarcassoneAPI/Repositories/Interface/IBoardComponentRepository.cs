using CarcassoneAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarcassoneAPI.Repositories.Interface
{
    public interface IBoardComponentRepository : IRepository<BoardComponent>
    {
        public Task<List<BoardComponent>> GetOpenMonasteries(int boardId);
    }
}
