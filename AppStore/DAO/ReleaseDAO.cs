using AppStore.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppStore.DAO
{
    public class ReleaseDAO
    {
        //依照應用程式id取得該應用程式所有的release種類
        public static JArray getReleaseType(AppStoreEntities db, int application_id)
        {
            string result = db.Database.SqlQuery<string>(
                "SELECT device_type FROM release WHERE application_id = " + application_id + " GROUP BY device_type FOR JSON PATH").FirstOrDefault();
            if (string.IsNullOrEmpty(result))
            {
                return new JArray();
            }
            else
            {
                JArray resultTypes = JArray.Parse(result);
                JArray results = new JArray();
                foreach (JObject type in resultTypes)
                {
                    results.Add(type.GetValue("device_type"));
                }
                return results;
            }
        }

        //取得該應用程式所有的release資料
        public static List<release> getReleaseList(AppStoreEntities db, int application_id)
        {
            return (from dbRelease in db.release where dbRelease.application_id == application_id select dbRelease).ToList();
        }

        //依照設備種類取得該應用程式release列表
        public static List<release> getReleaseList(AppStoreEntities db, int application_id, string device_type, string environment_type)
        {
            return (from dbRelease in db.release
                    where dbRelease.application_id == application_id &&
                    (string.IsNullOrEmpty(device_type) ? 1 == 1 : dbRelease.device_type == device_type) &&
                    (string.IsNullOrEmpty(environment_type) ? 1 == 1 : dbRelease.environment_type == environment_type)
                    orderby dbRelease.version_code descending
                    select dbRelease).ToList();
        }

        //取得單一Release資料
        public static release getSingleRelease(AppStoreEntities db, int releae_id)
        {
            return db.release.Find(releae_id);
            //return (from dbRelease in db.release where dbRelease.id == releae_id select dbRelease).SingleOrDefault();
        }
    }
}