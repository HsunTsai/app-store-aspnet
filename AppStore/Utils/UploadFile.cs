using AppStore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace AppStore.Utils
{
    public class UploadFile
    {
        public static HttpPostedFile uploadFile(application application, release release)
        {
            string saveFolder,fileExt;

            //檢查檔案是否存在
            HttpPostedFile uploadedApp = HttpContext.Current.Request.Files["file"];
            if (null == uploadedApp) throw setHttpException(HttpStatusCode.BadRequest, "file_not_exist");

            switch (release.device_type)
            {
                case "android":
                    fileExt = "apk";
                    break;
                case "win32":
                    fileExt = "exe";
                    break;
                case "win64":
                    fileExt = "exe";
                    break;
                case "ios":
                    fileExt = "ipa";
                    HttpPostedFile uploadedIOSPlist = HttpContext.Current.Request.Files["file_plist"];
                    if (null == uploadedIOSPlist) throw setHttpException(HttpStatusCode.BadRequest, "file_plist_not_exist");
                    break;
                default:
                    throw setHttpException(HttpStatusCode.BadRequest, "device_type_error");
            }

            //檢查資料夾是否存在 不存在則創建
            saveFolder = HttpContext.Current.Server.MapPath("~/") + $"app/{application.id}/{release.device_type}";
            if (!Directory.Exists(saveFolder)) Directory.CreateDirectory(saveFolder);

            //Debug.WriteLine("長度", HttpContext.Current.Request.Files.Count);

            HttpPostedFile file = HttpContext.Current.Request.Files.Count > 0 ? HttpContext.Current.Request.Files[0] : null;
            HttpPostedFile plistFile = HttpContext.Current.Request.Files.Count > 1 ? HttpContext.Current.Request.Files[1] : null;

            if (file != null && file.ContentLength > 0)
            {
                string appName = $"{application.name}_{release.version_code}_{release.version}.{fileExt}";
                file.SaveAs(Path.Combine(saveFolder, appName));

                if (release.device_type.Equals("ios")&& null != plistFile)
                {
                    string plistName = $"{application.name}_{release.version_code}_{release.version}.plist";
                    plistFile.SaveAs(Path.Combine(saveFolder, plistName));
                }

                return file;
            }
            else
            {
                throw setHttpException(HttpStatusCode.BadRequest, "file_stream_null");
            }
        }

        private static HttpResponseException setHttpException(HttpStatusCode httpStatusCodeCode, string message)
        {
            var response = new HttpResponseMessage(httpStatusCodeCode)
            {
                Content = new StringContent(message),
            };
            return new HttpResponseException(response);
        }
    }
}