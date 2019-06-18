﻿using AppStore.Helpers;
using AppStore.Models;
using AppStore.Services;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace AppStore.Filters
{
    public class JwtAuthAttribute : AuthorizeAttribute
    {
        public string ErrorMessage { get; set; } = "";
        private AppStoreEntities db = new AppStoreEntities();

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            if (string.IsNullOrEmpty(ErrorMessage) == false)
            {
                setErrorResponse(actionContext, ErrorMessage);
            }
            else
            {
                base.HandleUnauthorizedRequest(actionContext);
            }
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            string jwtToken = null;
            #region 進行存取權杖的解碼(Authorization Header)
            AuthenticationHeaderValue authorization = actionContext.Request.Headers.Authorization;
            if (null == authorization || authorization.Scheme != "Bearer")
            {
                #region 進行存取權杖的解碼(Cookie)
                CookieHeaderValue cookies = actionContext.Request.Headers.GetCookies("jwt").FirstOrDefault();
                if (null != cookies && !string.IsNullOrEmpty(cookies["jwt"].Value))
                {
                    jwtToken = cookies["jwt"].Value;
                }
                #endregion
            }
            else
            {
                jwtToken = authorization.Parameter;
            }
            #endregion


            if (null == jwtToken)
            {
                setErrorResponse(actionContext, "token miss");  //沒有看到存取權杖錯誤
            }
            else
            {
                try
                {

                    #region 進行存取權杖的解碼(Header)
                    string secretKey = MainHelper.SecretKey;
                    var json = new JwtBuilder()
                        .WithAlgorithm(new HMACSHA256Algorithm())
                        .WithSecret(secretKey)
                        .MustVerifySignature()
                        .Decode<Dictionary<string, object>>(jwtToken);
                    #endregion

                    #region 將存取權杖所夾帶的內容取出來
                    var fooRole = json["role"] as Newtonsoft.Json.Linq.JArray;
                    var fooRoleList = fooRole.Select(x => (string)x).ToList<string>();
                    string fooUserId = json["iss"] as string;
                    #endregion

                    #region 將存取權杖的夾帶欄位，儲存到 HTTP 要求的屬性
                    actionContext.Request.Properties.Add("user", fooUserId);
                    actionContext.Request.Properties.Add("role", fooRoleList);
                    #endregion

                    #region 設定目前 HTTP 要求的安全性資訊
                    var fooPrincipal =
                        new GenericPrincipal(new GenericIdentity(fooUserId, "MyPassport"), fooRoleList.ToArray());
                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.User = fooPrincipal;
                    }
                    #endregion

                    #region 檢查user id是否存在
                    if (string.IsNullOrEmpty(fooUserId))
                    {
                        setErrorResponse(actionContext, "no user"); //無使用者id
                    }
                    #endregion

                    #region 角色權限檢查(檢查控制器或動作之屬性(Attribute上設的 Roles的設定內容)
                    //if (!string.IsNullOrEmpty(Roles))
                    //{
                    //    string[] roles = Roles.Split(',');
                    //    switch (roles[0])
                    //    {
                    //        case "application":
                    //            string application_id = HttpContext.Current.Request.Params["id"];
                    //            string userApplicationRole = Service.userApplicationRole(db, fooUserId, int.Parse(application_id));
                    //            if (!roles.Contains(userApplicationRole))
                    //            {
                    //                setErrorResponse(actionContext, "role error"); //無效的角色設定，沒有權限使用這個 API
                    //            }
                    //            break;
                            
                    //    }
                    //}
                    #endregion

                }
                catch (TokenExpiredException)
                {
                    setErrorResponse(actionContext, "token expired"); //權杖已經逾期
                }
                catch (SignatureVerificationException)
                {
                    setErrorResponse(actionContext, "token digital signature error"); //權杖似乎不正確，沒有正確的數位簽名
                }
                catch (Exception ex)
                {
                    setErrorResponse(actionContext, $"token error : {ex.Message}");
                }
            }

            base.OnAuthorization(actionContext);
        }

        private void setErrorResponse(HttpActionContext actionContext, string message)
        {
            ErrorMessage = message;
            var response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, message);
            response.Content = new ObjectContent<APIResult>(new APIResult()
            {
                success = false,
                message = ErrorMessage,
                payload = null
            }, new JsonMediaTypeFormatter());
            actionContext.Response = response;
        }
    }
}