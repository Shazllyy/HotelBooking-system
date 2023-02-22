global using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBookingDAL.Contracts;
using HotelBookingDAL.Models;


namespace HotelBookingDAL.Repositories
{
    public class RepositoryRooms :IRepository <room>,
        IRoomReserve<room>,IRoomCancelReserve<room>,
        IRoomFindAvailable<room>,
        ICheckRoomNumbers<room>
    {

        private readonly HotelBookingContext _DbContext;        
        
        public RepositoryRooms(HotelBookingContext context)
        {
            _DbContext = context;
        }
        //one room
        public async Task<room> bookRoom(int roomId,int residentCount)
        {
            try
            {
               
                    var obj = await _DbContext.rooms.FindAsync(roomId);
                    if (obj != null) {
                        if (((obj.room_residents_count) + residentCount) > obj.room_capacity)
                        {
                            return null;
                        }
                    if ((obj.room_type == "double" || obj.room_type == "suite"))
                    {
                        obj.room_residents_count += residentCount;
                        obj.room_status = (obj.room_status== "available" ? "booked" : "available");
                        await _DbContext.SaveChangesAsync();
                        return obj;
                    } 
                    else if (obj.room_type == "single" && obj.room_status == "available"
                        &&
                        obj.room_capacity == 1
                        &&
                        obj.room_residents_count == 0
                        )
                    {
                        obj.room_residents_count = 1;
                        obj.room_status = "booked";
                        await _DbContext.SaveChangesAsync();
                        return obj;
                    }
                    else if (obj.room_type == "single" && obj.room_status == "booked")
                    {
                        return null;
                    }
                                                                                           
                    }
                    return null;    


            }
            catch (Exception)
            {

                return null;
            }
        }

        public async Task<room> cancelBooking(int RoomId)
        {
            try
            {
               
                    var obj = await _DbContext.rooms.FindAsync(RoomId);                                                                
                    obj.room_residents_count = 0;
                    obj.room_status = "available";                            
                    await _DbContext.SaveChangesAsync();
                    return obj;                                                                                                                   
               
            }
            catch (Exception)
            {
                 return null;
            }
        }

        public async Task<room> CheckRoomNumbers(int roomNumber, int branch_id)
        {
            try
            {
                return await _DbContext.rooms.Where(x => x.room_number == roomNumber && x.branch_id==branch_id).FirstOrDefaultAsync();
            }
            catch (Exception)
            {

                return null;
            }
        }

        public async Task<room> Create(room _object)
        {
            try
            {                                                             
                    var obj = await _DbContext.rooms.AddAsync(_object);
                    await _DbContext.SaveChangesAsync();
                    return obj.Entity;                                  
                               
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<room> Delete(int id)
        {
            try
            {
               
                var obj = await _DbContext.rooms.FindAsync(id);                   
                _DbContext.Remove(obj);
               await _DbContext.SaveChangesAsync();
                return obj;


            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<room>> GetAll()
        {
            try
            {
                return await _DbContext.rooms.ToListAsync();
                  
               
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<room>> getAvailableRooms(DateTime startDate, DateTime endDate ,int branch_id)
        {
            
            //get available rooms
            List<room> AvailableRooms = new List<room>();
            try
            {
                var RoomsList1 = await _DbContext.rooms.Where(r => r.room_status == "available" && r.branch_id== branch_id).ToListAsync();
                if (RoomsList1.Count() > 0)
                {
                    AvailableRooms.AddRange(RoomsList1);
                }
               
                

                var RoomsList2 = await _DbContext.resident_rooms.Where(r =>
                endDate < r.start_book_date                              
                ||                
                 startDate > r.end_book_date && r.room.branch_id == branch_id                                  
                ).Select(r => r.room).ToListAsync();                
                
                //TODO-consider real checkoutdate in serach
                if (RoomsList2.Count > 0) {
                    AvailableRooms.AddRange(RoomsList2);
                }

                var RoomList3 = await _DbContext.resident_rooms.Where(r =>
                r.is_active == false || r.canceled == true && r.room.branch_id == branch_id
                ).Select(r => r.room).ToListAsync();
                if (RoomList3.Count > 0)
                {
                    AvailableRooms.AddRange(RoomList3);
                }
                AvailableRooms = AvailableRooms.DistinctBy(r => r.room_id).ToList();

                return  AvailableRooms;
            }
            catch (Exception)
            {
                return null;
            }


        }

        public async Task<room> GetById(int Id)
        {
            try
            {
                return await _DbContext.rooms.FindAsync(Id);                                                      
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<room> Update(int id, room _object)
        {
            try
            {
               
                    var obj = await _DbContext.rooms.FindAsync(id);
                obj.branch_id = _object.branch_id;
                obj.room_type = _object.room_type;
                obj.room_capacity = _object.room_capacity;
                obj.room_status = _object.room_status;
                obj.room_residents_count = _object.room_residents_count;
                obj.room_price = _object.room_price;                
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
