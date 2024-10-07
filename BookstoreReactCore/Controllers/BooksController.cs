using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO;
using Services.Interfaces;

namespace BookstoreReactCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Bearer")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService  _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        [ProducesResponseType((200), Type = typeof(List<Book>))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public  async Task<IActionResult> GetAll()
        {
            return Ok(await _bookService.GetAll());
        }

        [HttpGet("{sortDirection}/{pageSize}/{page}")]
        [ProducesResponseType((200), Type = typeof(Task<PagedSearchDTO<Book>>))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public  async Task<IActionResult> Get(
            [FromQuery] string title,
            string sortDirection,
            int pageSize,
            int page)
        {
            return Ok(await _bookService.GetWithPagedSearch(title, sortDirection, pageSize, page));
        }

        [HttpGet("{id}")]
        [ProducesResponseType((200), Type = typeof(Book))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public  async Task<IActionResult> Get(long id)
        {
            var book = await _bookService.GetById(id);
            if (book == null) 
                return NotFound();

            return Ok(book);
        }

        [HttpPost]
        [ProducesResponseType((200), Type = typeof(Book))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public  async Task<IActionResult> Post([FromBody] Book book)
        {
            if (book == null) 
                return BadRequest();

            return Ok(await _bookService.Create(book));
        }

        [HttpPut("{id}")]
        [ProducesResponseType((200), Type = typeof(bool))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public  async Task<IActionResult> Put(long id, [FromBody] Book book)
        {
            if (book == null) 
                return BadRequest();

            return Ok(await _bookService.Update(id, book));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((200), Type = typeof(bool))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public  async Task<IActionResult> Delete(long id)
        {
            await _bookService.Delete(id);
            return NoContent();
        }
    }
}
