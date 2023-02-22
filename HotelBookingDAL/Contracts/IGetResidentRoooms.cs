using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingDAL.Contracts
{
    public interface IGetResidentRoooms <T>
    {
        public Task<IEnumerable<T>> GetResidentRooms(long residentId);
    }
}
