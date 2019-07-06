using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ator.Entity;
using Ator.Repository;
using Ator.Utility.CacheService;
using Ator.Utility.Helper;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;
namespace Ator.Site
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();//添加内存缓存

            services.AddDistributedMemoryCache();
            services.AddSession();//添加Session

            services.AddDbContext<AeDbContext>(options => options.UseMySql(Configuration.GetConnectionString("SQLConnection")));///注入数据库mysql上下文

            services.AddTransient(typeof(UnitOfWork));//注入工作单元

            //手动注册服务
            //services.AddTransient(typeof(ISysUserService), typeof(SysUserService));//注入工作单元
            //添加缓存服务注册
            services.AddSingleton(typeof(ICacheService), typeof(MemoryCacheService));
            #region 通过反射注入各种服务
            //集中注册服务
            foreach (var item in ReflectionHelper.GetClassName("Ator.Service"))
            {
                foreach (var typeArray in item.Value)
                {
                    services.AddScoped(typeArray, item.Key);
                }
            }
            #endregion


            #region 认证配置
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                //Cookie的middleware配置
                .AddCookie(options =>
                {
                    options.LoginPath = new PathString("/Admin/Home/Login");
                    options.AccessDeniedPath = new PathString("/Admin/Home/Login");
                    //options.ExpireTimeSpan = //有效期
                });
            #endregion

            #region AutoMapper（实体转换）配置
            services.AddAutoMapper();
            #endregion

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();//json默认处理不会大小写变化
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";//默认时间处理格式，去除T
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();

            #region 请求错误提示配置
            //app.UseErrorHandling();
            #endregion

            #region Nlog配置
            loggerFactory.AddNLog();
            NLog.LogManager.LoadConfiguration("nlog.config");
            #endregion

            #region 认证配置
            app.UseAuthentication();
            #endregion

            #region 添加IP获取
            app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto }); 
            #endregion

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

    }
}
