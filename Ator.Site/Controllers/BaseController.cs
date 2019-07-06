using Ator.Model.Api;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
namespace Ator.Site
{
    public class BaseController: Controller
    {
        protected LayuiDataModel apiResult;
        protected string resStr = string.Empty;
        protected bool result = false;
        public static readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings() { DateFormatString = "yyyy-MM-dd HH:mm:ss" };//Json时间格式化

        #region 属性
        //Session属性
        public UserInfo _UserInfo
        {
            get
            {
                return new UserInfo(HttpContext);
            }
        }

        public string GuidKey
        {
            get
            {
                return Guid.NewGuid().ToString("N");
            }
        }
        /// <summary>
        /// 登陆用户名，未登陆成功为空
        /// </summary>
        public string UserName
        {
            get
            {
                return HttpContext.User.Claims.Where(o => o.Type == ClaimTypes.Name).FirstOrDefault()?.Value;
            }
        }
        /// <summary>
        /// 会员唯一编码，未登陆成功为空
        /// </summary>
        public string Id
        {
            get
            {
                return HttpContext.User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            }
        }

        /// <summary>
        /// 返回实体验证错误信息
        /// </summary>
        /// <returns></returns>
        public string GetModelErrMsg()
        {
            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    var modelstate = ModelState[key];
                    if (modelstate.Errors.Any())
                    {
                        return modelstate.Errors.FirstOrDefault().ErrorMessage;
                    }
                }
            }
            return string.Empty;
        }
        #endregion

        /// <summary>
        /// 成功返回单个数据object
        /// </summary>
        /// <param name="data"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IActionResult SuccessRes(object data, string msg = "", string code = "")
        {
            apiResult = new LayuiDataModel()
            {
                msg = msg,
                data = data,
                code = code
            };
            return Json(apiResult, jsonSerializerSettings);
        }

        public IActionResult SuccessRes(string msg = "")
        {
            apiResult = new LayuiDataModel()
            {
                msg = msg
            };
            return Json(apiResult);
        }

        public IActionResult SuccessRes<T>(List<T> data, string msg = "")
        {
            apiResult = new LayuiDataModel()
            {
                data = data,
                msg = msg,
                count = data.LongCount(),
            };
            return Json(apiResult);
        }

        public IActionResult SuccessRes<T>(List<T> data, long count, string msg = "")
        {
            apiResult = new LayuiDataModel()
            {
                data = data,
                msg = msg,
                count = count,
            };
            return Json(apiResult, jsonSerializerSettings);
        }

        public IActionResult ErrRes(string msg="", string code = "")
        {
            apiResult = new LayuiDataModel()
            {
                success = false,
                msg = msg,
                code = code
            };
            return Json(apiResult);
        }

        public IActionResult ResultRes(string msg, string data = "")
        {
            if (string.IsNullOrEmpty(msg))
            {
                return SuccessRes(data, "");
            }
            return ErrRes(msg);
        }

        public IActionResult ResultRes(bool result)
        {
            if (result)
            {
                return SuccessRes();
            }
            return ErrRes();
        }
        //登录数据保存
        public class UserInfo
        {
            public HttpContext httpContext;
            public string UserName
            {
                get
                {
                    return httpContext.Session.GetString("UserName");
                }
                set
                {
                    httpContext.Session.SetString("UserName", value);
                }
            }
            public string UserId
            {
                get
                {
                    return httpContext.Session.GetString("UserId");
                }
                set
                {
                    httpContext.Session.SetString("UserId", value);
                }
            }
            public string Avatar
            {
                get
                {
                    return httpContext.Session.GetString("Avatar");
                }
                set
                {
                    httpContext.Session.SetString("Avatar", value);
                }
            }
            public string UserUnit
            {
                get
                {
                    return httpContext.Session.GetString("UserUnit");
                }
                set
                {
                    httpContext.Session.SetString("UserUnit", value);
                }
            }
            public string UserType
            {
                get
                {
                    return httpContext.Session.GetString("UserType");
                }
                set
                {
                    httpContext.Session.SetString("UserType", value);
                }
            }
            public UserInfo(HttpContext context)
            {
                httpContext = context;
            }
        }
    }
}
