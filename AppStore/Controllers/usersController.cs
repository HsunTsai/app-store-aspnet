using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using AppStore.DAO;
using AppStore.Filters;
using AppStore.Models;
using AppStore.Utils;

namespace AppStore.Controllers
{
    public class usersController : ApiController
    {
        private AppStoreEntities db = new AppStoreEntities();

        // GET: api/users
        [ResponseType(typeof(void))]
        [JwtAuth]
        public IHttpActionResult Getuser()
        {
            List<user> userList = (from user in db.user where user.@lock == false select user).ToList();
            if (null == userList || userList.Count() == 0)
            {
                return BadRequest("user_list_empty");
            }
            else
            {
                foreach (user user in userList)
                {
                    user.password = "";
                }
                return Ok(userList);
            }
        }

        // PUT: api/users/5
        [ResponseType(typeof(user))]
        [JwtAuth]
        public IHttpActionResult Putuser(user user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            string user_id = Request.Properties["user"] as string;
            user searchUser = UserDAO.getUser(db, user_id);
            if (null == searchUser) return NotFound();

            if (user_id == user.id)
            {
                searchUser.password = user.password;

                db.Entry(searchUser).State = EntityState.Modified;
                db.SaveChanges();
                return Ok(searchUser);
            }
            else if (searchUser.role.Trim() == "admin")
            {
                //管理員權限可以變更所有使用者密碼及狀態
                user modifyUser = UserDAO.getUser(db, user.id);
                if (null != user.role) modifyUser.role = user.role;
                if (null != user.password) modifyUser.password = user.password;
                modifyUser.@lock = user.@lock;
                db.Entry(modifyUser).State = EntityState.Modified;
                db.SaveChanges();

                return Ok(modifyUser);
            }
            else
            {
                return Content(HttpStatusCode.Unauthorized, new HttpMessage("user_identity_error").toJson());
            }
        }

        // POST: api/users
        [ResponseType(typeof(user))]
        public IHttpActionResult Postuser(user user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (userExists(user.id)) return BadRequest("account_id_already_exist");

            user.role = "user"; //角色初始化強制user
            db.user.Add(user);
            db.SaveChanges();

            return Ok(user);
        }

        private bool userExists(string id)
        {
            return db.user.Count(e => e.id == id) > 0;
        }
    }
}