using HotelBookingDAL.Contracts;
using HotelBookingDAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationDataController : ControllerBase
    {
        private readonly IReservationData<ReservationData> _reservationDataRepository;
        public ReservationDataController(IReservationData<ReservationData> reservationDataRepository)
        {
            _reservationDataRepository = reservationDataRepository;
        }
        [HttpGet("GetReservationData/{id}")]
        public async Task<ActionResult<ReservationData>> GetReservationData(int id)
        {
            if (id != 0)
            {
                var reservationData = await _reservationDataRepository.GetAllData(id);
                if (reservationData != null)
                {
                    return Ok(reservationData);
                }
                return NotFound();
            }
            return BadRequest("ID is missing");
        }
    }
}
