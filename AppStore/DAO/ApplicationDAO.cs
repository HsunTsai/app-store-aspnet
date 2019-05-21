﻿using AppStore.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppStore.DAO
{
    public class ApplicationDAO
    {
        

        public static List<application> getPublicApp(AppStoreEntities db)
        {
            return (from dbAPPs in db.application where dbAPPs.privacy_type == "public" && dbAPPs.@lock == false select dbAPPs).ToList();
        }

        public static List<application> getUserApp(AppStoreEntities db, string user_id)
        {
            List<user_application> userApps = (from dbApps in db.user_application where user_id == dbApps.user_id select dbApps).ToList();
            if (userApps.Count() > 0)
            {
                string conditionApps = "";
                foreach (user_application userApp in userApps)
                {
                    conditionApps += (" id = " + userApp.id);
                }

                return db.Database.SqlQuery<application>(
                    "SELECT id,name,device_type,privacy_type,lock " +
                            "FROM application " +
                            "WHERE" + conditionApps).ToList();
            }
            else
            {
                return new List<application>();
            }
        }

        public static application get(AppStoreEntities db, int id)
        {
            return (from dbAPPs in db.application where dbAPPs.id == id select dbAPPs).SingleOrDefault();
        }
    }
}