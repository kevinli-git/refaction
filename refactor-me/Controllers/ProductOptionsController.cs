using refactor_me.Models;
using refactor_me.Repository;
using System;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;

namespace refactor_me.Controllers
{
    [RoutePrefix("products/{productId:guid}/options")]
    public class ProductOptionsController : BaseController
    {
        public ProductOptionsController() : base()
        { }
        public ProductOptionsController(IProductDbContext db) : base(db)
        { }
        // GET: /products/{productId}/options
        [Route]
        [HttpGet]
        public IHttpActionResult GetOptions(Guid productId)
        {
            var options = db.ProductOptions.Where(p => p.ProductId == productId).ToList();
            return Ok(new
            {
                Items = options
            });
        }

        // GET: /products/{productId}/options/{id}
        [Route("{id:guid}", Name = "GetOptionById")]
        [HttpGet]
        [ResponseType(typeof(ProductOption))]
        public IHttpActionResult GetOption(Guid productId, Guid id)
        {
            ProductOption productOption = db.ProductOptions.Find(id);
            if (productOption == null || productOption.ProductId != productId)
            {
                return NotFound();
            }

            return Ok(productOption);
        }

        // PUT: /products/{productId}/options/{id}
        [Route("{id:guid}")]
        [HttpPut]
        [ResponseType(typeof(ProductOption))]
        public IHttpActionResult UpdateOption(Guid id, ProductOption option)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != option.Id || !db.ProductExists(option.ProductId))
            {
                return BadRequest();
            }

            db.SetEntityModified(option);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!db.ProductOptionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //return StatusCode(HttpStatusCode.NoContent);
            return Content(HttpStatusCode.Accepted, option);
        }

        // POST: /products/{productId}/options
        [Route]
        [HttpPost]
        [ResponseType(typeof(ProductOption))]
        public IHttpActionResult CreateOption(Guid productId, ProductOption option)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!db.ProductExists(productId))
            {
                return BadRequest("Product not found.");
            }

            option.ProductId = productId;
            db.ProductOptions.Add(option);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (db.ProductOptionExists(option.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("GetOptionById", new { id = option.Id, productId = productId }, option);
        }

        // DELETE: /products/{productId}/options/{id}
        [Route("{id:guid}")]
        [HttpDelete]
        [ResponseType(typeof(void))]
        public IHttpActionResult DeleteOption(Guid productId, Guid id)
        {
            ProductOption productOption = db.ProductOptions.Find(id);
            if (productOption == null || productOption.ProductId != productId)
            {
                return NotFound();
            }

            db.ProductOptions.Remove(productOption);
            db.SaveChanges();

            return Ok();
        }
    }
}