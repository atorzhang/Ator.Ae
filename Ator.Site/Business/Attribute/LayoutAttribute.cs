using Ator.Repository;
using Ator.Site;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ator.Entity;
using Ator.Utility.CacheService;
using Ator.Entity.Sys;
using Microsoft.AspNetCore.Http;

namespace Ator.Site.Attribute
{
    //数据采集
    public class LayoutAttribute : ActionFilterAttribute, IResultFilter
    {
        UnitOfWork _unitOfWork = UnitOfWork.Instance;
        MemoryCacheService _memoryCacheService = MemoryCacheService.Instance;
        public LayoutAttribute()
        {
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var controller = (context.Controller) as Controller;
            var ViewBag = controller.ViewBag;
            ViewBag.UserName = context.HttpContext.Session.GetString("UserName");
            ViewBag.Avatar = context.HttpContext.Session.GetString("Avatar");
            ViewBag.UserId = context.HttpContext.Session.GetString("UserId");
            //获取设置值
            var settings = _memoryCacheService.Get<List<SysSetting>>("SiteSettings");
            if (settings == null || settings.Count == 0)
            {
                settings = _unitOfWork.SysSettingRepository.Load(o => o.SysSettingGroup.ToLower() == "blog").ToList();
                if (settings != null)
                {
                    _memoryCacheService.Add("SiteSettings", settings);
                }
            }
            //设置值
            ViewBag.SiteAddr = settings.FirstOrDefault(o => o.SysSettingId == "SiteAddr")?.SetValue;
            ViewBag.SiteWx = settings.FirstOrDefault(o => o.SysSettingId == "SiteWx")?.SetValue;
            ViewBag.SiteWeibo = settings.FirstOrDefault(o => o.SysSettingId == "SiteWeibo")?.SetValue;
            ViewBag.SiteEmail = settings.FirstOrDefault(o => o.SysSettingId == "SiteEmail")?.SetValue;
            ViewBag.SiteAuthor = settings.FirstOrDefault(o => o.SysSettingId == "SiteAuthor")?.SetValue;
            ViewBag.SiteQQ = settings.FirstOrDefault(o => o.SysSettingId == "SiteQQ")?.SetValue;
            ViewBag.SiteICP = settings.FirstOrDefault(o => o.SysSettingId == "SiteICP")?.SetValue;
            ViewBag.SiteDesign = settings.FirstOrDefault(o => o.SysSettingId == "SiteDesign")?.SetValue;
            ViewBag.SiteUrl = settings.FirstOrDefault(o => o.SysSettingId == "SiteUrl")?.SetValue;
            ViewBag.SiteName = settings.FirstOrDefault(o => o.SysSettingId == "SiteName")?.SetValue;

            //Logo
            var SiteLogo = _memoryCacheService.Get<string>("SiteLogo");
            if (string.IsNullOrEmpty(SiteLogo))
            {
                SiteLogo = _unitOfWork.SysLinkItemRepository.Get("9f17e9bafa1948dda8b7ab1918d18b16")?.SysLinkImg;
                if (!string.IsNullOrEmpty(SiteLogo))
                {
                    _memoryCacheService.Add("SiteLogo", SiteLogo);
                }
            }
            ViewBag.SiteLogo = SiteLogo;
        }
    }
}
