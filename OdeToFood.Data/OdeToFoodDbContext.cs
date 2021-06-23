using Microsoft.EntityFrameworkCore;
//using OdeToFood.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace OdeToFood.Data
{
    public class OdeToFoodDbContext : DbContext //O.R.M.
    {
        public OdeToFoodDbContext(
            DbContextOptions<OdeToFoodDbContext> options)
            : base(options)
        {

        }
        public DbSet<Restaurant> Restaurants { get; set; }
    }
}
