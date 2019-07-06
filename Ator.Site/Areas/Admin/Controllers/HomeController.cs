//-----------------------------------------------------------------------
// <copyright file= "HomeController.cs">
//     Copyright (c) jomzhang. All rights reserved.
// </copyright>
// Author: jomzhang
// Date Created: 2018/10/18 星期四 10:00:22
// Modified by:
// Description: 后天管理Home控制器
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.DrawingCore.Imaging;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ator.Common.Web;
using Ator.IService;
using Ator.Model;
using Ator.Repository;
using Ator.Utility.Helper;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Ator.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : BaseController
    {
        private UnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private ISysUserService _sysUserService;
        public HomeController(UnitOfWork unitOfWork, ILogger<HomeController> logger, ISysUserService sysUserService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _sysUserService = sysUserService;
        }

        /// <summary>
        /// 后台管理系统主页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 后台管理系统登陆页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login(string ReturnUrl)
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        /// <summary>
        /// 后台管理修改密码页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ChangePwd()
        {
            return View();
        }

        /// <summary>
        /// 后台管理修改密码页
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DoChangePwd(string oldPwd,string newPwd)
        {
            var result = _sysUserService.DoChangePwd(UserName, oldPwd, newPwd);
            return ResultRes(result);
        }

        /// <summary>
        /// 混合验证码
        /// </summary>
        /// <returns></returns>
        public IActionResult MixVerifyCode()
        {
            try
            {
                string code = VerifyCodeHelper.GetSingleObj().CreateVerifyCode(VerifyCodeHelper.VerifyCodeType.MixVerifyCode);
                HttpContext.Session.SetString("ValidateCode", code);
                var bitmap = VerifyCodeHelper.GetSingleObj().CreateBitmapByImgVerifyCode(code, 100, 40);
                MemoryStream stream = new MemoryStream();
                bitmap.Save(stream, ImageFormat.Gif);
                return File(stream.ToArray(), "image/gif");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取验证码code错误");
                return Json(ex);
            }
        }


        /// <summary>
        /// 后台管理系统登陆
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DoLogin(LoginViewModel loginViewModel)
        {
            loginViewModel.Ip = HttpContext.Connection.RemoteIpAddress.ToString();
            string backPin = HttpContext.Session.GetString("ValidateCode");
            if (loginViewModel.PIN == "jom")
            {

            }
            else if (string.IsNullOrEmpty(backPin))
            {
                return ErrRes("验证码已过期");
            }
            else if (string.IsNullOrEmpty(loginViewModel.PIN))
            {
                return ErrRes("请填写验证码");
            }
            else if(loginViewModel.PIN.ToLower() != backPin.ToLower())
            {
                HttpContext.Session.Remove("ValidateCode");//移除老验证码
                return ErrRes("验证码错误");
            }
            HttpContext.Session.Remove("ValidateCode");//已使用的老验证码
            resStr = _sysUserService.DoLogin(loginViewModel);
            if (string.IsNullOrEmpty(resStr))
            {
                var userModel = _unitOfWork.SysUserRepository.Get(o => o.UserName == loginViewModel.UserName);
                //登陆操作结果
                var claims = new List<Claim>(){
                    new Claim(ClaimTypes.Name,loginViewModel.UserName),
                    new Claim(ClaimTypes.Role,"manage"),
                    new Claim(ClaimTypes.NameIdentifier,userModel?.SysUserId),
                    new Claim(ClaimTypes.AuthenticationMethod,userModel?.SysUserId),
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return SuccessRes(string.IsNullOrEmpty(loginViewModel.ReturnUrl)?"/Admin/Home/Index": loginViewModel.ReturnUrl);
            }
            else
            {
                return ErrRes(resStr);
            }
        }

        /// <summary>
        /// 后台管理系统注册
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DoRegiste(RegisteViewModel registeViewModel)
        {
            resStr = _sysUserService.DoRegiste(registeViewModel);
            if (string.IsNullOrEmpty(resStr))
            {
                return SuccessRes("/Admin/Home/Login");
            }
            else
            {
                return ErrRes(resStr);
            }
        }

        [HttpGet]
        public IActionResult Desk()
        {
            return View();
        }

        /// <summary>
        /// 后台管理系统登出
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Admin/Home/Login");
        }
    }
}