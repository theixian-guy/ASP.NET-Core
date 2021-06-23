using MySql.Data.MySqlClient;
using Microsoft.EntityFrameworkCore;
//using OdeToFood.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace OdeToFood.Data
{
    public class MySQLDbContext  //O.R.M.
    {
        public string ConnectionString { get; set; }

        public MySQLDbContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        /// <summary>
        /// This is a property that returns all restaurants and sets all restaurants
        /// 
        /// </summary>
        public List<Restaurant> Restaurants { 
            get
            {
                return GetAllRestaurants();
            } 
            set
            {
                UpdateAllRestaurants(value); 
            }
        }

        void UpdateAllRestaurants(List<Restaurant> updatedRestaurants)
        {

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                foreach (var r in updatedRestaurants)
                {
                    if (r.Cuisine == CuisineType.RestaurantDoesntExist)
                    {
                        Delete(conn, r);
                    }
                    // does 'r' restaurant exist in the MySQL Database table?
                    else
                    {
                        // Check if r.id exists in the ID column of the Restaurant table
                        // Get id
                        MySqlCommand cmdRead = new MySqlCommand(
                            $"SELECT * FROM Restaurants WHERE ID = {r.Id} LIMIT 1", conn);
                        using (var reader = cmdRead.ExecuteReader())
                        {
                            // if we have one element returned by SELECT then r exists in table
                            // do an UPDATE
                            if(reader.Read())
                            {
                                Update(r);
                            }
                            else // do an INSERT
                            {
                                Insert(r);
                            }
                        }

                    }
                }
            }
        }

        private static void Delete(MySqlConnection conn, Restaurant r)
        {
            MySqlCommand cmd = new MySqlCommand($"DELETE FROM Restaurants WHERE " +
                $" ID = {r.Id}", conn);
            cmd.ExecuteNonQuery();
        }

        private void Update(Restaurant r)
        {
            using (MySqlConnection connUpdate = GetConnection())
            {
                connUpdate.Open();
                MySqlCommand cmdUpdate = new MySqlCommand(
                    $"UPDATE Restaurants SET Name = \"{r.Name}\", " +
                    $"Location = '{r.Location}', " +
                    $"Cuisine = '{Convert.ToInt32(r.Cuisine)}' " +
                    $"WHERE ID = {r.Id}", connUpdate);
                cmdUpdate.ExecuteNonQuery();
            }
        }

        private void Insert(Restaurant r)
        {
            using (MySqlConnection connInsert = GetConnection())
            {
                connInsert.Open();
                MySqlCommand cmdInsert = new MySqlCommand(
                    $"INSERT INTO Restaurants VALUES " +
                    $"({r.Id}, " +
                    $"\"{r.Name}\", " +
                    $"'{r.Location}', " +
                    $"'{Convert.ToInt32(r.Cuisine)}')", connInsert);
                cmdInsert.ExecuteNonQuery();
            }
        }

        public void Add(Restaurant newRestaurant)
        {
            var Restaurants = GetAllRestaurants();
            Restaurants.Add(newRestaurant);
            this.Restaurants = Restaurants;
        }

        public int MaxId()
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand($"SELECT max(Id) AS maxId FROM Restaurants LIMIT 1", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return Convert.ToInt32(reader["maxId"]);
                    }
                }
            }
            return -1;
        }

        public List<Restaurant> GetAllRestaurants()
        {
            List<Restaurant> list = new List<Restaurant>();

            // SQL very useful code
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from Restaurants", conn);
                
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Restaurant()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = reader["Name"].ToString(),
                            Location = reader["Location"].ToString(),
                            Cuisine = (CuisineType)(Convert.ToInt32(reader["Cuisine"]))
                        });
                    }
                }
            }
            return list;
        }

        public Restaurant GetById(int id)
        {
            // It should be only one Restaurant returned 
            Restaurant res = new Restaurant();
        
            // SQL very useful code
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand($"select * from Restaurants" + 
                    $" WHERE Id={id} LIMIT 1", conn);
                // cmd.ExecuteNonQuery //pt INSERT, UPDATE, DELETE
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        res = new Restaurant()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = reader["Name"].ToString(),
                            Location = reader["Location"].ToString(),
                            Cuisine = (CuisineType)(Convert.ToInt32(reader["Cuisine"]))
                        };
                    }
                    
                }
            }
            return res;
        }

    }
}
