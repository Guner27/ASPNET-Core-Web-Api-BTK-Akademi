using bookDemo.Data;
using bookDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace bookDemo.Controller
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var books = ApplicationContext.Books;
            return Ok(books);
        }
        [HttpGet("{id:int}")]
        public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
        {
            var book = ApplicationContext.Books.Where(b => b.Id.Equals(id)).SingleOrDefault();
            if (book == null)
                return NotFound();  //404

            return Ok(book);
        }

        [HttpPost]
        public IActionResult CreateOneBook([FromBody] Book book)
        {
            try
            {
                if (book == null)
                    return BadRequest(); // 400

                ApplicationContext.Books.Add(book);
                return StatusCode(201, book);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] Book book)
        {
            //check book?
            var entity = ApplicationContext.Books.Find(b => b.Id.Equals(id));

            if (entity == null)
                return NotFound();//404

            if(id != book.Id)
                return BadRequest(); //400

            ApplicationContext.Books.Remove(entity);
            book.Id = entity.Id;
            ApplicationContext.Books.Add(book);
            return Ok(book);
        }

        [HttpDelete]
        public IActionResult DeleteAllBook()
        {
            ApplicationContext.Books.Clear();
            return NoContent();//204
        }
        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook([FromRoute(Name ="id")]int id)
        {
            var entity = ApplicationContext.Books.Find(b=>b.Id.Equals(id));
            if (entity is null)
                return NotFound(new { message=$"Book with id: {id} could not found"});//404

            ApplicationContext.Books.Remove(entity);
            return NoContent();//204
        }

        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdateOneBook([FromRoute(Name ="id")]int id,[FromBody] JsonPatchDocument<Book> bookPatch) {

            //check entity
            var entity = ApplicationContext.Books.Find(b=>b.Id.Equals(id));
            if (entity is null)
                return NotFound(); //404

            bookPatch.ApplyTo(entity);
            return NoContent();
        }
    }
}
