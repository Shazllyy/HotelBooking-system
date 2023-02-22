using HotelBookingClient.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingClient.Controllers
{
    public class BranchController : Controller
    {

        HttpClient client;
        private readonly IConfiguration _configuration;
        public BranchController(IConfiguration configuration)
        {

            _configuration = configuration;
            var URL = _configuration.GetValue<string>("HOTELAPI");
            client = new HttpClient();            
            client.BaseAddress = new Uri(URL);
        }


        public ActionResult Index()
        {
            if (!CheckCookies())
            {
                return View("_Unauthorized");
            }

            HttpResponseMessage message = client.GetAsync("Branche").Result;
            if (message.IsSuccessStatusCode)
            {
                var branches = message.Content.ReadAsAsync<IEnumerable<branch>>().Result;
                return View(branches);
            }
            else {
                return View("_HTTP_ResponseStatus",message);
            }
            
        }
        

        public ActionResult Create()
        {
            if (!CheckCookies())
            {
                return View("_Unauthorized");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(branch branch)
        {
            HttpResponseMessage message = client.PostAsJsonAsync($"Branche", branch).Result;
            if (!message.IsSuccessStatusCode)
            {
                return View("_HTTP_ResponseStatus", message);
            }
            return RedirectToAction("Index", "Branch");
        }

        [HttpGet]
        public ActionResult Edit(branch branch)
        {
            if (!CheckCookies())
            {
                return RedirectToAction(actionName: "Login", controllerName: "Home");
            }
            return View(branch);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(branch branch, int id)
        {
            HttpResponseMessage message = message = client.PutAsJsonAsync($"Branche/{branch.branch_id}", branch).Result;
            if (!message.IsSuccessStatusCode)
            {
                return View("_HTTP_ResponseStatus", message);
            }
            return RedirectToAction("Index", "Branch");
        }

        [HttpGet]
        public ActionResult Delete(branch branch)
        {
            if (!CheckCookies())
            {
                return View("_Unauthorized");
            }
            return View(branch);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            HttpResponseMessage message = client.DeleteAsync($"Branche/{id}").Result;
            if (!message.IsSuccessStatusCode)
            {
                return View("_HTTP_ResponseStatus", message);
            }
            return RedirectToAction("Index", "Branch");
        }

        public bool CheckCookies()
        {
            string role;
            if (HttpContext.Request.Cookies.TryGetValue("user_role", out role))
            {
                if (role == "admin")
                {
                    return true;
                }
                else
                {

                    return false;
                }


            }
            return false;
        }
    }
}
