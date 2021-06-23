using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
//using OdeToFood.Core;
using OdeToFood.Data;

namespace OdeToFood_v5.Pages.Restaurants
{
    public class EditModel : PageModel
    {
        private readonly IRestaurantData restaurantData;
        private readonly IHtmlHelper htmlHelper;

        [BindProperty]
        public Restaurant Restaurant { get; set; }
        public IEnumerable<SelectListItem> Cuisines { get; set; }
        public EditModel(IRestaurantData restaurantData,
            IHtmlHelper htmlHelper)
        {
            this.restaurantData = restaurantData;
            this.htmlHelper = htmlHelper;
        }
        public IActionResult OnGet(int? restaurantId)
        {
            Cuisines = htmlHelper.GetEnumSelectList<CuisineType>();
            // restaurantId.HasValue is only available if we use int?
            if (restaurantId.HasValue)
            {
                Restaurant = restaurantData.GetById(restaurantId.Value);
            }
            else
            {
                Restaurant = new Restaurant();
            }
            if(Restaurant == null)
            {
                return RedirectToPage("./NotFound");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            // In OnPost do model validation, you can never
            // trust input fromm the user
            // if(){} is the hard way
            // Linked with the Restaurant definition is ModelState
            // ModelState Razor page variable
            if (!ModelState.IsValid)
            {
                // Cuisines has to be updated during a Post request also
                // Not only in a Get request
                Cuisines = htmlHelper.GetEnumSelectList<CuisineType>();
                return Page();//render the page
            }
            if (Restaurant.Id > 0)
            {
                // Restaurant gets updated thanks to model updating
                // Restaurant = restaurantData.Update(Restaurant);
                restaurantData.Update(Restaurant);
            }
            else
            {
                restaurantData.Add(Restaurant);
            }
            restaurantData.Commit();
            //    If we refresh the paga we get a warning that the action we
            //    just dit will be repeated.If we saved restaurant data it will be
            //    saved again if we had a credit card order the order will be billed twice
            //    On a Page with Post request user must be redirected after
            //    the post request has been done. Post requests modify data on the server.
            //    We should redirect user to Detail request to let himi see his modifications.
            //    The Detail page uses Get request which is save
            // we create a new object with new for RedirectToPage

            // ASP.NET has TempData so that if the user bookmarks the Detail page after
            // the page loads with "Restaurant saved!" string it will be shown only one time
            // if the page is loaded from bookmarks it won't appear.
            // We can use 
            //@if(TempData["Message"])
            // in Detail.cshtml or we can bind TempData["Message"] in Detail.cshtml.cs
            TempData["Message"] = "Restaurant saved!";

            return RedirectToPage("./Detail", new { restaurantId = Restaurant.Id });
            // Now when we refresh we refresh a get request


        }
    }
}