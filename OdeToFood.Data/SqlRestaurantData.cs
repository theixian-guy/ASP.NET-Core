using Microsoft.EntityFrameworkCore;
//using OdeToFood.Core;
using System.Collections.Generic;
using System.Linq;

namespace OdeToFood.Data
{
    public class SqlRestaurantData : IRestaurantData
    {
        private readonly MySQLDbContext db;

        // After creating a db field with this db value. And the beauty of all this is 
        // through the magic of ConfigureServices, ASP.NET COre is going to come across some
        // component or some Razor Page that needs something that implements IRestaurantData.
        // We're going to configure in SqlIRestaurantData to be injected for IRestaurantData.
        // And ASP.NET COre is going to see oh, the SqlRestaurantData needs a DbContext.
        // Let me work with the Entity Framework to grab one of those and pass that into the constructor.
        // So there's nothing else I have to do to receive a DbContext.
        public SqlRestaurantData(MySQLDbContext db)
        {
            this.db = db;
        }
        //      in Startup.cs put 
        //      services.AddScoped<IRestaurantData, SqlRestaurantData>();
        //        public void ConfigureServices(IServiceCollection services)
        //        {
        //            services.AddDbContextPool<OdeToFoodDbContext>(options =>
        //            {
        //                options.UseSqlServer(Configuration.
        //                    GetConnectionString("OdeToFoodDb"));
        //            }
        //            );
        //
        //            // use a SqlRestaurantData whenever you need a  IRestaurantData
        //            // every time the same SqlRestaurantData each time
        //            services.AddScoped<IRestaurantData, SqlRestaurantData>();

        // Using MySQL INSERT
        public Restaurant Add(Restaurant newRestaurant)
        {
            // get max(Id) from database table 
            var maxId = MaxId(); 
            newRestaurant.Id = maxId + 1;

            db.Add(newRestaurant);
            return newRestaurant;
        }

        private int MaxId()
        {
            return db.MaxId();
        }

        public int Commit()
        {
            // return db.SaveChanges();
            return 1;
        }

        // Using MySQL DELETE
        public Restaurant Delete(int id)
        {
            List<Restaurant> restaurants = db.Restaurants;
           
            var restaurantDb = restaurants.Where(i => i.Id == id).First();
            var restaurant = GetById(id);
            
            // Mark restaurant for deletion
            restaurant.Cuisine = CuisineType.RestaurantDoesntExist;
            var index = restaurants.IndexOf(restaurantDb);

            if (index != -1)
                // Write to list the restaurant to be deleted
                restaurants[index] = restaurant;
            db.Restaurants = restaurants;

            // if (restaurant != null)
            // {
            //     db.Restaurants = .Remove(restaurant);
            // }
            return restaurant;
        }

        public Restaurant GetById(int id)
        {
            // Find tracks down the entity inside of the database.
            // return db.GetById(id);
            return db.GetById(id);
        }

        public int GetCountOfRestaurants()
        {
            return db.Restaurants.Count();
        }

        public IEnumerable<Restaurant> GetRestaurantsByName(string name)
        {
            // Must include System.Linq for from where ... to work
            var query = from r in db.Restaurants
                        // if string.IsNullOrEmpty(name) is true it will return all restaurants
                        where string.IsNullOrEmpty(name) || r.Name.StartsWith(name)
                        orderby r.Name
                        select r;
            return query;
        }

        /// <summary>
        /// Using MySQL UPDATE
        /// Find restaurant in database using the ID field from the updatedRestaurant
        /// and write to that row.
        /// </summary>
        /// <param name="updatedRestaurant"></param>
        /// <returns></returns>
        public Restaurant Update(Restaurant updatedRestaurant)
        {
            // entity is for database management, I think
            //var entity = db.Restaurants.Attach(updatedRestaurant);
            //entity.State = EntityState.Modified;

            var restaurants = db.Restaurants;

            var restaurantDb = restaurants.Where(i => i.Id == updatedRestaurant.Id).First();
            //var restaurant = GetById(updatedRestaurant.Id);
            var index = restaurants.IndexOf(restaurantDb);
            if (index != -1)
            {
                restaurants[index] = updatedRestaurant;
                db.Restaurants = restaurants;
            }


            return updatedRestaurant;
        }
    }
}
