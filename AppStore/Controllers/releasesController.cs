using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using AppStore.DAO;
using AppStore.Filters;
using AppStore.Models;
using AppStore.Services;
using AppStore.Utils;

namespace AppStore.Controllers
{
    public class releasesController : ApiController
    {
        private AppStoreEntities db = new AppStoreEntities();

        // GET: api/releases/{application_id}
        [ResponseType(typeof(release))]
        [JwtAuth]
        public IHttpActionResult Getrelease(int id)
        {
            string user_id = Request.Properties["user"] as string;
            if (Service.isApplicationCanRead(db, user_id, id))
            {
                List<release> releaseApps = ReleaseDAO.getReleaseList(db, id);
                return Ok(releaseApps);
            }
            else
            {
                return Content(HttpStatusCode.Unauthorized, new HttpMessage("insufficient_user_rights").toJson());
            }
        }

        // PUT: api/releases
        [ResponseType(typeof(void))]
        [JwtAuth]
        public IHttpActionResult Putrelease(release release)
        {
            // 檢查Model的狀態
            if (!ModelState.IsValid) return BadRequest(ModelState);

            string user_id = Request.Properties["user"] as string;
            if (Service.isApplicationCanModify(db, user_id, release.application_id))
            {
                release releaseApp = ReleaseDAO.getSingleRelease(db, release.id);
                if (null == releaseApp)
                {
                    return NotFound();
                }
                else
                {
                    try
                    {
                        if (null != release.environment_type) releaseApp.environment_type = release.environment_type;
                        if (null != release.notes) releaseApp.notes = release.notes;
                        releaseApp.icon_id = release.icon_id;
                        releaseApp.force_update = release.force_update;
                        releaseApp.@lock = release.@lock;

                        db.Entry(releaseApp).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        throw;
                    }
                }
                return Ok(releaseApp);
            }
            else
            {
                return Content(HttpStatusCode.Unauthorized, new HttpMessage("insufficient_user_rights").toJson());
            }
        }

        // POST: api/releases
        [ResponseType(typeof(release))]
        [JwtAuth]
        public async Task<HttpResponseMessage> Postrelease([FromUri()]release release)
        {
            //因multipart/form-data 無法夾帶參數 因此手動填入
            release.application_id = int.Parse(HttpContext.Current.Request.Form["application_id"]);
            release.environment_type = HttpContext.Current.Request.Form["environment_type"];
            release.device_type = HttpContext.Current.Request.Form["device_type"];
            release.version = HttpContext.Current.Request.Form["version"];
            release.version_code = int.Parse(HttpContext.Current.Request.Form["version_code"]);
            release.notes = HttpContext.Current.Request.Form["notes"];
            release.icon_id = int.Parse(HttpContext.Current.Request.Form["icon_id"]);
            release.force_update = bool.Parse(HttpContext.Current.Request.Form["force_update"]);

            // 檢查Model的狀態
            if (!ModelState.IsValid) return await Task.FromResult(Request.CreateResponse(HttpStatusCode.BadRequest, ModelState));

            string user_id = Request.Properties["user"] as string;
            if (Service.isApplicationCanModify(db, user_id, release.application_id))
            {
                application selectApp = ApplicationDAO.get(db, release.application_id);
                //查詢該應用程式是否存在
                if (null == selectApp)
                    return await Task.FromResult(Request.CreateResponse(HttpStatusCode.BadRequest, "application_not_exist"));
                //ios以外的平台不可以直接發布production
                if (!release.device_type.Equals("ios") && release.environment_type.Equals("production"))
                    return await Task.FromResult(Request.CreateResponse(HttpStatusCode.BadRequest, $"{release.device_type} can not release production"));

                //上傳的版本編號不得小於目前release列表中最大的版本編號
                release selectRelease = (from latestRelease in db.release
                                         where latestRelease.application_id == release.application_id &&
                                         latestRelease.environment_type == release.environment_type &&
                                         latestRelease.device_type == release.device_type &&
                                         latestRelease.@lock == false
                                         orderby latestRelease.version_code descending
                                         select latestRelease).FirstOrDefault();

                if (null == selectRelease || release.version_code > selectRelease.version_code)
                {
                    // 儲存檔案
                    HttpPostedFile file = UploadFile.uploadFile(selectApp, release);
                    release.size = file.ContentLength;
                    release.release_time = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                    release.@lock = false;

                    db.release.Add(release);
                }
                else
                {
                    if (release.version_code == selectRelease.version_code)
                    {
                        //該版本已經在線上 因此更新該版本資料
                        selectRelease.version = release.version;
                        selectRelease.notes = release.notes;
                        selectRelease.icon_id = release.icon_id;
                        selectRelease.force_update = release.force_update;
                        selectRelease.@lock = release.@lock;
                        db.Entry(selectRelease).State = EntityState.Modified;
                    }
                    else if (release.version_code < selectRelease.version_code)
                    {
                        return await Task.FromResult(Request.CreateResponse(HttpStatusCode.BadRequest,
                            "Your version_code need lager than " + selectRelease.version_code));
                    }
                }

                db.SaveChanges();
                return await Task.FromResult(Request.CreateResponse(HttpStatusCode.OK, null == selectRelease ? selectRelease : release));


                //更新靜態頁面
                //AppPage.update(device_id == 2 && environment_id == 2, releaseApp);
            }
            else
            {
                return await Task.FromResult(Request.CreateResponse(HttpStatusCode.Unauthorized));
            }
        }

        // DELETE: api/releases/5
        [ResponseType(typeof(release))]
        [JwtAuth]
        public IHttpActionResult Deleterelease(int id)
        {
            release selectRelesae = ReleaseDAO.getSingleRelease(db, id);
            if (null == selectRelesae) return NotFound();

            string user_id = Request.Properties["user"] as string;
            if (Service.isApplicationCanModify(db, user_id, selectRelesae.application_id))
            {
                db.release.Remove(selectRelesae);
                db.SaveChanges();
                return Ok(selectRelesae);
            }
            else
            {
                return Content(HttpStatusCode.Unauthorized, new HttpMessage("insufficient_user_rights").toJson());
            }
        }
    }
}