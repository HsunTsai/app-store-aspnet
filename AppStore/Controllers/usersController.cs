using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using AppStore.Models;
using AppStore.Utils;

namespace AppStore.Controllers
{
    public class UsersController : ApiController
    {
        private AppStoreEntities db = new AppStoreEntities();

        // GET: api/users
        public IHttpActionResult Getuser()
        {
            List<user> userList = (from user in db.user select user).ToList();
            if (null == userList || userList.Count() == 0)
            {
                return BadRequest(new HttpMessage("user_list_empty").toString());
            }
            else
            {
                return Ok(userList);
            }
        }


        // PUT: api/users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putuser(user user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            user searchUser = (from dbUser in db.user where user.account_id == dbUser.account_id select user).Single();

            if (null == searchUser) return NotFound();

            searchUser.password = user.password;

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Ok(new HttpMessage("update_succeed").toString());

            //return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/users
        [ResponseType(typeof(user))]
        public IHttpActionResult Postuser(user user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (userExists(user.account_id))
            {
                db.user.Add(user);
                db.SaveChanges();
            }
            else
            {
                return BadRequest(new HttpMessage("account_id_conflict").toString());
            }

            return Ok(new HttpMessage("create_succeed").toString());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool userExists(string account_id)
        {
            return db.user.Count(e => e.account_id == account_id) > 0;
        }
    }
}