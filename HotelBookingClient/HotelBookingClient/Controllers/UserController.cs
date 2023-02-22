using HotelBookingClient.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingClient.Controllers
{
    public class UserController : Controller
    {

        HttpClient client;
        private readonly IConfiguration _configuration;
        public UserController(IConfiguration configuration)
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
            HttpResponseMessage message = client.GetAsync("User").Result;
            if (message.IsSuccessStatusCode)
            {
                var result = message.Content.ReadAsAsync<IEnumerable<user>>().Result;
                return View(result);
            }
            return View("_HTTP_ResponseStatus",message);
           
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
        public ActionResult Create(user user)
        {
            HttpResponseMessage message = client.PostAsJsonAsync("User", user).Result;
            if (!message.IsSuccessStatusCode)
            {
                return View("_HTTP_ResponseStatus", message);
            }
            return RedirectToAction("Index");
        }
       

        public ActionResult Delete(user usr)
        {
            if (!CheckCookies())
            {
                return View("_Unauthorized");
            }
            return View(usr);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            HttpResponseMessage message = client.DeleteAsync($"User/{id}").Result;
            if (!message.IsSuccessStatusCode)
            {
                return View("_HTTP_ResponseStatus", message);
            }
            return RedirectToAction("Index");
        }

        public ActionResult VerifyUserName(string userName) {

            HttpResponseMessage message = client.GetAsync("User").Result;
            if (!message.IsSuccessStatusCode)
            {
                return View("_HTTP_ResponseStatus", message);
            }
            else {
                var users = message.Content.ReadAsAsync<IEnumerable<user>>().Result;
                var ishere = users.Where(x => x.userName == userName).FirstOrDefault();
                if (ishere != null)
                {
                    return Json(false);
                }
                else
                {
                    return Json(true);
                }
            }   
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
