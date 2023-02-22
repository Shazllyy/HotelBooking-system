using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingDAL.Contracts
{
    public interface IResident <T>
    {
        public Task<T> Create(T _object);
        public Task<T> Delete(long id);        
        public Task<T> UpdateName(long id, string name);
        public Task<T> GetById(long Id);
        public Task<IEnumerable<T>> GetAll();
    }
}
