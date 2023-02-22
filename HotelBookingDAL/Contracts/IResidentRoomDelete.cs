using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingDAL.Contracts
{
    public interface IResidentRoomDelete <T>
    {
        public Task<T> DeleteByUsrIDAndRomID(int USR_id, int RomID);
    }
}
