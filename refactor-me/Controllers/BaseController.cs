using refactor_me.Repository;
using System.Web.Http;

namespace refactor_me.Controllers
{
    public class BaseController : ApiController
    {
        protected IProductDbContext db;

        public BaseController()
        {
            db = new ProductDbContext();
        }
        public BaseController(IProductDbContext dbContext)
        {
            db = dbContext;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
