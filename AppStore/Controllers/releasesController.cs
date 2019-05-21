using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using AppStore.DAO;
using AppStore.Filters;
using AppStore.Models;
using AppStore.Services;

namespace AppStore.Controllers
{
    public class releasesController : ApiController
    {
        private AppStoreEntities db = new AppStoreEntities();

        // GET: api/releases/{application_id}
        [ResponseType(typeof(release))]
        [JwtAuth]
        public IHttpActionResult Getrelease(int id)
        {
            string user_id = Request.Properties["user"] as string;
            if (Service.isApplicationCanRead(db, user_id, id))
            {
                List<release> releaseApps = ReleaseDAO.getAppRelease(db, id);
                return Ok(releaseApps);
            }
            else
            {
                return Unauthorized();
            }
        }

        // PUT: api/releases/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putrelease(int id, release release)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != release.id)
            {
                return BadRequest();
            }

            db.Entry(release).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!releaseExists(id))
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

        // POST: api/releases
        [ResponseType(typeof(release))]
        public IHttpActionResult Postrelease(release release)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.release.Add(release);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = release.id }, release);
        }

        // DELETE: api/releases/5
        [ResponseType(typeof(release))]
        public IHttpActionResult Deleterelease(int id)
        {
            release release = db.release.Find(id);
            if (release == null)
            {
                return NotFound();
            }

            db.release.Remove(release);
            db.SaveChanges();

            return Ok(release);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool releaseExists(int id)
        {
            return db.release.Count(e => e.id == id) > 0;
        }
    }
}