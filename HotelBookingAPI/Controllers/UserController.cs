using Microsoft.AspNetCore.Mvc;
using HotelBookingDAL.Models;
using HotelBookingDAL.Contracts;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HotelBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IRepository<user> _repositoryusers;
        private readonly ILogin<user> _repositoryusersLogin;
        public UserController(IRepository<user> repository,ILogin<user> Login)
        {
            _repositoryusers = repository;
            _repositoryusersLogin = Login;
        }
        // GET: api/<UserController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<user>>> Get()
        {
            var obj = await _repositoryusers.GetAll();
            if (obj != null)
            {
                return Ok(obj);
            }
            return NotFound();
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<user>> Get(int id)
        {            
            if (id != 0)
            {
                var obj = await _repositoryusers.GetById(id);
                if (obj != null)
                {
                    return Ok(obj);
                }
                return NotFound();
            }
            return BadRequest();            
        }
        
        [HttpGet("login/{userName}/{passWord}")]
        public async Task<ActionResult<user>> Login(string userName , string passWord)
        {
            if (userName != "" && passWord !="")
            {
                var obj = await _repositoryusersLogin.Login(userName, passWord);
                if (obj != null)
                {
                    return Ok(obj);
                }
                return NotFound();
            }
            return BadRequest();
        }


        // POST api/<UserController>
        [HttpPost]
        public async Task<ActionResult> Post(user usr)
        {
            if (usr!=null)
            {
                var obj = await _repositoryusers.Create(usr);
                if (obj != null) {
                    string url = HttpContext.Request.Path.Value;
                    return Created(url, obj);
                }
                
            }
            return BadRequest("Not created");

        }


        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<user>> Put(int id,user usr)
        {

            if (id != 0 && usr != null)
            {
                var obj = await _repositoryusers.Update(id, usr);
                if (obj != null)
                {
                    return Ok(obj);
                }
                else
                {
                    return NotFound();
                }
            }
            return BadRequest();
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<user>> Delete(int id)
        {
            if (id != 0)
            {
                var obj = await _repositoryusers.Delete(id);
                if (obj != null)
                {
                    return Ok(obj);
                }
                else
                {
                    return NotFound();
                }
            }
            return BadRequest();
        }
    }
}
