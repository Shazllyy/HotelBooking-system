using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingClient.Models
{
    public class ReserveRoomData
    {
                
        public resident_room resident_Room { get; set; }
        public resident resident { get; set; }        
        public room room { get; set; }
    }
}
