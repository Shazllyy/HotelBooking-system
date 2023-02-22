using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingDAL.Contracts
{
    public interface ILogin <T>
    {
        public Task<T> Login(string userName, string passWord);
    }
}
