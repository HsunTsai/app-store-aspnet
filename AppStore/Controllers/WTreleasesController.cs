﻿using AppStore.DAO;
using AppStore.Models;
using AppStore.Services;
using AppStore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace AppStore.Controllers
{
    public class WTreleasesController : ApiController
    {
        private AppStoreEntities db = new AppStoreEntities();

        // GET: api/WTreleases/{application_id}
        public IHttpActionResult Getrelease(int id)
        {
            if (Service.isApplicationRublic(db, id))
            {
                List<release> releaseApps = ReleaseDAO.getAppRelease(db, id);
                return Ok(releaseApps);
            }
            else
            {
                return BadRequest(new HttpMessage("application_lock_private_notexist").toString());
            }
        }
    }
}
