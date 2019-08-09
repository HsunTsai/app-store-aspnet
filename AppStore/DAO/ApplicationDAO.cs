using AppStore.Models;
using AppStore.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace AppStore.DAO
{
    public class ApplicationDAO
    {
        private static string applicationListQueryString(string condition)
        {
            return "SELECT application_id, name, description, i18n_id, privacy_type, application.lock, device_type, environment_type, max(version) as release_version, max(release.id) as release_id " +
                            "FROM release " +
                            "Left Join application On release.application_id = application.id " +
                            "WHERE release.lock=\'false\' AND" + (condition == null ? " " : (condition + " ")) +
                            "Group by application_id, name, description, i18n_id, privacy_type, application.lock, device_type, environment_type " +
                            "FOR JSON PATH";
        }

        // 取得單一APP
        public static application get(AppStoreEntities db, int application_id)
        {
            return (from dbAPPs in db.application where dbAPPs.id == application_id select dbAPPs).SingleOrDefault();
        }

        // 取得單一APP(包含所有清單)
        public static JObject get(AppStoreEntities db, int application_id, bool withDetail)
        {
            application App = get(db, application_id);
            JObject app = JObject.Parse(JsonConvert.SerializeObject(App));
            if (withDetail)
            {
                //管理APP的人員
                List<user_application> managers = (from dbUserApplication in db.user_application
                                                   where dbUserApplication.application_id == application_id && dbUserApplication.role == "manager"
                                                   select dbUserApplication).ToList();
                List<user_application> users = (from dbUserApplication in db.user_application
                                                where dbUserApplication.application_id == application_id && dbUserApplication.role == "user"
                                                select dbUserApplication).ToList();
                app.Add("managers", JArray.Parse(JsonConvert.SerializeObject(managers)));
                app.Add("users", JArray.Parse(JsonConvert.SerializeObject(users)));

                //各種設備列表
                //List<release> releaseListWindow32 = ReleaseDAO.getReleaseList(db, application_id, "win32", "development");
                //List<release> releaseListWindow64 = ReleaseDAO.getReleaseList(db, application_id, "win64", "development");
                //List<release> releaseListAndroid = ReleaseDAO.getReleaseList(db, application_id, "android", "development");
                //List<release> releaseListIOS = ReleaseDAO.getReleaseList(db, application_id, "ios", "development");
                //JObject release = new JObject();
                //release.Add("win32", JArray.Parse(JsonConvert.SerializeObject(releaseListWindow32)));
                //release.Add("win64", JArray.Parse(JsonConvert.SerializeObject(releaseListWindow64)));
                //release.Add("android", JArray.Parse(JsonConvert.SerializeObject(releaseListAndroid)));
                //release.Add("ios", JArray.Parse(JsonConvert.SerializeObject(releaseListIOS)));
                app.Add("release_type", ReleaseDAO.getReleaseType(db, application_id));
            }
            return app;
        }

        // 取得公開的APP(含最新釋出版本)
        public static JArray getPublicApp(AppStoreEntities db)
        {
            string results = db.Database.SqlQuery<string>(applicationListQueryString(
                " privacy_type = \'public\' AND application.lock = \'false\' AND environment_type = \'production\' ")).FirstOrDefault();
            return string.IsNullOrEmpty(results) ? new JArray() : JArray.Parse(results);
            // return (from dbAPPs in db.application where dbAPPs.privacy_type == "public" && dbAPPs.@lock == false select dbAPPs).ToList();
        }

        // 取得使用者的APP(含最新釋出版本)
        public static JArray getUserApps(AppStoreEntities db, string user_id)
        {
            List<user_application> userApps = (from dbApps in db.user_application where user_id == dbApps.user_id select dbApps).ToList();
            if (userApps.Count() > 0)
            {
                string conditionApps = "";
                for (int i = 0; i < userApps.Count(); ++i)
                {
                    conditionApps += (i == 0 ? " " : " or ");
                    conditionApps += ($"id = {userApps[i].application_id}");
                }
                List<application> applicationList = db.Database.SqlQuery<application>(
                    "SELECT id,name,description,i18n_id,privacy_type,lock " +
                            "FROM application " +
                            "WHERE" + conditionApps).ToList();

                JArray applications = JArray.Parse(JsonConvert.SerializeObject(applicationList));

                string results = db.Database.SqlQuery<string>(
                    applicationListQueryString(conditionApps.Replace("id = ", "application_id = "))).FirstOrDefault();

                return string.IsNullOrEmpty(results) ? new JArray() : JArray.Parse(results);

                //找出各APP的最新版本
                //applications

                //return db.Database.ExecuteSqlCommand("SELECT id,name,description,i18n_id,privacy_type,lock " +
                //            "FROM application " +
                //            "WHERE" + conditionApps + " FOR JSON PATH").ToString();
            }
            else
            {
                return new JArray(); //new HttpMessage("").toJson();
                //return new List<application>();
            }
        }


    }
}