using AppStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppStore.DAO
{
    public class TrackingDAO
    {
        //分頁取得該應用程式的tracking
        public static List<tracking> getTrackingList(AppStoreEntities db, int application_id, int offset, int count)
        {
            return (from dbTracking in db.tracking
                    where dbTracking.application_id == application_id
                    orderby dbTracking.time_stamp descending
                    select dbTracking).Skip(offset).Take(count).ToList();
        }
    }
}