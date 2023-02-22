using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingDAL.Models
{
    public class ReservationData
    {
        public resident resident { get; set; }
        public room room { get; set; }
        public branch branch { get; set; }
        public resident_room resident_Room { get; set; }
    }
}
