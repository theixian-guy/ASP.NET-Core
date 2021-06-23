using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
//using OdeToFood.Core;
using OdeToFood.Data;

namespace OdeToFood_v5.Pages.Restaurants
{
    public class DetailModel : PageModel
    {
        private readonly IRestaurantData restaurantData;


        // ASP.NET has TempData so that if the user bookmarks the Detail page after
        // the page loads with "Restaurant saved!" string it will be shown only one time
        // if the page is loaded from bookmarks it won't appear.
        // We can use 
        //@if(TempData["Message"])
        // in Detail.cshtml or we can bind TempData["Message"] in Detail.cshtml.cs
        [TempData]
        public string Message { get; set; }

        public Restaurant Restaurant { get; set; }
        public DetailModel(IRestaurantData restaurantData)
        {
            this.restaurantData = restaurantData;
        }
        public IActionResult OnGet(int restaurantId)
        {
            Restaurant = restaurantData.GetById(restaurantId);
            if(Restaurant == null)
            {
                return RedirectToPage("./NotFound");
            }
            return Page();
        }
    }
}