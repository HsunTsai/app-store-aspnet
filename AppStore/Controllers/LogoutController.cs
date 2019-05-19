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
    public class LogoutController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            JObject succeedmessage = new JObject();
            succeedmessage.Add("success", true);
            succeedmessage.Add("message", "succeed");
            succeedmessage.Add("payload", "");
            var response = Request.CreateResponse(HttpStatusCode.OK, succeedmessage);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            response.Headers.AddCookies(new[] { new CookieHeaderValue("jwt", "") });

            return ResponseMessage(response);
        }
    }
}
