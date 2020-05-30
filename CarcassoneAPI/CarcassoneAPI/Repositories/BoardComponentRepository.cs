using CarcassoneAPI.Data;
using CarcassoneAPI.Models;
using CarcassoneAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassoneAPI.Repositories
{
    public class BoardComponentRepository : Repository<BoardComponent>, IBoardComponentRepository
    {
        public DataContext DataContext => _context as DataContext;

        public BoardComponentRepository(DataContext context) : base(context)
        {

        }
        public override BoardComponent Get(int id)
        {
            var bc = _entities
                .Include(bc => bc.Components)
                .SingleOrDefault(bc => bc.BoardComponentId == id && !bc.IsDeleted);

            return bc;
        }

        public override void Remove(BoardComponent component)
        {
            component.Components = null;
            component.IsDeleted = true;
            //_entities.Update(component);
        }
    }
}
