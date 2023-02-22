using Microsoft.AspNetCore.Mvc;
using HotelBookingDAL.Models;
using HotelBookingDAL.Contracts;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HotelBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResidentController : ControllerBase
    {
        private readonly IResident<resident> _repositoryResidents;

        public ResidentController(IResident<resident> repository)
        {
            _repositoryResidents = repository;
        }

        // GET: api/<ResidentController>
        [HttpGet]
        public  async Task<ActionResult<IEnumerable<resident>>> Get()
        {
            var obj= await _repositoryResidents.GetAll();
            if (obj != null) {
                return Ok(obj);
            }
            return NotFound();
        }

        // GET api/<ResidentController>/5
        [HttpGet("{id}")]
        public  async Task<ActionResult<resident>> Get(long id)
        {
            
            if (id != 0) {
                var obj = await _repositoryResidents.GetById(id);
                if (obj != null)
                {
                    return Ok(obj);
                }              
                  return NotFound();
                
            }
            return BadRequest("ID is missing");
                
            
        }

        // POST api/<ResidentController>
        [HttpPost]
        public async Task<ActionResult<resident>> Post(resident rsnt)
        {
            if (rsnt.nationalID.ToString().Length != 14)
            {
                return BadRequest("National ID must be 14 digits");
            }

            if (rsnt != null && ModelState.IsValid)
            {
                var residentHere = await _repositoryResidents.GetById(rsnt.nationalID);
                if (residentHere != null)
                {
                    return BadRequest("Resident with this national ID already exists");
                }
                else {
                    var obj = await _repositoryResidents.Create(rsnt);
                    if (obj != null)
                    {
                        string url = HttpContext.Request.Path.Value;
                        return Created(url, obj);

                    }
                }                               
            }
            return BadRequest("Not created");

        }
       
        [HttpPut("name/{id}")]
        public async Task<ActionResult<resident>> Put(long id, string name)
        {
            if (id != 0 && name != "" && ModelState.IsValid)
            {                
                    var obj = await _repositoryResidents.UpdateName(id, name);
                    if (obj != null)
                    {
                        return Ok(obj);
                    }
                    return NotFound();
                
            }
            return BadRequest("ID or name is missing ");
        }


        // DELETE api/<ResidentController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<resident>> Delete(long id)
        {
            if (id != 0) { 
                var obj = await _repositoryResidents.Delete(id);
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
