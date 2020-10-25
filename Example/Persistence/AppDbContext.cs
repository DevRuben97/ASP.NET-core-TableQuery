using Example.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Example.Persistence
{
    public class AppDbContext: DbContext
    {

        public DbSet<Product> Products { get; set; }


        public AppDbContext(DbContextOptions options): base(options)
        {

        }
    }
}
