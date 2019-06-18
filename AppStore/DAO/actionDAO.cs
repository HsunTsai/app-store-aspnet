using AppStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppStore.DAO
{
    public class ActionDAO
    {
        //取得該Action
        public static action getAction(AppStoreEntities db, int application_id, int action_id)
        {
            return (from dbActions in db.action
                    where dbActions.application_id == application_id &&
                    dbActions.action_id == action_id
                    select dbActions).SingleOrDefault();
        }

        //取得該應用程式所有的Action
        public static List<action> getApplicationActions(AppStoreEntities db, int application_id)
        {
            return (from dbActions in db.action where dbActions.application_id == application_id select dbActions).ToList();
        }
    }
}