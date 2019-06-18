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
using AppStore.Utils;

namespace AppStore.Controllers
{
    public class actionsController : ApiController
    {
        private AppStoreEntities db = new AppStoreEntities();

        // GET: api/actions/{application_id}
        [ResponseType(typeof(tracking))]
        public IHttpActionResult Getaction(int id)
        {
            List<action> actions = ActionDAO.getApplicationActions(db, id);
            return Ok(actions);
        }

        // PUT: api/actions
        [ResponseType(typeof(void))]
        [JwtAuth]
        public IHttpActionResult Putaction(action action)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            string user_id = Request.Properties["user"] as string;
            if (Service.isApplicationCanModify(db, user_id, action.application_id))
            {
                action selectAction = ActionDAO.getAction(db, action.application_id, action.action_id);
                if (null == selectAction) return NotFound();
                if (null != action.name) selectAction.name = action.name;
                if (null != action.note) selectAction.note = action.note;

                db.Entry(selectAction).State = EntityState.Modified;
                db.SaveChanges();

                return Ok(selectAction);
            }
            else
            {
                return Content(HttpStatusCode.Unauthorized, new HttpMessage("insufficient_user_rights").toJson());
            }
        }

        // POST: api/actions
        [ResponseType(typeof(action))]
        [JwtAuth]
        public IHttpActionResult Postaction(action action)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            string user_id = Request.Properties["user"] as string;
            if (Service.isApplicationCanModify(db, user_id, action.application_id))
            {
                //若action_id在此APP已經擁有則不允許新增 & 若action_iod==9999也不允許新增
                List<action> appActionList = ActionDAO.getApplicationActions(db, action.application_id);
                if (action.action_id == 9999)
                {
                    return BadRequest("action_id_can_not_input_9999");
                }
                else if (appActionList.Count(e => e.action_id == action.action_id) > 0)
                {
                    return BadRequest("action_id_exist");
                }
                else
                {
                    db.action.Add(action);
                    db.SaveChanges();
                    return Ok(action);
                }
            }
            else
            {
                return Content(HttpStatusCode.Unauthorized, new HttpMessage("insufficient_user_rights").toJson());
            }
        }

        // DELETE: api/actions
        [ResponseType(typeof(action))]
        public IHttpActionResult Deleteaction(action action)
        {
            string user_id = Request.Properties["user"] as string;
            if (Service.isApplicationCanModify(db, user_id, action.application_id))
            {
                action selectAction = ActionDAO.getAction(db, action.application_id, action.action_id);
                if (null == selectAction) return NotFound();

                db.action.Remove(selectAction);
                db.SaveChanges();

                return Ok(selectAction);
            }
            else
            {
                return Content(HttpStatusCode.Unauthorized, new HttpMessage("insufficient_user_rights").toJson());
            }
        }
    }
}