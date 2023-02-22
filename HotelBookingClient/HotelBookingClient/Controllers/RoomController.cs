using Microsoft.AspNetCore.Mvc;
using HotelBookingClient.Models;
using Microsoft.EntityFrameworkCore;
using HotelBookingClient.PaginatedList;

namespace HotelBookingClient.Controllers
{
    public class RoomController : Controller
    {
        HttpClient client;
        private readonly IConfiguration _configuration;
        public RoomController(IConfiguration configuration) {

            _configuration = configuration;
            var URL = _configuration.GetValue<string>("HOTELAPI");
            client = new HttpClient();
            client.BaseAddress = new Uri(URL);
        }        
      
       

        public  IActionResult  Index(string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber) {
            if (!CheckCookies())
            {
                return View("_Unauthorized");
            }
            HttpResponseMessage message =  client.GetAsync($"Room").Result;
            if (!message.IsSuccessStatusCode)
            {
                return View("_HTTP_ResponseStatus", message);
            }
            IEnumerable<room> rooms = message.Content.ReadAsAsync<IEnumerable<room>>().Result;
            //return View(rooms);
            ViewData["CurrentSort"] = sortOrder;            
            ViewData["PriceSortParm"] = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewData["NumberSortParm"] = sortOrder == "number" ? "number_desc" : "number";
            ViewData["CurrentFilter"] = searchString;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

                IQueryable<room> roomsList = rooms.AsQueryable();
            if (!String.IsNullOrEmpty(searchString))
            {
                roomsList = roomsList.Where(s => s.room_number.ToString().Contains(searchString)
                                       || s.room_status.Contains(searchString)
                                       || s.room_type.Contains(searchString)
                                       || s.room_price.ToString().Contains(searchString)                                      
                                       );               
            }
            
            
            switch (sortOrder)
            {
                case "price_desc":
                    roomsList = roomsList.OrderByDescending(s => s.room_price);
                    break;
                case "number":
                    roomsList = roomsList.OrderBy(s => s.room_number);
                    break;
                case "number_desc":
                    roomsList = roomsList.OrderByDescending(s => s.room_number);
                    break;
                default:
                    roomsList = roomsList.OrderBy(s => s.branch_id);
                    break;
            }
            int pageSize = 10;
            //can't make it asynchronously, because the app don't use EF :)
            var List = PaginatedList<room>.CreateAsync(roomsList, pageNumber ?? 1, pageSize);            
            return View(List);            
        }

        //public  IActionResult VerifyRoomRoom(int room_number, int branch_id) {
        //    if (room_number != 0 && branch_id != 0) {
        //        HttpResponseMessage message =  client.GetAsync($"Room/checknumber/{room_number}/{branch_id}").Result;
        //        if (message.IsSuccessStatusCode)
        //        {
        //            room result = message.Content.ReadAsAsync<room>().Result;
        //            if (result != null)
        //            {
        //                return Json(false);
        //            }
        //            return Json(true);
        //        }
        //    }           
        //    return Json(true);

        //}
        public IActionResult CheckOut(int id, DateTime startdate, DateTime enddate)
        {
            room rm = client.GetAsync("room/" + id).Result.Content.ReadAsAsync<room>().Result;

            resident_room rr = new resident_room()
            {
                start_book_date = startdate,
                end_book_date = enddate,
                room_id = rm.room_id,
            };

            return View(new ReserveRoomData()
            {
                resident_Room = rr,
                room = rm
            });
        }
        public IActionResult ConfirmReserve(ReserveRoomData data)
        {
            data.resident_Room.canceled = false;
            HttpResponseMessage message;
            HttpResponseMessage temp;
            ViewBag.haveDiscount = false;
            decimal totalPrice;
            
            int dayes = data.resident_Room.end_book_date.Subtract(data.resident_Room.start_book_date).Days;
            if (dayes == 0)
            {
                 totalPrice = 1 * data.room.room_price;
            }
            else {
                totalPrice = dayes * data.room.room_price;
            }
               
            
            data.resident_Room.room_total_price = totalPrice;
            data.resident_Room.resident_nationalID = data.resident.nationalID;
            data.resident_Room.room_id = data.room.room_id;
            
           
            //check for discount
            message = client.GetAsync("ResidentRooms/GetByResidentID/" + data.resident.nationalID).Result;

            if (message.IsSuccessStatusCode)
            {
                IEnumerable<resident_room> residentRooms = message.Content.ReadAsAsync<IEnumerable<resident_room>>().Result;
                if (residentRooms != null && residentRooms.Count() > 0)
                {
                    decimal dicountValue = data.resident_Room.room_total_price * Convert.ToDecimal(0.95);
                    data.resident_Room.room_total_price -= dicountValue;
                    ViewBag.haveDiscount = true;
                }
            }
            else if (message.StatusCode != System.Net.HttpStatusCode.NotFound)
            {

                return View("_HTTP_ResponseStatus", message);
            }

            //reserve room
            message = client.PutAsJsonAsync($"Room/reserve/{data.room.room_id}/{data.room.room_residents_count}", data).Result;
            if (!message.IsSuccessStatusCode)
            {
                return View("_HTTP_ResponseStatus", message);
            }

            //check if user exists
            message = client.GetAsync($"Resident/{data.resident.nationalID}").Result;
            if (!message.IsSuccessStatusCode)
            {
                //create user
                message = client.PostAsJsonAsync($"Resident", data.resident).Result;
                temp = message;
                if (!message.IsSuccessStatusCode)
                {   //cancel the reservation if there's error                    
                    message = client.PutAsJsonAsync($"Room/cancelreserve/{data.room.room_id}", data).Result;
                    return View("_HTTP_ResponseStatus", temp);
                }
            }

            //reserve resident_rooms

            message = client.PostAsJsonAsync($"ResidentRooms", data.resident_Room).Result;
            if (!message.IsSuccessStatusCode)
            {
                //cancel the reservation if there's error
                //no need to delete the user if created in the previous step, w'll keep him 😊 for next reservations
                message = client.PutAsJsonAsync($"Room/cancelreserve/{data.room.room_id}", data).Result;
                return View("_HTTP_ResponseStatus", message);
            }
            return View(data);

        }
        public IActionResult CancelReservce(int id)
        {
            room rm = client.GetAsync("room/" + id).Result.Content.ReadAsAsync<room>().Result;
            return View(rm);
        }
        
        [HttpGet]
        public IActionResult Create() {
            if (!CheckCookies())
            {
                return View("_Unauthorized");
            }
            IEnumerable<branch> Results;

            HttpResponseMessage result = client.GetAsync($"Branche").Result;
            if (result.IsSuccessStatusCode)
            {
                Results = result.Content.ReadAsAsync<IEnumerable<branch>>().Result;
                ViewBag.branches = Results;
                return View();
            }
            else
            {
                return View("_HTTP_ResponseStatus", result);
            }            
        }

        [HttpPost]
        public IActionResult Create(room rm)
        {           
                HttpResponseMessage message = client.PostAsJsonAsync($"Room", rm).Result;
                if (!message.IsSuccessStatusCode)
                {
                    return View("_HTTP_ResponseStatus", message);
                }
                return RedirectToAction("Index", "Room");                               
        }
        
        [HttpGet]
        public IActionResult Delete(room rm) {
            if (!CheckCookies())
            {
                return View("_Unauthorized");
            }
            return View(rm);                      
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            HttpResponseMessage message = client.DeleteAsync($"Room/{id}").Result;
            if (!message.IsSuccessStatusCode)
            {
                return View("_HTTP_ResponseStatus", message);
            }
            return RedirectToAction("Index", "Room");
        }

        public bool CheckCookies() {
            string role;
            if (HttpContext.Request.Cookies.TryGetValue("user_role", out role)) {
                if (role == "admin")
                {
                    return true;
                }
                else {

                    return false;
                }

                
            }
            return false;
        }
    }
}
