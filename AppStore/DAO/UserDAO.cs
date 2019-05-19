using AppStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppStore.DAO
{
    public class UserDAO
    {
        public static user getUser(AppStoreEntities db, string user_id, string password)
        {
            return (from dbUser in db.user where user_id == dbUser.id && password == dbUser.password select dbUser).SingleOrDefault();
        }
    }
}