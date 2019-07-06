using Ator.Entity.Sys;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Entity
{
    public class AeDbContext : DbContext
    {
        public AeDbContext(DbContextOptions<AeDbContext> options) : base(options) { }

        public AeDbContext() { }


        #region 表对象
        public virtual DbSet<SysCmsColumn> SysCmsColumns { get; set; }
        public virtual DbSet<SysCmsInfo> SysCmsInfos { get; set; }
        public virtual DbSet<SysCmsInfoGood> SysCmsInfoGoods { get; set; }
        public virtual DbSet<SysCmsInfoComment> SysCmsInfoComments { get; set; }
        public virtual DbSet<SysUser> SysUsers { get; set; }
        public virtual DbSet<SysRole> SysRoles { get; set; }
        public virtual DbSet<SysUserRole> SysUserRoles { get; set; }
        public virtual DbSet<SysPage> SysPages { get; set; }
        public virtual DbSet<SysRolePage> SysRolePages { get; set; }
        public virtual DbSet<SysSetting> SysSettings { get; set; }
        public virtual DbSet<SysDictionary> SysDictionarys { get; set; }
        public virtual DbSet<SysDictionaryItem> SysDictionaryItems { get; set; }
        public virtual DbSet<SysLinkType> SysLinkTypes { get; set; }
        public virtual DbSet<SysLinkItem> SysLinkItem { get; set; }
        public virtual DbSet<SysOperateRecord> SysOperateRecords { get; set; }
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (!optionsBuilder.IsConfigured)
            {
                //ef配置数据库使用
                var builder = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                var configuration = builder.Build();

                string connectionString = configuration.GetConnectionString("SQLConnection");

                optionsBuilder.UseMySql(connectionString);

                //optionsBuilder.UseMySql(@"server=127.0.0.1;database=dbae;userid=root;pwd=123456;port=3306;sslmode=none;");
            }
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //做些fluntapi初始化



        }
    }
}
