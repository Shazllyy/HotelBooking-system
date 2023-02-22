using Microsoft.AspNetCore.Mvc;
using HotelBookingDAL.Models;
using HotelBookingDAL.Contracts;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HotelBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrancheController : ControllerBase
    {

        private readonly IRepository<branch> _repositoryBranches;

        public BrancheController(IRepository<branch> repository)
        {
            _repositoryBranches = repository;
        }

        // GET: api/<BranchesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<branch>>> Get()
        {
           
            var obj = await _repositoryBranches.GetAll();
            if (obj != null) {
                return Ok(obj);
            }
            return NotFound();
        }

        // GET api/<BranchesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<branch>> Get(int id)
        {
            if (id != 0) {
                var obj = await _repositoryBranches.GetById(id);
                if (obj != null)
                {
                    return Ok(obj);
                }
                return NotFound();                                    
            }
            return BadRequest("ID is missing");
        }

        // POST api/<BranchesController>
        [HttpPost]
        public async Task<ActionResult<branch>> Post(branch branch)
        {
            if (branch != null && ModelState.IsValid) {
                var obj = await _repositoryBranches.Create(branch);
                if (obj != null)
                {
                    string url = HttpContext.Request.Path.Value;
                    return Created(url, obj);
                }               
            }
            
            return BadRequest("Not created");
        }

        // PUT api/<BranchesController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<branch>> Put(int id,branch branch)
        {
            if (id != 0 && branch != null && ModelState.IsValid) {
             var obj = await _repositoryBranches.Update(id, branch);
                if (obj != null)
                {
                    return Ok(obj);
                }
                else {
                    return NotFound();
                }
            }
            return BadRequest("data is not valid or ID is missing");
        }

        // DELETE api/<BranchesController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<branch>> Delete(int id)
        {
            if (id != 0) {
               var obj = await _repositoryBranches.Delete(id);
                if (obj != null)
                {
                    return Ok(obj);
                }
                else {
                    return NotFound();
                }
            }
            return BadRequest("ID is missing");

        }
    }
}
