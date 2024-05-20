using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    //Deprecated : Versiyonu yayından kaldır. (true)
    //[ApiVersion("2.0", Deprecated = true)]  //It's done comment line for Conventions 
    [ApiController]
    [Route("api/books")]
    //[Route("api/{v:apiversion}/books")] //Versioning with URL
    [ApiExplorerSettings(GroupName ="v2")]
    public class BooksV2Controller : ControllerBase
    {
        private readonly IServiceManager _manager;

        public BooksV2Controller(IServiceManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookAsync()
        {
            var books = await _manager.BookService.GetAllBooksAync(false);
            var booksV2 = books.Select(b => new
            {
                Title = b.Title,
                Id = b.Id
            });
            return Ok(booksV2);
        }
    }
}

