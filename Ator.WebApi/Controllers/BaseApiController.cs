using Ator.Model.Api;
using IdentityModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ator.WebApi
{
    public class BaseApiController: Controller
    {
        protected ApiResultModel apiResult;
        protected string resStr = string.Empty;
        public static readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings() { DateFormatString = "yyyy-MM-dd HH:mm:ss" };//Json时间格式化

        #region 属性
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
                return HttpContext.User.Claims.Where(o => o.Type == JwtClaimTypes.Name).FirstOrDefault()?.Value;
            }
        }
        /// <summary>
        /// 会员唯一编码，未登陆成功为空
        /// </summary>
        public string Id
        {
            get
            {
                return HttpContext.User.Claims.Where(o => o.Type == JwtClaimTypes.Id).FirstOrDefault()?.Value;
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
        public IActionResult SuccessRes(object data, string msg = "", string type = "")
        {
            apiResult = new ApiResultModel()
            {
                Msg = msg,
                Data = data,
                Type = type
            };
            return Json(apiResult, jsonSerializerSettings);
        }

        public IActionResult SuccessRes(string msg = "")
        {
            apiResult = new ApiResultModel()
            {
                Msg = msg
            };
            return Json(apiResult);
        }

        public IActionResult SuccessRes(IEnumerable<object> data, string msg = "")
        {
            apiResult = new ApiResultModel()
            {
                Data = new ListData()
                {
                    ListInfo = data,
                    Total = data.Count()
                },
                Msg = msg
            };
            return Json(apiResult);
        }

        public IActionResult SuccessRes(IEnumerable<object> data, long total, string msg = "")
        {
            apiResult = new ApiResultModel()
            {
                Data = new ListData()
                {
                    ListInfo = data,
                    Total = total
                },
                Msg = msg
            };
            return Json(apiResult, jsonSerializerSettings);
        }

        public IActionResult ErrRes(string msg, string Type = "")
        {
            apiResult = new ApiResultModel()
            {
                Success = false,
                Msg = msg,
                Type = Type
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
    }
}
