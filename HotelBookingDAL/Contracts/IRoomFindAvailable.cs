using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingDAL.Contracts
{
    public interface IRoomFindAvailable<T>
    {
        //public Task<T> getAllBookingsForRoom(int roomId);
        //public Task<T> getAllBookingsForCustomer(int customerId);
        //public Task<T> getAllBookingsForDate(DateTime date);
        //public Task<T> getAllBookingsForDateRange(DateTime startDate, DateTime endDate);        
        public Task<IEnumerable<T>> getAvailableRooms(DateTime startDate, DateTime endDate, int branch_id);
    }
}
