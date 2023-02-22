using HotelBookingClient.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace HotelBookingClient.Controllers
{
    public class HomeController : Controller
    {       
        HttpClient client;
        private readonly IConfiguration _configuration;
        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
            var URL = _configuration.GetValue<string>("HOTELAPI");            
            client = new HttpClient();
            client.BaseAddress = new Uri(URL);
        }
        public IActionResult Login()
        {
            bool ishere = HttpContext.Request.Cookies.TryGetValue("user_role", out string userName);
            if (ishere)
            {
                return RedirectToAction(actionName: "SearchView");
            }
            return View(new user());
        }
        public IActionResult Checklogin(user usr) {                        
                HttpResponseMessage result = client.GetAsync($"User/login/{usr.userName}/{usr.userPassword}").Result;
                if (result.IsSuccessStatusCode)
                {
                    user user = result.Content.ReadAsAsync<user>().Result;                    
                    HttpContext.Response.Cookies.Append("user_role", user.role);
                    return RedirectToAction(actionName: "SearchView");
                }                           
            ViewBag.falseData = true;
            return View("Login");           
        }
        public IActionResult SearchView()
        {

            bool ishere = HttpContext.Request.Cookies.TryGetValue("user_role", out string userName);
            if (ishere)
            {
                IEnumerable<branch> Results;
               
                HttpResponseMessage result = client.GetAsync($"Branche").Result;
                if (result.IsSuccessStatusCode)
                {
                    Results = result.Content.ReadAsAsync<IEnumerable<branch>>().Result;
                    ViewBag.branches = Results;
                    return View(new searchData());
                }
                else {
                    return View("_HTTP_ResponseStatus", result); 
                }


            }

            return RedirectToAction("Login");



        }        
        public  IActionResult SearchRooms(searchData searchData) {
            if (ModelState.IsValid)
            {
                HttpResponseMessage result = client.GetAsync($"Room/getavailable?startDate={searchData.startDate}&endDate={searchData.endDate}&branch_id={searchData.branch_id}").Result;
                if (result.IsSuccessStatusCode)
                {
                    IEnumerable<room> rooms = result.Content.ReadAsAsync<IEnumerable<room>>().Result;

                    TempData["startdate"] = searchData.startDate;
                    TempData["enddate"] = searchData.endDate;

                    return View("SearchRoomsResult", rooms);
                }
                else {
                    return View("_HTTP_ResponseStatus", result);
                }                
            }           
             return RedirectToAction(actionName: "SearchView");                        
        }      
        public IActionResult Privacy()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult UnderConstruction() {
            return View("_UnderConstruction");
        }
        public IActionResult Logout() {
            HttpContext.Response.Cookies.Delete("user_role");
            return RedirectToAction("Login");
        }

        
    }
}

