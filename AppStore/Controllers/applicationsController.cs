using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using AppStore.DAO;
using AppStore.Filters;
using AppStore.Models;
using AppStore.Services;
using AppStore.Utils;
using Newtonsoft.Json.Linq;

namespace AppStore.Controllers
{
    public class applicationsController : ApiController
    {
        private AppStoreEntities db = new AppStoreEntities();

        // GET: api/applications/{application_id}
        [ResponseType(typeof(application))]
        [JwtAuth]
        public IHttpActionResult Getapplication(int id)
        {
            string user_id = Request.Properties["user"] as string;
            if (Service.isApplicationCanRead(db, user_id, id))
            {
                JObject app = ApplicationDAO.get(db, id, true);
                if (null == app)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(app);
                }
            }
            else
            {
                return BadRequest("can_not_read_application");
            }
        }

        // GET: api/applications
        [ResponseType(typeof(application))]
        [JwtAuth]
        public IHttpActionResult Getapplication()
        {
            string user_id = Request.Properties["user"] as string;
            return Ok(ApplicationDAO.getUserApps(db, user_id));
        }

        // PUT: api/applications
        [ResponseType(typeof(void))]
        [JwtAuth]
        public IHttpActionResult Putapplication(application application)
        {
            // 檢查Model的狀態
            if (!ModelState.IsValid) return BadRequest(ModelState);

            string user_id = Request.Properties["user"] as string;
            application searchApp = ApplicationDAO.get(db, application.id);
            if (null == searchApp) return NotFound();

            if (Service.isApplicationCanModify(db, user_id, application.id))
            {
                if (null != application.privacy_type) searchApp.privacy_type = application.privacy_type;
                searchApp.@lock = application.@lock;

                db.Entry(searchApp).State = EntityState.Modified;
                db.SaveChanges();

                return Ok(searchApp);
            }
            else
            {
                return Content(HttpStatusCode.Unauthorized, new HttpMessage("insufficient_user_rights").toJson());
            }
        }

        // POST: api/applications
        [ResponseType(typeof(application))]
        [JwtAuth]
        public IHttpActionResult Postapplication(application application)
        {
            // 檢查Model的狀態
            if (!ModelState.IsValid) return BadRequest(ModelState);

            string user_id = Request.Properties["user"] as string;

            db.application.Add(application);
            //把自己建的APP跟自己建立關聯
            db.user_application.Add(new user_application() { user_id = user_id, application_id = application.id, role = "manager" });
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = application.id }, application);
        }

        // DELETE: api/applications
        [ResponseType(typeof(application))]
        [JwtAuth]
        public IHttpActionResult Deleteapplication(application application)
        {
            string user_id = Request.Properties["user"] as string;
            application searchApp = ApplicationDAO.get(db, application.id);
            if (searchApp == null) return NotFound();

            if (Service.isApplicationCanModify(db, user_id, application.id))
            {
                db.application.Remove(searchApp);
                db.SaveChanges();

                return Ok(searchApp);
            }
            else
            {
                return Content(HttpStatusCode.Unauthorized, new HttpMessage("insufficient_user_rights").toJson());
            }
        }
    }
}