using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingDAL.Contracts
{
    public interface ICheckRoomNumbers <T>
    {
        public Task<T> CheckRoomNumbers(int roomNumber, int branch_id);
    }
}
