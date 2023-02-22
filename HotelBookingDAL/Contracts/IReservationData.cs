using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingDAL.Contracts
{
    public interface IReservationData <T>
    {
        public Task<T> GetAllData(int id);
    }
}
