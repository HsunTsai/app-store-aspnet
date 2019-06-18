using AppStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppStore.DAO
{
    public class ReleaseDAO
    {
        //取得該應用程式所有的release資料
        public static List<release> getReleaseList(AppStoreEntities db, int application_id)
        {
            return (from dbRelease in db.release where dbRelease.application_id == application_id select dbRelease).ToList();
        }

        public static release getSingleRelease(AppStoreEntities db, int releae_id)
        {
            return db.release.Find(releae_id);
            //return (from dbRelease in db.release where dbRelease.id == releae_id select dbRelease).SingleOrDefault();
        }
    }
}