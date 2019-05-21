using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using AppStore.DAO;
using AppStore.Filters;
using AppStore.Models;
using AppStore.Services;
using AppStore.Utils;

namespace AppStore.Controllers
{
    public class applicationsController : ApiController
    {
        private AppStoreEntities db = new AppStoreEntities();

        // GET: api/applications
        [ResponseType(typeof(application))]
        [JwtAuth]
        public IHttpActionResult Getapplication()
        {
            string user_id = Request.Properties["user"] as string;
            return Ok(ApplicationDAO.getUserApp(db, user_id));
        }

        // PUT: api/applications
        [ResponseType(typeof(void))]
        [JwtAuth]
        public IHttpActionResult Putapplication(application application)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string user_id = Request.Properties["user"] as string;

            if (Service.isApplicationCanModify(db, user_id, application.id))
            {
                application searchApp = ApplicationDAO.get(db, application.id);
                if (null != application.privacy_type) searchApp.privacy_type = application.privacy_type;
                searchApp.@lock = application.@lock;

                db.Entry(searchApp).State = EntityState.Modified;
                db.SaveChanges();

                return Ok(new HttpMessage("update_succeed"));
            }
            else
            {
                return Unauthorized();
            }
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
            //把自己建的APP跟自己建立關聯
            db.user_application.Add(new user_application() { user_id = user_id, application_id = application.id, role = "manager" });
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = application.id }, application);
        }

        // DELETE: api/applications
        [ResponseType(typeof(application))]
        [JwtAuth]
        [Authorize(Roles = "admin")]
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

        private bool applicationExists(int id)
        {
            return db.application.Count(e => e.id == id) > 0;
        }
    }
}