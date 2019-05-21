using AppStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppStore.Services
{
    public class Service
    {
        public static bool isApplicationCanModify(AppStoreEntities db, string user_id, int application_id)
        {
            user_application user_Application = (from dbApps in db.user_application where user_id == dbApps.user_id && application_id == dbApps.application_id select dbApps).SingleOrDefault();
            return null != user_Application && user_Application.role == "manager";
        }

        public static bool isApplicationCanRead(AppStoreEntities db, string user_id, int application_id)
        {
            user_application user_Application = (from dbApps in db.user_application where user_id == dbApps.user_id && application_id == dbApps.application_id select dbApps).SingleOrDefault();
            return null != user_Application && (user_Application.role == "manager" || user_Application.role == "user");
        }

        //依照APPID尋找公開且沒被上鎖的APP是否存在
        public static bool isApplicationRublic(AppStoreEntities db, int application_id)
        {
            return (from dbAPPs in db.application where dbAPPs.privacy_type == "public" && dbAPPs.@lock == false && application_id == dbAPPs.id select dbAPPs).SingleOrDefault() != null;
        }
    }
}