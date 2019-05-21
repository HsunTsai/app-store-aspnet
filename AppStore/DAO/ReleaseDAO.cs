using AppStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppStore.DAO
{
    public class ReleaseDAO
    {
        public static List<release> getAppRelease(AppStoreEntities db,int application_id)
        {
            return (from dbRelease in db.release where dbRelease.application_id == application_id select dbRelease).ToList();
        }
    }
}