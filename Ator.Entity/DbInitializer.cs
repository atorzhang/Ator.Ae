using Ator.Entity.Sys;
using Ator.Utility.Ext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ator.Entity
{
    public static class DbInitializer
    {
        public static void Initialize(AeDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.SysUsers.Any())
                return;
            SysUser admin = new SysUser()
            {
                UserName = "admin",
                Password = "123456".Md532(),
                Email = "459741134@qq.com",
                Mobile = "16605911669",
                QQ = "459741134",
                SysUserId = Guid.NewGuid().ToString("N"),
                TrueName = "Jom",
                NikeName = "超级管理员",
                CreateUser="admin",
                Remark=""
            };

            context.SysUsers.Add(admin);
            context.SaveChanges();
        }
    }
}
