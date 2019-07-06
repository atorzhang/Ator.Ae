using Ator.Entity;
using Ator.Repository.Sys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ator.Repository
{
    public class UnitOfWork : IDisposable
    {
        public static UnitOfWork Instance = new UnitOfWork(new AeDbContext());
        public AeDbContext DbContext { get; set; } = null;
        public UnitOfWork(AeDbContext dbContext)
        {
            DbContext = dbContext;
        }

        #region 字段

        private SysDictionaryItemRepository _SysDictionaryItemRepository = null;
        private SysDictionaryRepository _SysDictionaryRepository = null;
        private SysLinkItemRepository _SysLinkItemRepository = null;
        private SysLinkTypeRepository _SysLinkTypeRepository = null;
        private SysPageRepository _SysPageRepository = null;
        private SysRolePageRepository _SysRolePageRepository = null;
        private SysRoleRepository _SysRoleRepository = null;
        private SysUserRepository _SysUserRepository = null;
        private SysUserRoleRepository _SysUserRoleRepository = null;
        private SysSettingRepository _SysSettingRepository = null;
        private SysCmsColumnRepository _SysCmsColumnRepository = null;
        private SysCmsInfoGoodRepository _SysCmsInfoGoodRepository = null;
        private SysCmsInfoRepository _SysCmsInfoRepository = null;
        private SysOperateRecordRepository _SysOperateRecordRepository = null;
        private SysCmsInfoCommentRepository _SysCmsInfoCommentRepository = null;

        
        #endregion

        #region 操作类属性
        public SysDictionaryItemRepository SysDictionaryItemRepository => _SysDictionaryItemRepository ?? (_SysDictionaryItemRepository = new SysDictionaryItemRepository(DbContext));
        public SysDictionaryRepository SysDictionaryRepository => _SysDictionaryRepository ?? (_SysDictionaryRepository = new SysDictionaryRepository(DbContext));
        public SysLinkItemRepository SysLinkItemRepository => _SysLinkItemRepository ?? (_SysLinkItemRepository = new SysLinkItemRepository(DbContext));
        public SysLinkTypeRepository SysLinkTypeRepository => _SysLinkTypeRepository ?? (_SysLinkTypeRepository = new SysLinkTypeRepository(DbContext));
        public SysPageRepository SysPageRepository => _SysPageRepository ?? (_SysPageRepository = new SysPageRepository(DbContext));
        public SysRolePageRepository SysRolePageRepository => _SysRolePageRepository ?? (_SysRolePageRepository = new SysRolePageRepository(DbContext));
        public SysRoleRepository SysRoleRepository => _SysRoleRepository ?? (_SysRoleRepository = new SysRoleRepository(DbContext));
        public SysUserRepository SysUserRepository => _SysUserRepository ?? (_SysUserRepository = new SysUserRepository(DbContext));
        public SysUserRoleRepository SysUserRoleRepository=> _SysUserRoleRepository ?? (_SysUserRoleRepository = new SysUserRoleRepository(DbContext));
        public SysSettingRepository SysSettingRepository => _SysSettingRepository ?? (_SysSettingRepository = new SysSettingRepository(DbContext));
        public SysCmsColumnRepository SysCmsColumnRepository => _SysCmsColumnRepository ?? (_SysCmsColumnRepository = new SysCmsColumnRepository(DbContext));
        public SysCmsInfoGoodRepository SysCmsInfoGoodRepository => _SysCmsInfoGoodRepository ?? (_SysCmsInfoGoodRepository = new SysCmsInfoGoodRepository(DbContext));
        public SysCmsInfoRepository SysCmsInfoRepository => _SysCmsInfoRepository ?? (_SysCmsInfoRepository = new SysCmsInfoRepository(DbContext));
        public SysCmsInfoCommentRepository SysCmsInfoCommentRepository => _SysCmsInfoCommentRepository ?? (_SysCmsInfoCommentRepository = new SysCmsInfoCommentRepository(DbContext));
        public SysOperateRecordRepository SysOperateRecordRepository => _SysOperateRecordRepository ?? (_SysOperateRecordRepository = new SysOperateRecordRepository(DbContext));
        #endregion

        #region 仓储操作（提交事务保存SaveChanges(),回滚RollBackChanges(),释放资源Dispose()）
        /// <summary>
        /// 保存
        /// </summary>
        public int SaveChanges()
        {
            return  DbContext.SaveChanges();
        }
        public async Task<int> SaveChangesAsync()
        {
            return await DbContext.SaveChangesAsync();
        }
        /// <summary>
        /// 回滚
        /// </summary>
        public void RollBackChanges()
        {
            var items = DbContext.ChangeTracker.Entries().ToList();
            items.ForEach(o => o.State = EntityState.Unchanged);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    DbContext.Dispose();//随着工作单元的销毁而销毁
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        public IDbContextTransaction BeginTransaction()
        {
            var scope = DbContext.Database.BeginTransaction();
            return scope;
        }

        //public List<T> SqlQuery<T>(string sql, object param = null) where T : class
        //{
        //    var con = DbContext.Database.GetDbConnection();
        //    if (con.State != ConnectionState.Open)
        //    {
        //        con.Open();
        //    }
        //    var list = MysqlDapperReader.SqlQuery<T>(con, sql, param);
        //    return list;
        //    //throw new NotImplementedException();
        //}

        //public Task<List<T>> SqlQueryAsync<T>(string sql, object param = null) where T : class
        //{
        //    throw new NotImplementedException();
        //}
        #endregion
    }
}
