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
using AppStore.Models;

namespace AppStore.Controllers
{
    public class trackingsController : ApiController
    {
        private AppStoreEntities db = new AppStoreEntities();

        // GET: api/trackings
        public IQueryable<tracking> Gettracking()
        {
            return db.tracking;
        }

        // GET: api/trackings/5
        [ResponseType(typeof(tracking))]
        public IHttpActionResult Gettracking(int id)
        {
            tracking tracking = db.tracking.Find(id);
            if (tracking == null)
            {
                return NotFound();
            }

            return Ok(tracking);
        }

        // PUT: api/trackings/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttracking(int id, tracking tracking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tracking.id)
            {
                return BadRequest();
            }

            db.Entry(tracking).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!trackingExists(id))
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

        // POST: api/trackings
        [ResponseType(typeof(tracking))]
        public IHttpActionResult Posttracking(tracking tracking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tracking.Add(tracking);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tracking.id }, tracking);
        }

        // DELETE: api/trackings/5
        [ResponseType(typeof(tracking))]
        public IHttpActionResult Deletetracking(int id)
        {
            tracking tracking = db.tracking.Find(id);
            if (tracking == null)
            {
                return NotFound();
            }

            db.tracking.Remove(tracking);
            db.SaveChanges();

            return Ok(tracking);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool trackingExists(int id)
        {
            return db.tracking.Count(e => e.id == id) > 0;
        }
    }
}