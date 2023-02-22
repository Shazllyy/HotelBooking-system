using Microsoft.AspNetCore.Mvc;
using HotelBookingDAL.Models;
using HotelBookingDAL.Contracts;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HotelBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
 
    public class RoomController : ControllerBase
    {
        private readonly IRepository<room> _repositoryRooms;
        private readonly IRoomReserve<room> _repositoryRoomsReserve;
        private readonly IRoomCancelReserve<room> _repositoryRoomsCancelReserve;
        private readonly IRoomFindAvailable<room> _repositoryRoomsFindAvailable;
        private readonly ICheckRoomNumbers<room> _repositoryRoomsCHeckNumber;
        public RoomController(IRepository<room> repository,
            IRoomReserve<room> RoomsReserve
            ,IRoomCancelReserve<room> RoomsCancelReserve,
            IRoomFindAvailable<room> RoomsFindAvailable,
            ICheckRoomNumbers<room> RoomsCheckNumber)
        { 
            _repositoryRooms = repository;
            _repositoryRoomsReserve = RoomsReserve;
            _repositoryRoomsCancelReserve = RoomsCancelReserve;
            _repositoryRoomsFindAvailable = RoomsFindAvailable;
            _repositoryRoomsCHeckNumber = RoomsCheckNumber;
        }

        // GET: api/<RoomsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<room>>> Get()
        {
            var obj = await _repositoryRooms.GetAll();
            if (obj != null)
            {
                return Ok(obj);
            }
            return NotFound();
        }

        // GET api/<RoomsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<room>> Get(int id)
        {
            if (id != 0)
            {
                var obj = await _repositoryRooms.GetById(id);
                if (obj != null)
                {
                    return Ok(obj);
                }
                return NotFound();
            }
            return BadRequest("ID is missing");
        }

        
        // POST api/<RoomsController>
        [HttpPost]
        public async Task<ActionResult<room>> Post(room rm)
        {

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            if (rm.room_type == "single")
            {
                rm.room_capacity = 1;
            }
            else if (rm.room_type == "double")
            {
                rm.room_capacity = 2;
            }
            else if (rm.room_type == "suite")
            {
                rm.room_capacity = 8;
            }

            if (rm.room_type != "single" && rm.room_type != "double" && rm.room_type != "suite")
            {
                return BadRequest("Invalid room type");
            }

            var CheckRoomNumber = _repositoryRooms.GetAll().Result.Where(r => r.room_number == rm.room_number 
            && 
            r.branch_id == rm.branch_id).FirstOrDefault();
            if (CheckRoomNumber != null) {
                return BadRequest("Room number already exists at same branch");
            }
            

            if (rm.room_status != "available")
            {
                return BadRequest("Invalid room status , room status must be available at creation");
            }           
            else {
                room obj = new room()
                {
                    branch_id = rm.branch_id,
                    room_type = rm.room_type,
                    room_price = rm.room_price,
                    room_status = rm.room_status,
                    room_number = rm.room_number,
                    room_capacity = rm.room_capacity,
                    room_residents_count = rm.room_residents_count
                };
                var result = await _repositoryRooms.Create(obj);
                if (result != null)
                {
                    string url = HttpContext.Request.Path.Value;
                    return Created(url, result);
                }              
                return BadRequest("Not created - Error probably from branch ID");
                
            }

                     
        }

        // PUT api/<RoomsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<room>> Put(int id,room rm)
        {          
            if (id != 0 && rm != null && ModelState.IsValid)
            {


                if (rm.room_type == "single")
                {
                    rm.room_capacity = 1;
                }
                else if (rm.room_type == "double")
                {
                    rm.room_capacity = 2;
                }

                if (rm.room_status != "booked" && rm.room_status != "available")
                {
                    return BadRequest("Invalid room status");
                }

                if (rm.room_type != "single" && rm.room_type != "double" && rm.room_type != "suite")
                {
                    return BadRequest("Invalid room type");
                }

                var CheckRoomNumber = _repositoryRooms.GetAll().Result.Where(r => r.room_number == rm.room_number
                &&
                r.branch_id == rm.branch_id).FirstOrDefault();
                if (CheckRoomNumber != null)
                {
                    return BadRequest("Room number already exists at same branch");
                }
                var obj = await _repositoryRooms.Update(id, rm);
                if (obj != null)
                {
                    return Ok(obj);
                }
                return NotFound("Not found or invalid branch ID");
            }
            return BadRequest("ID is missing or data is invalid");

        }

        // DELETE api/<RoomsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<room>> Delete(int id)
        {         
            if (id != 0)
            {
                var obj = await _repositoryRooms.Delete(id);
                if (obj != null)
                {
                    return Ok(obj);
                }
                return NotFound();
            }
            return BadRequest("ID is missing");
        }

        [HttpPut("reserve/{id}/{residentCount}")]
        public async Task<ActionResult<room>> ReserveRoom(int id, int residentCount)
        {
            if (id != 0 && residentCount != 0 && residentCount > 0 ) 
            {
                var obj = await _repositoryRoomsReserve.bookRoom(id, residentCount);
                if (obj != null)
                {
                    return Ok(obj);
                }
                return NotFound("Not found or invalid resident number");
            }
            return BadRequest("ID is missing or residentCount is missing");

        }

        [HttpPut("cancelreserve/{id}")]
        public async Task<ActionResult<room>> cancelReserve(int id)
        {
            if (id != 0)
            {
                var obj =  await _repositoryRoomsCancelReserve.cancelBooking(id);
                if (obj != null) {
                    return Ok(obj);
                }
                return NotFound();
            }               
            return BadRequest("ID is missing");
                        
            
        }

        [HttpGet("getavailable")]
        public async Task<ActionResult<IEnumerable<room>>> findAvailable(DateTime startDate, DateTime endDate,int branch_id)
        {
            if (startDate >= DateTime.MinValue && endDate <= DateTime.MaxValue && branch_id !=0
               )
            {
                if (startDate > endDate) {
                    return BadRequest("Start date can't be greater than end date");
                }
                var obj = await _repositoryRoomsFindAvailable.getAvailableRooms(startDate, endDate,branch_id);
                if (obj != null)
                {
                    return Ok(obj);
                }
                return NotFound("No rooms available");
            }
            return BadRequest("Dates invalid");
        }

        [HttpGet("checknumber/{number}")]
        public async Task<ActionResult<room>> checkRoomNumber(int number, int branch_id)
        {
            if (number != 0 && branch_id != 0)
            {
                var obj = await _repositoryRoomsCHeckNumber.CheckRoomNumbers(number, branch_id);
                if (obj != null)
                {
                    return Ok(obj);
                }
                return NotFound("Room number not found");
            }
            return BadRequest("Invalid room number or branch ID");
        }



    }
}
