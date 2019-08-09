using AppStore.DAO;
using AppStore.Models;
using AppStore.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace AppStore.Controllers
{
    public class WTapplicationsController : ApiController
    {
        private AppStoreEntities db = new AppStoreEntities();

        // GET: api/applications
        [ResponseType(typeof(application))]
        public IHttpActionResult Getapplication()
        {
            return Ok(ApplicationDAO.getPublicApp(db));
        }

        // GET: api/applications/{application_id}
        [ResponseType(typeof(application))]
        public IHttpActionResult Getapplication(int id)
        {
            if (Service.isApplicationCanRead(db, "", id))
            {
                application app = ApplicationDAO.get(db, id);
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
    }
}
