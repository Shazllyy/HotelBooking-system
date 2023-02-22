using HotelBookingClient.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingClient.Controllers
{
    public class ResidentController : Controller
    {


        HttpClient client;
        private readonly IConfiguration _configuration;
        public ResidentController(IConfiguration configuration)
        {
            _configuration = configuration;
            var URL = _configuration.GetValue<string>("HOTELAPI");
            client = new HttpClient();
            client.BaseAddress = new Uri(URL);
        }

        // GET: ResidentController
        public ActionResult Index()
        {
            if (!CheckCookies())
            {
                return View("_Unauthorized");
            }
            HttpResponseMessage message = client.GetAsync("Resident").Result;
            if (!message.IsSuccessStatusCode)
            {
                return View("_HTTP_ResponseStatus", message);              
            }
            var data = message.Content.ReadAsAsync<IEnumerable<resident>>().Result;
            return View(data);            

        }

      

        // GET: ResidentController/Create
        public ActionResult Create()
        {
            if (!CheckCookies())
            {
                return View("_Unauthorized");
            }
            return View();
        }

        // POST: ResidentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(resident resident)
        {
            HttpResponseMessage message = client.PostAsJsonAsync("Resident", resident).Result;

            if (!message.IsSuccessStatusCode) {
                return View("_HTTP_ResponseStatus", message);            
            }
            return RedirectToAction("Index");
        }
       

        // GET: ResidentController/Delete/5
        public ActionResult Delete(resident resident)
        {
            if (!CheckCookies())
            {
                return View("_Unauthorized");
            }
            return View(resident);
        }

        // POST: ResidentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(long id)
        {
            HttpResponseMessage message = client.DeleteAsync($"Resident/{id}").Result;
            if (!message.IsSuccessStatusCode)
            {
                return View("_HTTP_ResponseStatus", message);
            }
            return RedirectToAction("Index");
        }



        public ActionResult CheckResidentID(long nationalID) {
            HttpResponseMessage message = client.GetAsync("Resident").Result;
            if (!message.IsSuccessStatusCode)
            {
                return View("_HTTP_ResponseStatus", message);
            }
            var data = message.Content.ReadAsAsync<IEnumerable<resident>>().Result;
            var ishere = data.Where(x => x.nationalID == nationalID).FirstOrDefault();
            if (ishere != null)
            {
                return Json(false);
            }
            return Json(true);

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
