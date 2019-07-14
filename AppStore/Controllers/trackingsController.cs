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
using Newtonsoft.Json.Linq;

namespace AppStore.Controllers
{
    public class trackingsController : ApiController
    {
        private AppStoreEntities db = new AppStoreEntities();

        // GET: api/trackings/{application_id}
        [ResponseType(typeof(tracking))]
        [JwtAuth]
        public IHttpActionResult Gettracking([FromUri()] int application_id, [FromUri()] int offset, [FromUri()] int count)
        {
            string user_id = Request.Properties["user"] as string;
            if (Service.isApplicationCanRead(db, user_id, application_id))
            {
                List<tracking> trackingList = TrackingDAO.getTrackingList(db, application_id, offset, count);
                return Ok(trackingList);
            }
            else
            {
                return Content(HttpStatusCode.Unauthorized, new HttpMessage("insufficient_user_rights").toJson());
            }
        }

        // POST: api/trackings
        [ResponseType(typeof(tracking))]
        public IHttpActionResult Posttracking(tracking[] trackings)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            foreach (tracking tracking in trackings)
            {
                //若端傳送的action id後端沒有相對應值 則預設為9999
                List<action> appActionList = ActionDAO.getApplicationActions(db, tracking.application_id);
                if (appActionList.Count(e => e.action_id == tracking.action_id) == 0) tracking.action_id = 9999;
                if (tracking.time_stamp == 0) tracking.time_stamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                db.tracking.Add(tracking);
            }

            db.SaveChanges();

            JObject message = new JObject();
            message.Add("messgae", trackings.Length + " items already insert");

            return Ok(message);
        }
    }
}