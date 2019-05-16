using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using AppStore.Filters;
using AppStore.Helpers;
using AppStore.Models;

namespace AppStore.Controllers
{
    [JwtAuth]
    //[Authorize(Roles = "admin")]
    public class applicationsController : ApiController
    {
        private AppStoreEntities db = new AppStoreEntities();


        // GET: api/applications
        [ResponseType(typeof(application))]
        public APIResult Getapplication()
        {
            var authUser = Request.Properties["user"] as string;

            //checkAuth.isAuth(Request)

            return new APIResult()
            {
                success = true,
                message = $"授權使用者為 {authUser}",
                payload = new string[] { "有提供存取權杖1", "有提供存取權杖2" }
            };

            //application application = db.application.Find(id);
            //if (application == null)
            //{
            //    return NotFound();
            //}

            //return Ok(application);
        }

        // PUT: api/applications/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putapplication(int id, application application)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != application.id)
            {
                return BadRequest();
            }

            db.Entry(application).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!applicationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/applications
        [ResponseType(typeof(application))]
        public IHttpActionResult Postapplication(application application)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.application.Add(application);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = application.id }, application);
        }

        // DELETE: api/applications/5
        [ResponseType(typeof(application))]
        public IHttpActionResult Deleteapplication(int id)
        {
            application application = db.application.Find(id);
            if (application == null)
            {
                return NotFound();
            }

            db.application.Remove(application);
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