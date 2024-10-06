using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;

namespace BookstoreReactCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Bearer")]
    public class PersonsController : ControllerBase
    {
        private readonly IPersonService _personService;

        public PersonsController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpGet]
        [ProducesResponseType((200), Type = typeof(List<Person>))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _personService.GetAll());
        }

        [HttpGet("{id}")]
        [ProducesResponseType((200), Type = typeof(Person))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public  async Task<IActionResult> Get(long id)
        {
            var person = await _personService.GetById(id);
            if (person == null) 
                return NotFound();

            return Ok(person);
        }

        [HttpGet("findPersonByName")]
        [ProducesResponseType((200), Type = typeof(List<Person>))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public  async Task<IActionResult> Get([FromQuery] string firstName, [FromQuery] string lastName)
        {
            var person = await _personService.FindByName(firstName, lastName);

            if (person == null) 
                return NotFound();

            return Ok(person);
        }

        [HttpPost]
        [ProducesResponseType((200), Type = typeof(Person))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public  async Task<IActionResult> Post([FromBody] Person person)
        {
            if (person == null) 
                return BadRequest();

            return Ok(await _personService.Create(person));
        }

        [HttpPut("{id}")]
        [ProducesResponseType((200), Type = typeof(bool))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public  async Task<IActionResult> Put(long id, [FromBody] Person person)
        {
            if (person == null) 
                return BadRequest();

            return Ok(await _personService.Update(id, person));
        }

        [HttpPatch("{id}")]
        [ProducesResponseType((200), Type = typeof(bool))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public  async Task<IActionResult> Patch(long id)
        {
            var person = await _personService.Disable(id);
            return Ok(person);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((200), Type = typeof(bool))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public  async Task<IActionResult> Delete(long id)
        {
            await _personService.Delete(id);
            return NoContent();
        }
    }
}
