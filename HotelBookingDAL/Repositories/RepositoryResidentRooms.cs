using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBookingDAL.Contracts;
using HotelBookingDAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBookingDAL.Repositories
{
    public class RepositoryResidentRooms :IRepository <resident_room> ,
        IResidentRoomDelete<resident_room>,
        IGetResidentRoooms<resident_room>
    {
        private readonly HotelBookingContext _DbContext;

        public RepositoryResidentRooms(HotelBookingContext context)
        {
            _DbContext = context;
        }
       

        public async Task<resident_room> Create(resident_room _object)
        {
            try
            {                                   
                    var obj =await _DbContext.resident_rooms.AddAsync(_object);
                    await _DbContext.SaveChangesAsync();
                    return obj.Entity;
               
            }
            catch (Exception)
            {
               return null;
            }
        }

        public async Task<resident_room> Delete(int id)
        {
            try
            {
               
                    var obj = await _DbContext.resident_rooms.FindAsync(id);                   
                     _DbContext.Remove(obj);
                     await _DbContext.SaveChangesAsync();
                     return obj;

            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<resident_room> DeleteByUsrIDAndRomID(int USR_id, int RomID)
        {
            try
            {                
                    var obj = await _DbContext.resident_rooms.FindAsync(USR_id.ToString(), RomID);                   
                    _DbContext.resident_rooms.Remove(obj);
                     await _DbContext.SaveChangesAsync();
                     return obj;
                 
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<resident_room>> GetAll()
        {
            try
            {
                return await _DbContext.resident_rooms.ToListAsync();
               
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<resident_room> GetById(int Reservation_ID )
        {
            try
            {
                return await _DbContext.resident_rooms.FindAsync(Reservation_ID);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<resident_room>> GetResidentRooms(long residentId)
        {
            try
            {
                return await _DbContext.resident_rooms.Where(RS => RS.resident_nationalID == residentId).ToListAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<resident_room> Update(int id ,resident_room _object)
        {
            try
            {
                var obj = await _DbContext.resident_rooms.FindAsync(id);
                obj.room_id = _object.room_id;
                obj.resident_nationalID = _object.resident_nationalID;
                obj.checkout_date = _object.checkout_date;
                obj.start_book_date = _object.start_book_date;
                obj.room_id = _object.room_id;
                obj.resident_nationalID = _object.resident_nationalID;
                obj.checkout_date = _object.checkout_date;
                obj.room_total_price = _object.room_total_price;
                obj.canceled = _object.canceled;
                obj.is_active = _object.is_active;
                
                await  _DbContext.SaveChangesAsync();
                return obj;                                                  
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
