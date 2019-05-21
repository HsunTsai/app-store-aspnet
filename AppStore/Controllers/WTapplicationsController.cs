using AppStore.DAO;
using AppStore.Models;
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

        // GET: api/applications/type
        [ResponseType(typeof(application))]
        public IHttpActionResult Getapplication()
        {
            return Ok(ApplicationDAO.getPublicApp(db));
        }
    }
}
