using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBookingDAL.Contracts;
using HotelBookingDAL.Models;

namespace HotelBookingDAL.Repositories
{
    public class RepositoryReservationData : IReservationData<ReservationData>
    {

        private readonly HotelBookingContext _DbContext;
        public RepositoryReservationData(HotelBookingContext DbContext)
        { 
            _DbContext= DbContext;
        }
        public async Task<ReservationData> GetAllData(int id)
        {
            ReservationData reservationData = new ReservationData();
            
            try
            {
                reservationData.branch = await _DbContext.resident_rooms.Where(x => x.reservation_ID == id).Select(r => r.room.branch).FirstOrDefaultAsync();
                reservationData.resident = await _DbContext.resident_rooms.Where(x => x.reservation_ID == id).Select(r => r.resident).FirstOrDefaultAsync();
                reservationData.room = await _DbContext.resident_rooms.Where(x => x.reservation_ID == id).Select(r => r.room).FirstOrDefaultAsync();
                reservationData.resident_Room = await _DbContext.resident_rooms.Where(x => x.reservation_ID == id).FirstOrDefaultAsync();
                return reservationData;
            }
            catch (Exception)
            {

                return null;
            }
        }
    }
}
