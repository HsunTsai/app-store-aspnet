using AppStore.DAO;
using AppStore.Filters;
using AppStore.Helpers;
using AppStore.Models;
using JWT.Algorithms;
using JWT.Builder;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace AppStore.Controllers
{
    public class checkLoginController : ApiController
    {
        private AppStoreEntities db = new AppStoreEntities();

        [HttpGet]
        public IHttpActionResult Get()
        {
            //string user_id = Request.Properties["user"] as string;
            //// string jwtToken = Request.Headers.GetCookies("jwt").FirstOrDefault().ToString();
            //user searchUser = UserDAO.getUser(db, user_id);
            //JObject user = JObject.Parse(JsonConvert.SerializeObject(searchUser));
            //user.Remove("password");
            //return Ok(user);

            CookieHeaderValue cookies = Request.Headers.GetCookies("jwt").FirstOrDefault();
            string jwtToken = cookies["jwt"].Value;
            if (string.IsNullOrEmpty(jwtToken))
            {
                return BadRequest("not_login");
            }
            else
            {
                try
                {
                    var json = new JwtBuilder()
                                        .WithAlgorithm(new HMACSHA256Algorithm())
                                        .WithSecret(MainHelper.SecretKey)
                                        .MustVerifySignature()
                                        .Decode<Dictionary<string, object>>(jwtToken);

                    string user_id = json["iss"] as string;
                    user searchUser = UserDAO.getUser(db, user_id);
                    JObject user = JObject.Parse(JsonConvert.SerializeObject(searchUser));
                    user.Remove("password");
                    return Ok(user);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
        }
    }
}
