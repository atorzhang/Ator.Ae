using Ator.Entity;
using Ator.Model;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ator.Repository
{
    public interface IRepository<T> where T : EntityDb
    {
        /// <summary>
        /// 数据操作类
        /// </summary>
        AeDbContext DbContext { get;}
        DatabaseFacade Database { get; }
        IQueryable<T> Entities { get; }
        bool Any(Expression<Func<T, bool>> whereLambd);
        int SaveChanges();
        Task<int> SaveChangesAsync();
        void Disposed();

        //添加
        bool Insert(List<T> entitys, bool isSaveChange = true);
        bool Insert(T entity, bool isSaveChange = true);
        Task<bool> InsertAsync(List<T> entitys, bool isSaveChange = true);
        Task<bool> InsertAsync(T entity, bool isSaveChange = true);

        //删除
        bool Delete(List<T> entitys, bool isSaveChange = true);
        bool Delete(T entity, bool isSaveChange = true);
        Task<bool> DeleteAsync(List<T> entitys, bool isSaveChange = true);
        Task<bool> DeleteAsync(T entity, bool isSaveChange = true);

        //修改
        bool Update(List<T> entitys, bool isSaveChange = true);
        bool Update(T entity, bool isSaveChange = true, List<string> updatePropertyList = null,bool modified = true);
        Task<bool> UpdateAsync(List<T> entitys, bool isSaveChange = true);
        Task<bool> UpdateAsync(T entity, bool isSaveChange = true, List<string> updatePropertyList = null, bool modified = true);

        //查询
        Task<List<T>> GetListAsync(Expression<Func<T, bool>> predicate = null,string ordering="", bool isNoTracking = true);
        List<T> GetList(Expression<Func<T, bool>> predicate = null, string ordering = "", bool isNoTracking = true);
        T Get(object id);
        T Get(Expression<Func<T, bool>> predicate = null, bool isNoTracking = true);
        Task<T> GetAsync(object id);
        Task<T> GetAsync(Expression<Func<T, bool>> predicate = null, bool isNoTracking = true);
        Task<IQueryable<T>> LoadAsync(Expression<Func<T, bool>> predicate = null, bool isNoTracking = true);
        IQueryable<T> Load(Expression<Func<T, bool>> predicate = null, bool isNoTracking = true);
        long Count(Expression<Func<T, bool>> predicate = null);
        Task<long> CountAsync(Expression<Func<T, bool>> predicate = null);
        //分页查找
        Task<PageData<T>> GetPageAsync<TKey>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderBy, int pageIndex, int pageSize,bool isOrder = true, bool isNoTracking = true);
        Task<PageData<T>> GetPageAsync(Expression<Func<T, bool>> whereLambda, string ordering, int pageIndex, int pageSize, bool isNoTracking = true);
        //Task<PageData<T>> GetPageAsync(Expression<Func<T, bool>> whereLambda, List<KeyValuePair<string, bool> >lstOrdering, int pageIndex, int pageSize, bool isNoTracking = true);
        PageData<T> GetPage(Expression<Func<T, bool>> whereLambda, string ordering, int pageIndex, int pageSize, bool isNoTracking = true);

        #region 旧的仓储
        //#region 查询实体

        //#region 异步获取单条实体
        ///// <summary>
        ///// 主键获取单挑数据
        ///// </summary>
        ///// <param name="key">主键</param>
        ///// <returns></returns>
        //Task<T> Get(string key);
        ///// <summary>
        ///// 表达式获取单条数据
        ///// </summary>
        ///// <param name="predicate">条件表达式树</param>
        ///// <returns></returns>
        //Task<T> Get(Expression<Func<T, bool>> predicate);
        //#endregion

        //#region 获取实体集合
        ///// <summary>
        ///// 获取IQueryable集合实体
        ///// </summary>
        ///// <param name="predicate"></param>
        ///// <returns></returns>
        //IQueryable<T> GetList(Expression<Func<T, bool>> predicate = null);

        //List<T> GetPage<TKey>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderBy, int pageIndex, int pageSize, out int rows, bool isOrder = true);

        //List<T> GetPage(Expression<Func<T, bool>> whereLambda, string ordering, int pageIndex, int pageSize, out int rows, bool isOrder = true);

        //List<T> GetPage(Expression<Func<T, bool>> whereLambda, SortedDictionary<string, bool> dicOrdering, int pageIndex, int pageSize, out int rows);
        //#endregion

        //#region 获取个数、判断对象是否存在
        ///// <summary>
        ///// 判断条件是否存在
        ///// </summary>
        ///// <param name="whereLambd"></param>
        ///// <returns></returns>
        //bool Any(Expression<Func<T, bool>> whereLambd);

        ///// <summary>
        ///// 个数
        ///// </summary>
        ///// <returns></returns>
        //long Count(Expression<Func<T, bool>> whereLambd = null);
        //#endregion

        //#endregion

        //#region 插入实体
        ///// <summary>
        ///// 插入单条实体
        ///// </summary>
        ///// <param name="entity"></param>
        //int Insert(T entity);
        ///// <summary>
        ///// 插入多条实体
        ///// </summary>
        ///// <param name="entitys"></param>
        //int Insert(List<T> entitys);
        ///// <summary>
        ///// 插入单条实体（事务未提交）
        ///// </summary>
        ///// <param name="entity"></param>
        //void InsertThen(T entity);
        ///// <summary>
        ///// 插入多条实体（事务未提交）
        ///// </summary>
        ///// <param name="entitys"></param>
        //void InsertThen(List<T> entitys);
        //#endregion

        //#region 更新实体
        ///// <summary>
        ///// 更新单条实体
        ///// </summary>
        ///// <param name="entity"></param>
        //int Update(T entity);
        ///// <summary>
        ///// 更新多条实体
        ///// </summary>
        ///// <param name="entitys"></param>
        //int Update(List<T> entitys);
        ///// <summary>
        ///// 更新单条实体（事务未提交）
        ///// </summary>
        ///// <param name="entity"></param>
        //void UpdateThen(T entity);
        ///// <summary>
        ///// 更新多条实体（事务未提交）
        ///// </summary>
        ///// <param name="entitys"></param>
        //void UpdateThen(List<T> entitys);
        //#endregion

        //#region 删除实体
        ///// <summary>
        ///// 删除单条实体
        ///// </summary>
        ///// <param name="entity"></param>
        //int Delete(T entity);
        ///// <summary>
        ///// 删除多条实体
        ///// </summary>
        ///// <param name="entitys"></param>
        //int Delete(List<T> entitys);
        ///// <summary>
        ///// 删除单条实体（事务未提交）
        ///// </summary>
        ///// <param name="entity"></param>
        //void DeleteThen(T entity);
        ///// <summary>
        ///// 删除多条实体（事务未提交）
        ///// </summary>
        ///// <param name="entitys"></param>
        //void DeleteThen(List<T> entitys);
        //#endregion 
        #endregion
    }
}
