using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingDAL.Contracts
{
    public interface IRoomCancelReserve <T>
    {
        public Task<T> cancelBooking(int RoomId);
    }
}
