using HotelBookingClient.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingClient.Controllers
{
    public class ReservationController : Controller
    {
        HttpClient client;
        private readonly IConfiguration _configuration;
        public ReservationController(IConfiguration configuration)
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
                return RedirectToAction(actionName: "Login", controllerName: "Home");
            }
            HttpResponseMessage message = client.GetAsync("ResidentRooms").Result;
            if (message.IsSuccessStatusCode)
            {
                var data = message.Content.ReadAsAsync<IEnumerable<resident_room>>().Result;
                return View(data);
            }
            else {
                return View("_HTTP_ResponseStatus",message);
            }
            
        }

      
        public ActionResult Details(resident_room rsndt)
        {
            if (!CheckCookies()) {
                return RedirectToAction(actionName: "Login", controllerName: "Home");
            }

            HttpResponseMessage message = client.GetAsync($"ReservationData/GetReservationData/{rsndt.reservation_ID}").Result;
            if (message.IsSuccessStatusCode) {
                ReservationData reservationData = message.Content.ReadAsAsync<ReservationData>().Result;
                return View(reservationData);
            }
            return View("_HTTP_ResponseStatus", message);
            
        }
       
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

      

       
        public ActionResult Delete(resident_room resident_Room)
        {
            if (!CheckCookies())
            {
                return RedirectToAction(actionName: "Login", controllerName: "Home");
            }
            return View(resident_Room);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            HttpResponseMessage message = client.DeleteAsync($"ResidentRooms/{id}").Result;
            if (!message.IsSuccessStatusCode) {
                return View("_HTTP_ResponseStatus", message);
            }
            return RedirectToAction("Index");
        }

        //id=> room id
        [HttpGet]
        public ActionResult Cancel(resident_room resident_Room) {
       
            //cancel room , update status to available and residents to zero
            HttpResponseMessage message = client.PutAsync($"Room/cancelreserve/{resident_Room.room_id}", null).Result;
            if (!message.IsSuccessStatusCode)
            {                
                return View("_HTTP_ResponseStatus", message);
            }
            //update resident room canceled to true and isactive to false
            resident_Room.canceled = true;
            resident_Room.is_active = false;
            message = client.PutAsJsonAsync($"ResidentRooms/{resident_Room.reservation_ID}", resident_Room).Result;
            if (!message.IsSuccessStatusCode) {
                return View("_HTTP_ResponseStatus", message);
            }
            return RedirectToAction("Index");

        }

        [HttpGet]
        public ActionResult CheckOut(resident_room resident_Room)
        {
            //update resident room isactive to false and update chekoutdate to today date
            resident_Room.is_active = false;
            resident_Room.checkout_date = DateTime.Now;
            HttpResponseMessage message = client.PutAsJsonAsync($"ResidentRooms/{resident_Room.reservation_ID}", resident_Room).Result;
            if (!message.IsSuccessStatusCode)
            {
                return View("_HTTP_ResponseStatus", message);
            }
            return RedirectToAction("Index");
        }


        public bool CheckCookies()
        {
            return HttpContext.Request.Cookies.TryGetValue("user_role", out string userName);
        }
    }
}
