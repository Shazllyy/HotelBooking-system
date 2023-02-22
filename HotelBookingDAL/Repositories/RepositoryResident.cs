using HotelBookingDAL.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBookingDAL.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingDAL.Repositories
{
    public class RepositoryResident : IResident<resident>
    {
        private readonly HotelBookingContext _DbContext;

        public RepositoryResident(HotelBookingContext context)
        {
            _DbContext = context;
        }
        
        public async Task<resident> Create(resident _object)
        {
            try
            {                
                    var obj =await _DbContext.residents.AddAsync(_object);
                    await _DbContext.SaveChangesAsync();
                    return obj.Entity;
               
            }
            catch (Exception)
            {
               return null;
            }
        }

        public async  Task<resident> Delete(long id)
        {
            try
            {               
                    var obj = await _DbContext.residents.FindAsync(id);                   
                     _DbContext.residents.Remove(obj);
                     await  _DbContext.SaveChangesAsync();
                     return obj;                                                   
            }
            catch (Exception)
            {
                 return null;
            }
        }

        public async Task<IEnumerable<resident>> GetAll()
        {
            try
            {

                return  await _DbContext.residents.ToListAsync();

            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<resident> GetById(long Id)
        {
            try
            {

                return await _DbContext.residents.FindAsync(Id);
                
                   
               
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        public async Task<resident> UpdateName(long id, string name)
        {
            try
            {
                var obj = await _DbContext.residents.FindAsync(id);
                obj.resident_name = name;
                await _DbContext.SaveChangesAsync();
                return obj;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
