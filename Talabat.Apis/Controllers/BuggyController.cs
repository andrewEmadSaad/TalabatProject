using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Apis.Errors;
using Talabat.Repository.Data.Contexts;

namespace Talabat.Apis.Controllers
{

    public class BuggyController : BaseApiController
    {
        private readonly StoreContext _context;

        public BuggyController(StoreContext context) 
        {
           _context = context;
        }

        [HttpGet("notfound")]
        public ActionResult GetNotFoundRequest()
        {
            var product = _context.Products.Find(100);
            if(product == null) return NotFound(new ApiResponse(404));

            return Ok(product);
        }


        [HttpGet("servererror")]
        public ActionResult GetServerErorr()
        {
            var product = _context.Products.Find(100);
            //var productToReturn =product.ToString(); //will Throw Exception
            var productToReturn =product?.ToString(); //will Throw Exception

            return Ok(productToReturn);
        }

        [HttpGet("badrequest")]
        public ActionResult GetBadRequest() 
        {
            return BadRequest(new ApiResponse(400));
        }

        [HttpGet("badrequest/{id}")]
        public ActionResult GetBadRequest(int id)
        {
            return BadRequest();
        }
    }
}
