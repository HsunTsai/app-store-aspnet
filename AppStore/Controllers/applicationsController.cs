using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using AppStore.DAO;
using AppStore.Filters;
using AppStore.Models;

namespace AppStore.Controllers
{
    public class applicationsController : ApiController
    {
        private AppStoreEntities db = new AppStoreEntities();

        // GET: api/trackings/type
        [ResponseType(typeof(tracking))]
        public IHttpActionResult Getapplication(string type)
        {
            switch (type)
            {
                case "all":
                    return Ok(ApplicationDAO.getPublicApp(db));
                default:
                    return BadRequest("type not match");

            }
        }

        // GET: api/applications
        [ResponseType(typeof(application))]
        [JwtAuth]
        public IHttpActionResult Getapplication()
        {
            string user_id = Request.Properties["user"] as string;

            //return new APIResult()
            //{
            //    success = true,
            //    message = $"授權使用者為 {authUser}",
            //    payload = new string[] { "有提供存取權杖1", "有提供存取權杖2" }
            //};

            return Ok(ApplicationDAO.getUserApp(db, user_id));
        }

        // PUT: api/applications
        [ResponseType(typeof(void))]
        [JwtAuth]
        [Authorize(Roles = "admin,manager")]
        public IHttpActionResult Putapplication(application application)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            application searchApp = ApplicationDAO.get(db, application.id);
            searchApp.privacy_type = application.privacy_type;
            searchApp.@lock = application.@lock;

            db.Entry(searchApp).State = EntityState.Modified;
            db.SaveChanges();

            //try
            //{
            //    db.SaveChanges();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!applicationExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/applications
        [ResponseType(typeof(application))]
        [JwtAuth]
        public IHttpActionResult Postapplication(application application)
        {
            string user_id = Request.Properties["user"] as string;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.application.Add(application);
            user_application user_Application = new user_application() { user_id = user_id, application_id = application.id };
            db.user_application.Add(user_Application);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = application.id }, application);
        }

        // DELETE: api/applications
        [ResponseType(typeof(application))]
        [JwtAuth]
        [Authorize(Roles = "admin,manager")]
        public IHttpActionResult Deleteapplication(application application)
        {
            application app = db.application.Find(application.id);
            if (app == null)
            {
                return NotFound();
            }

            db.application.Remove(app);
            db.SaveChanges();

            return Ok(application);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool applicationExists(int id)
        {
            return db.application.Count(e => e.id == id) > 0;
        }
    }
}