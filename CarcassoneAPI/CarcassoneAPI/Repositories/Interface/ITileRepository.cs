﻿using CarcassoneAPI.Models;
using System.Threading.Tasks;

namespace CarcassoneAPI.Repositories.Interface
{
    public interface ITileRepository : IRepository<Tile>
    {
        Task<Tile> GetTileAt(int boardId, int x, int y);
    }
}
