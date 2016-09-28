using refactor_me.Models;
using refactor_me.Repository;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;

namespace refactor_me.Controllers
{
    [RoutePrefix("products")]
    public class ProductsController : BaseController
    {
        public ProductsController() : base()
        { }
        public ProductsController(IProductDbContext db) : base(db)
        { }
        // GET: /products
        [Route]
        [HttpGet]
        public IHttpActionResult GetProducts()
        {
            return Ok(new
            {
                Items = db.Products.ToList()
            });
        }

        // GET: /products?name={name}
        [Route]
        [HttpGet]
        public IHttpActionResult SearchByName(string name)
        {
            var products = db.Products.Where(p => p.Name.Contains(name)).ToList();

            return Ok(new
            {
                Items = products
            });
        }

        // GET: /products/{id}
        [Route("{id:guid}", Name = "GetProductById")]
        [HttpGet]
        [ResponseType(typeof(Product))]
        public IHttpActionResult GetProduct(Guid id)
        {
            var product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // POST: /products
        [Route]
        [HttpPost]
        [ResponseType(typeof(Product))]
        public IHttpActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Products.Add(product);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (db.ProductExists(product.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("GetProductById", new { id = product.Id }, product);
        }

        // PUT: /products/{id}
        [Route("{id:guid}")]
        [HttpPut]
        [ResponseType(typeof(Product))]
        public IHttpActionResult Update(Guid id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.Id)
            {
                return BadRequest();
            }

            db.SetEntityModified(product);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!db.ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //return StatusCode(HttpStatusCode.Accepted);
            return Content(HttpStatusCode.Accepted, product);
        }

        // DELETE: /products/{id}
        [Route("{id:guid}")]
        [HttpDelete]
        [ResponseType(typeof(void))]
        public IHttpActionResult Delete(Guid id)
        {
            var product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            //avoid orphan options
            db.ProductOptions.RemoveRange(db.ProductOptions.Where(p => p.ProductId == id).ToList());

            db.Products.Remove(product);
            db.SaveChanges();

            return Ok();
        }
    }
}
