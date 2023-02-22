using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HotelBookingDAL.Contracts
{
    public interface IRepository <T>
    {
        public Task<T> Create(T _object);
        public Task<T> Delete(int id);
        public Task<T> Update(int id ,T _object);
        public Task<IEnumerable<T>> GetAll();
        public Task<T> GetById(int Id);
    }
}
