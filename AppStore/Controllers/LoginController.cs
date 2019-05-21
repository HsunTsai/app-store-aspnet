using AppStore.DAO;
using AppStore.Helpers;
using AppStore.Models;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace AppStore.Controllers
{
    public class loginController : ApiController
    {
        private IHttpActionResult response;
        private AppStoreEntities db = new AppStoreEntities();

        [HttpPost]
        public IHttpActionResult Post(user user)
        {
            if (null != user && null != user.id && null != user.password)
            {
                // 這裡可以修改成為與後端資料庫內的使用者資料表進行比對
                user searchUser = UserDAO.getUser(db, user.id, user.password);
                if (null != searchUser)
                {
                    string secretKey = MainHelper.SecretKey;
                    //設定該存取權杖的有效期限
                    IDateTimeProvider provider = new UtcDateTimeProvider();
                    // 這個 Access Token只有一個小時有效
                    var now = provider.GetNow().AddHours(1);
                    var unixEpoch = UnixEpoch.Value; // 1970-01-01 00:00:00 UTC
                    var secondsSinceEpoch = Math.Round((now - unixEpoch).TotalSeconds);

                    var jwtToken = new JwtBuilder()
                          .WithAlgorithm(new HMACSHA256Algorithm())
                          .WithSecret(secretKey)
                          .AddClaim("iss", searchUser.id)
                          .AddClaim("exp", secondsSinceEpoch)
                          .AddClaim("role", new string[] { searchUser.role.Trim() }) //new string[] { "user", "People" }
                          .Build();

                    // 帳號與密碼比對正確，回傳帳密比對正確

                    var cookie = new CookieHeaderValue("jwt", jwtToken);//Replace with your cookie

                    //Create response as usual
                    var response = Request.CreateResponse(HttpStatusCode.OK, new APIResult()
                    {
                        success = true,
                        message = "succeed",//$"帳號:{searchUser.account_id} / 密碼:{searchUser.password}",
                        payload = $"{jwtToken}"
                    });
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    response.Headers.AddCookies(new[] { cookie });

                    return ResponseMessage(response);
                }
                else
                {
                    // 帳號與密碼比對不正確，回傳帳密比對不正確
                    response = Ok(new APIResult()
                    {
                        success = false,
                        message = "Account or Password error", //帳號或密碼不正確
                        payload = ""
                    });
                }
                return response;
            }
            else
            {
                // 沒有收到正確格式的 Authorization 內容，回傳無法驗證訊息
                response = Ok(new APIResult()
                {
                    success = false,
                    message = "Account or Password should not be null", //帳號或密碼不得為空值
                    payload = ""
                });
                return response;
            }
        }
    }
}
