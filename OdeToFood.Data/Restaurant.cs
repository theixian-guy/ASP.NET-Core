using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OdeToFood.Data
{
    public class Restaurant 
    {
        private MySQLDbContext context;
        public int Id { get; set; }

        // Name can't be blank
        [Required, StringLength(80)]
        public string Name { get; set; }
        [Required, StringLength(255)]
        public string Location { get; set; }
        public CuisineType Cuisine { get; set; }

    }
}
