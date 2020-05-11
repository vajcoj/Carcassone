using CarcassoneAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassoneAPI.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Tile> Tiles { get; set; }
        public DbSet<Board> Boards { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
    }
}
