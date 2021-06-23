using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
//using OdeToFood.Core;
using OdeToFood.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OdeToFood_v5.Pages.Restaurants
{
    public class ListModel : PageModel
    {
        public string Message { get; set; }
        private readonly IConfiguration config;
        private readonly IRestaurantData restaurantData;
        private readonly ILogger<ListModel> logger;

        public IEnumerable<Restaurant> Restaurants { get; set; }
        [BindProperty(SupportsGet =true)]
        public string SearchTerm { get; set; }

        public ListModel(IConfiguration config, 
            IRestaurantData restaurantData,
            // logs custom messages given by developer
            ILogger<ListModel> logger)
        {
            this.config = config;// Save IConfig to a private field
            this.restaurantData = restaurantData;
            this.logger = logger;
        }
        // 'Restaurants' is a Model variable. We can call it in the *.cshtml
        // file. It is public. When we refresh the Restaurants/List address
        // a get with no parameters is sent so searchTerm = null. Look at the 
        // html in the List.cshtml.cs. The input tag gets a name="searchTerm"
        // field the searchTerm from name="searchTerm" must match the searchTerm
        // from the List.cshtml.cs file.
        // If it were int searchTerm it would get a null but since int is a value type 
        // it would throw an exception because int can't be null.
        // Continuing... searchTerm is passed on a parameter to 
        // restaurantData.GetRestaurantsByName(searchTerm) and since searchTerm
        // is null it will return all restaurants form the LINQ query inside this
        // method.
        // When the form magnifying glass button is pressed the searchTerm gets
        // the string entered in the form and is passed on to the restaurantsData's
        // method which with the LINQ query will return the restaurants that begin
        // with the string entered in the form.
        public void OnGet()
        {
            // Example of logging an ERROR
            logger.LogError("Executing ListModel");
            Message = config["Message"];// indexing like in a json variable
            Restaurants = restaurantData.GetRestaurantsByName(SearchTerm);
        }
    }
}
