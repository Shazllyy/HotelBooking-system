using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingDAL.Contracts
{
    public interface IRoomReserve <T>
    {       
        public Task<T> bookRoom(int roomId,int residentCount);                    
    }
}
