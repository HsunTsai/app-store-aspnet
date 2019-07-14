using AppStore.DAO;
using AppStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppStore.Services
{
    public class Service
    {
        //取得使用者對於APP的權限
        public static string userApplicationRole(AppStoreEntities db, string user_id, int application_id)
        {
            user_application user_Application = (from dbApps in db.user_application where user_id == dbApps.user_id && application_id == dbApps.application_id select dbApps).SingleOrDefault();
            return null == user_Application ? "" : user_Application.role;
        }

        //public static string userAppStoreRole(AppStoreEntities db, string user_id)
        //{
        //    user user = (from users in db.user where user_id == users.id select users).SingleOrDefault();
        //    return null == user ? "" : user.role;
        //}

        //public static bool isApplicationInRole(AppStoreEntities db, string user_id, int application_id, string[] roles)
        //{
        //    return roles.Contains(userApplicationRole(db, user_id, application_id));
        //}

        //使用者是否可以讀取APP的資料
        public static bool isApplicationCanRead(AppStoreEntities db, string user_id, int application_id)
        {
            user user = UserDAO.getUser(db, user_id);
            if (user.role.Trim().Equals("admin")) return true;
            return userApplicationRole(db, user_id, application_id) == "manager" || userApplicationRole(db, user_id, application_id) == "user";
        }

        //使用者是否可以修改APP的資料
        public static bool isApplicationCanModify(AppStoreEntities db, string user_id, int application_id)
        {
            user user = UserDAO.getUser(db, user_id);
            if (user.role.Trim().Equals("admin")) return true;
            return userApplicationRole(db, user_id, application_id) == "manager";
        }

        //依照APPID尋找公開且沒被上鎖的APP是否存在
        public static bool isApplicationRublic(AppStoreEntities db, int application_id)
        {
            return (from dbAPPs in db.application where dbAPPs.privacy_type == "public" && dbAPPs.@lock == false && application_id == dbAPPs.id select dbAPPs).SingleOrDefault() != null;
        }
    }
}