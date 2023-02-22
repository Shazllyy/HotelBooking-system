using Microsoft.AspNetCore.Mvc;
using HotelBookingDAL.Models;
using HotelBookingDAL.Contracts;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HotelBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResidentRoomsController : ControllerBase
    {

        private readonly IRepository<resident_room> _repositoryResidentsRooms;
        private readonly IGetResidentRoooms<resident_room> _repositoryGetResidentRooms;       
        public ResidentRoomsController(IRepository<resident_room> repository,
            IGetResidentRoooms<resident_room> repositoryGetResidentRooms           
            )
        {
            _repositoryResidentsRooms = repository;
            _repositoryGetResidentRooms = repositoryGetResidentRooms;
           
        }
        // GET: api/<ResidentRoomsController>
        [HttpGet]
        public  async Task<ActionResult<IEnumerable<resident_room>>> Get()
        {
            var obj = await _repositoryResidentsRooms.GetAll();
            if (obj != null) {
                return Ok(obj);
            }
            return NotFound();
        }

        // GET api/<ResidentRoomsController>/5
        [HttpGet("GetByReservationID/{id}")]
        public async Task<ActionResult<resident_room>> Get(int id)
        {
            if (id != 0)
            {
                var obj = await _repositoryResidentsRooms.GetById(id);
                if (obj != null)
                {
                    return Ok(obj);
                }
                return NotFound();

            }
            return BadRequest();
        }

        // POST api/<ResidentRoomsController>

        [HttpGet("GetByResidentID/{id}")]
        public async Task<ActionResult<IEnumerable<resident_room>>> Get(long id)
        {
            if (id != 0)
            {
                var obj = await _repositoryGetResidentRooms.GetResidentRooms(id);
                if (obj != null && obj.Count() >0)
                {
                    return Ok(obj);
                }
                return NotFound();

            }
            return BadRequest("ID is missing");
        }

        [HttpPost]
        public async Task<ActionResult<resident_room>> Post(resident_room rsntRom)
        {      
            if (rsntRom != null && ModelState.IsValid)
            {
                if (rsntRom.resident_nationalID.ToString().Length < 14)
                {
                    return BadRequest("National ID is less than 14 digits long !!");
                }
                if (rsntRom.start_book_date > rsntRom.end_book_date)
                {
                    return BadRequest("Start date can't be greater than end date");
                }
                
                rsntRom.is_active = true;
                rsntRom.canceled = false;
                
                var obj = await _repositoryResidentsRooms.Create(rsntRom);
                if (obj != null)
                {
                    string url = HttpContext.Request.Path.Value;
                    return Created(url, obj);
                }
            }
            return BadRequest("Not created");

        }

        // PUT api/<ResidentRoomsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<resident_room>> Put(int id,resident_room rsntRom)
        {
            if (id != 0 && rsntRom != null && ModelState.IsValid) { 
            var obj = await _repositoryResidentsRooms.Update(id, rsntRom);
                if (obj != null) {
                    return Ok(obj);
                }
                return NotFound();
            }
            return BadRequest("ID is missing or data is invalid");
            
        }

        // DELETE api/<ResidentRoomsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<resident_room>> Delete(int id)
        {
            if (id != 0)
            {
                var obj = await _repositoryResidentsRooms.Delete(id);
                if (obj != null)
                {
                    return Ok(obj);
                }
                return NotFound();
            }
            return BadRequest("ID is missing");
            
          
        }
    }
}
