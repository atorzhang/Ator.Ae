using Ator.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using Microsoft.EntityFrameworkCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Ator.Model;
using System.IO;

namespace Ator.Repository
{
    public class RepositoryBase<T> : IRepository<T> where T : EntityDb
    {
        private readonly DbSet<T> _dbSet;
        public AeDbContext DbContext { get; } = null;
        public RepositoryBase(AeDbContext context)
        {
            DbContext = context;
            _dbSet = DbContext.Set<T>();
        }
        public DatabaseFacade Database => DbContext.Database;
        public IQueryable<T> Entities => _dbSet.AsQueryable().AsNoTracking();
        public int SaveChanges()
        {
            return DbContext.SaveChanges();
        }
        public async Task<int> SaveChangesAsync()
        {
            return await DbContext.SaveChangesAsync();
        }
        public bool Any(Expression<Func<T, bool>> whereLambd)
        {
            return _dbSet.Where(whereLambd).Any();
        }
        public void Disposed()
        {
            throw new Exception("不允许在这里释放上下文，请在UnitOfWork中操作");
            //DbContext.Dispose();
        }

        #region 插入数据
        public bool Insert(T entity, bool isSaveChange = true)
        {
            _dbSet.Add(entity);
            if (isSaveChange)
            {
                return SaveChanges() > 0;
            }
            return false;
        }
        public async Task<bool> InsertAsync(T entity, bool isSaveChange = true)
        {
            _dbSet.Add(entity);
            if (isSaveChange)
            {
                return await SaveChangesAsync() > 0;
            }
            return false;
        }
        public bool Insert(List<T> entitys, bool isSaveChange = true)
        {
            _dbSet.AddRange(entitys);
            if (isSaveChange)
            {
                return SaveChanges() > 0;
            }
            return false;
        }
        public async Task<bool> InsertAsync(List<T> entitys, bool isSaveChange = true)
        {
            _dbSet.AddRange(entitys);
            if (isSaveChange)
            {
                return await SaveChangesAsync() > 0;
            }
            return false;
        }
        #endregion

        #region 删除
        public bool Delete(T entity, bool isSaveChange = true)
        {
            _dbSet.Attach(entity);
            _dbSet.Remove(entity);
            return isSaveChange ? SaveChanges() > 0 : false;
        }
        public bool Delete(List<T> entitys, bool isSaveChange = true)
        {
            entitys.ForEach(entity =>
            {
                _dbSet.Attach(entity);
                _dbSet.Remove(entity);
            });
            return isSaveChange ? SaveChanges() > 0 : false;
        }

        public virtual async Task<bool> DeleteAsync(T entity, bool isSaveChange = true)
        {

            _dbSet.Attach(entity);
            _dbSet.Remove(entity);
            return isSaveChange ? await SaveChangesAsync() > 0 : false;
        }
        public virtual async Task<bool> DeleteAsync(List<T> entitys, bool isSaveChange = true)
        {
            entitys.ForEach(entity =>
            {
                _dbSet.Attach(entity);
                _dbSet.Remove(entity);
            });
            return isSaveChange ? await SaveChangesAsync() > 0 : false;
        }
        #endregion

        #region 更新数据
        public bool Update(T entity, bool isSaveChange = true, List<string> updatePropertyList = null, bool modified = true)
        {
            if (entity == null)
            {
                return false;
            }
            _dbSet.Attach(entity);
            var entry = DbContext.Entry(entity);
            if (updatePropertyList == null)
            {
                entry.State = EntityState.Modified;//全字段更新
            }
            else
            {
                if (modified)
                {
                    updatePropertyList.ForEach(c => {
                        entry.Property(c).IsModified = true; //部分字段更新的写法
                    });
                }
                else
                {
                    entry.State = EntityState.Modified;//全字段更新
                    updatePropertyList.ForEach(c => {
                        entry.Property(c).IsModified = false; //部分字段不更新的写法
                    });
                }
            }
            if (isSaveChange)
            {
                return SaveChanges() > 0;
            }
            return false;
        }
        public bool Update(List<T> entitys, bool isSaveChange = true)
        {
            if (entitys == null || entitys.Count == 0)
            {
                return false;
            }
            entitys.ForEach(c => {
                Update(c, false);
            });
            if (isSaveChange)
            {
                return SaveChanges() > 0;
            }
            return false;
        }
        public async Task<bool> UpdateAsync(T entity, bool isSaveChange = true, List<string> updatePropertyList = null, bool modified = true)
        {
            if (entity == null)
            {
                return false;
            }
            _dbSet.Attach(entity);
            var entry = DbContext.Entry<T>(entity);
            if (updatePropertyList == null)
            {
                entry.State = EntityState.Modified;//全字段更新
            }
            else
            {
                if (modified)
                {
                    updatePropertyList.ForEach(c => {
                        entry.Property(c).IsModified = true; //部分字段更新的写法
                    });
                }
                else
                {
                    entry.State = EntityState.Modified;//全字段更新
                    updatePropertyList.ForEach(c => {
                        entry.Property(c).IsModified = false; //部分字段不更新的写法
                    });
                }
            }
            if (isSaveChange)
            {
                return await SaveChangesAsync() > 0;
            }
            return false;
        }
        public async Task<bool> UpdateAsync(List<T> entitys, bool isSaveChange = true)
        {
            if (entitys == null || entitys.Count == 0)
            {
                return false;
            }
            entitys.ForEach(c => {
                _dbSet.Attach(c);
                DbContext.Entry<T>(c).State = EntityState.Modified;
            });
            if (isSaveChange)
            {
                return await SaveChangesAsync() > 0;
            }
            return false;
        }
        #endregion

        #region 查找
        public long Count(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate == null)
            {
                predicate = c => true;
            }
            return  _dbSet.LongCount(predicate);
        }
        public async Task<long> CountAsync(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate == null)
            {
                predicate = c => true;
            }
            return await _dbSet.LongCountAsync(predicate);
        }
        public T Get(object id)
        {
            if(id == null)
            {
                return default(T);
            }
            return _dbSet.Find(id);
        }
        public T Get(Expression<Func<T, bool>> predicate = null, bool isNoTracking = true)
        {
            var data = isNoTracking ? _dbSet.Where(predicate).AsNoTracking() : _dbSet.Where(predicate);
            return data.FirstOrDefault();
        }
        public async Task<T> GetAsync(object id)
        {
            if (id == null)
            {
                return default(T);
            }
            return await _dbSet.FindAsync(id);
        }
        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate = null, bool isNoTracking = true)
        {
            var data = isNoTracking ? _dbSet.Where(predicate).AsNoTracking() : _dbSet.Where(predicate);
            return await data.FirstOrDefaultAsync();
        }
        public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> predicate = null,string ordering = "", bool isNoTracking = true)
        {
            var data = isNoTracking? _dbSet.Where(predicate).AsNoTracking() : _dbSet.Where(predicate);
            if (!string.IsNullOrEmpty(ordering))
            {
                data = data.OrderByBatch(ordering);
            }
            return await data.ToListAsync();
        }
        public List<T> GetList(Expression<Func<T, bool>> predicate = null, string ordering = "", bool isNoTracking = true)
        {
            var data = isNoTracking ? _dbSet.Where(predicate).AsNoTracking() : _dbSet.Where(predicate);
            if (!string.IsNullOrEmpty(ordering))
            {
                data = data.OrderByBatch(ordering);
            }
            return data.ToList();
        }
        public async Task<IQueryable<T>> LoadAsync(Expression<Func<T, bool>> predicate = null, bool isNoTracking = true)
        {
            if (predicate == null)
            {
                predicate = c => true;
            }
            return await Task.Run(() => isNoTracking ? _dbSet.Where(predicate).AsNoTracking(): _dbSet.Where(predicate));
        }
        public IQueryable<T> Load(Expression<Func<T, bool>> predicate = null, bool isNoTracking = true)
        {
            if (predicate == null)
            {
                predicate = c => true;
            }
            return isNoTracking ? _dbSet.Where(predicate).AsNoTracking() : _dbSet.Where(predicate);
        }
        #region 分页查找
        /// <summary>
        /// 分页查询异步
        /// </summary>
        /// <param name="whereLambda">查询添加（可有，可无）</param>
        /// <param name="ordering">排序条件（一定要有）</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="isOrder">排序正反</param>
        /// <returns></returns>
        public async Task<PageData<T>> GetPageAsync<TKey>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderBy, int pageIndex, int pageSize, bool isOrder = true, bool isNoTracking = true)
        {
            IQueryable<T> data = isOrder ?
                _dbSet.OrderBy(orderBy) :
                _dbSet.OrderByDescending(orderBy);

            if (whereLambda != null)
            {
                data = isNoTracking ? data.Where(whereLambda).AsNoTracking() : data.Where(whereLambda);
            }
            PageData<T> pageData = new PageData<T>
            {
                Totals = await data.CountAsync(),
                Rows = await data.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync()
            };
            return pageData;
        }

        /// <summary>
        /// 分页查询异步
        /// </summary>
        /// <param name="whereLambda">查询添加（可有，可无）</param>
        /// <param name="ordering">排序条件（一定要有，多个用逗号隔开，倒序开头用-号）</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns></returns>
        public async Task<PageData<T>> GetPageAsync(Expression<Func<T, bool>> whereLambda, string ordering, int pageIndex, int pageSize, bool isNoTracking = true)
        {
            // 分页 一定注意： Skip 之前一定要 OrderBy
            if (string.IsNullOrEmpty(ordering))
            {
                ordering = nameof(T) + "Id";//默认以Id排序
            }
            var data = _dbSet.OrderByBatch(ordering);
            if (whereLambda != null)
            {
                data = isNoTracking ? data.Where(whereLambda).AsNoTracking() : data.Where(whereLambda);
            }
            //查看生成的sql，找到大数据下分页巨慢原因为order by 耗时
            //var sql = data.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToSql();
            //File.WriteAllText(@"D:\sql.txt",sql);
            PageData <T> pageData = new PageData<T>
            {
                Totals = await data.CountAsync(),
                Rows = await data.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync()
            };
            return pageData;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="whereLambda">查询添加（可有，可无）</param>
        /// <param name="ordering">排序条件（一定要有，多个用逗号隔开，倒序开头用-号）</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns></returns>
        public PageData<T> GetPage(Expression<Func<T, bool>> whereLambda, string ordering, int pageIndex, int pageSize, bool isNoTracking = true)
        {
            // 分页 一定注意： Skip 之前一定要 OrderBy
            if (string.IsNullOrEmpty(ordering))
            {
                ordering = nameof(T) + "Id";//默认以Id排序
            }
            var data = _dbSet.OrderByBatch(ordering);
            if (whereLambda != null)
            {
                data = isNoTracking ? data.Where(whereLambda).AsNoTracking() : data.Where(whereLambda);
            }
            PageData<T> pageData = new PageData<T>
            {
                Totals = data.Count(),
                Rows = data.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList()
            };
            return pageData;
        }

        #region 舍弃
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="whereLambda">查询添加（可有，可无）</param>
        /// <param name="lstOrdering">多个排序条件[字段,正反排序]（一定要有）,貌似不行</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns></returns>
        //public async Task<PageData<T>> GetPageAsync(Expression<Func<T, bool>> whereLambda, List<KeyValuePair<string, bool>> lstOrdering, int pageIndex, int pageSize, bool isNoTracking = true)
        //{
        //    IQueryable<T> data = _dbSet;
        //    bool frist = true;
        //    foreach (var item in lstOrdering)
        //    {
        //        if (frist)
        //        {
        //            if (item.Value)
        //            {
        //                data = data.OrderBy(item.Key);
        //            }
        //            //反序排序
        //            else
        //            {
        //                data = data.OrderByDescending(item.Key);
        //            }
        //            frist = false;
        //        }
        //        else
        //        {
        //            //正序排序
        //            if (item.Value)
        //            {
        //                data = data.ThenBy(item.Key);
        //            }
        //            //反序排序
        //            else
        //            {
        //                data = data.ThenByDescending(item.Key);
        //            }
        //        }
        //    }
        //    if (whereLambda != null)
        //    {
        //        data = isNoTracking ? data.Where(whereLambda).AsNoTracking() : data.Where(whereLambda);
        //    }
        //    PageData<T> pageData = new PageData<T>
        //    {
        //        Totals = await data.CountAsync(),
        //        Rows = await data.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync()
        //    };
        //    return pageData;
        //}  
        #endregion

        #endregion


        #endregion

        #region 旧的实现

        //#region 查询实体

        //#region 异步获取单条实体
        ///// <summary>
        ///// 主键获取单挑数据
        ///// </summary>
        ///// <param name="key">主键</param>
        ///// <returns></returns>
        //public async Task<T> Get(string key)
        //{
        //    return await _dbSet.FindAsync(key);
        //}
        ///// <summary>
        ///// 表达式获取单条数据
        ///// </summary>
        ///// <param name="predicate">条件表达式树</param>
        ///// <returns></returns>
        //public async Task<T> Get(Expression<Func<T, bool>> predicate)
        //{
        //    return await _dbSet.FirstOrDefaultAsync(predicate);
        //}
        //#endregion

        //#region 获取实体集合
        ///// <summary>
        ///// 获取所有数据IQueryable
        ///// </summary>
        ///// <param name="predicate"></param>
        ///// <returns></returns>
        //public IQueryable<T> GetList(Expression<Func<T, bool>> predicate = null)
        //{
        //    if (predicate == null)
        //    {
        //        return _dbSet;
        //    }
        //    return _dbSet.Where(predicate);
        //}

        ///// <summary>
        ///// 分页查询
        ///// </summary>
        ///// <param name="whereLambda">查询添加（可有，可无）</param>
        ///// <param name="ordering">排序条件（一定要有）</param>
        ///// <param name="pageIndex">当前页码</param>
        ///// <param name="pageSize">每页大小</param>
        ///// <param name="rows">总条数</param>
        ///// <param name="isOrder">排序正反</param>
        ///// <returns></returns>
        //public List<T> GetPage<TKey>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderBy, int pageIndex, int pageSize, out int rows,  bool isOrder = true)
        //{
        //    IQueryable<T> data = isOrder ?
        //        _dbSet.OrderBy(orderBy) :
        //        _dbSet.OrderByDescending(orderBy);

        //    if (whereLambda != null)
        //    {
        //        data = data.Where(whereLambda);
        //    }
        //    rows = data.Count();
        //    return  data.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        //}

        ///// <summary>
        ///// 分页查询
        ///// </summary>
        ///// <param name="whereLambda">查询添加（可有，可无）</param>
        ///// <param name="ordering">排序条件（一定要有）</param>
        ///// <param name="pageIndex">当前页码</param>
        ///// <param name="pageSize">每页大小</param>
        ///// <param name="rows">总条数</param>
        ///// <param name="isOrder">排序正反</param>
        ///// <returns></returns>
        //public List<T> GetPage(Expression<Func<T, bool>> whereLambda, string ordering, int pageIndex, int pageSize, out int rows, bool isOrder = true)
        //{
        //    // 分页 一定注意： Skip 之前一定要 OrderBy
        //    var data = isOrder ? _dbSet.OrderBy(ordering) : _dbSet.OrderByDescending(ordering);
        //    if (whereLambda != null)
        //    {
        //        data = data.Where(whereLambda);
        //    }
        //    rows = data.Count();
        //    return data.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        //}

        ///// <summary>
        ///// 分页查询
        ///// </summary>
        ///// <param name="whereLambda">查询添加（可有，可无）</param>
        ///// <param name="dicOrdering">多个排序条件[字段,正反排序]（一定要有）</param>
        ///// <param name="pageIndex">当前页码</param>
        ///// <param name="pageSize">每页大小</param>
        ///// <param name="rows">总条数</param>
        ///// <returns></returns>
        //public List<T> GetPage(Expression<Func<T, bool>> whereLambda, SortedDictionary<string, bool> dicOrdering, int pageIndex, int pageSize, out int rows)
        //{
        //    IQueryable<T> data = null;
        //    foreach (var item in dicOrdering)
        //    {
        //        //正序排序
        //        if (item.Value)
        //        {
        //            data = data.OrderBy(item.Key);
        //        }
        //        //反序排序
        //        else
        //        {
        //            data = data.OrderByDescending(item.Key);
        //        }
        //    }
        //    if (whereLambda != null)
        //    {
        //        data = data.Where(whereLambda);
        //    }
        //    rows = data.Count();
        //    return data.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        //}
        //#endregion 

        //#region 获取个数、判断对象是否存在
        ///// <summary>
        ///// 获取个数
        ///// </summary>
        ///// <returns></returns>
        //public long Count(Expression<Func<T, bool>> whereLambd = null)
        //{
        //    if(whereLambd!= null)
        //    {
        //        return _dbSet.Where(whereLambd).LongCount();
        //    }
        //    return _dbSet.LongCount();
        //}
        ///// <summary>
        ///// 判断对象是否存在
        ///// </summary>
        ///// <param name="whereLambd"></param>
        ///// <returns></returns>
        //public bool Any(Expression<Func<T, bool>> whereLambd)
        //{
        //    return _dbSet.Where(whereLambd).Any();
        //}
        //#endregion

        //#endregion

        //#region 插入实体
        ///// <summary>
        ///// 插入数据
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <returns>影响条数</returns>
        //public int Insert(T entity)
        //{
        //    _dbSet.Add(entity);
        //    return DbContext.SaveChanges();
        //}
        ///// <summary>
        ///// 插入多条数据
        ///// </summary>
        ///// <param name="entitys"></param>
        ///// <returns>影响条数</returns>
        //public int Insert(List<T> entitys)
        //{
        //    foreach (var entity in entitys)
        //    {
        //        Insert(entity);
        //    }
        //    return DbContext.SaveChanges();
        //}
        ///// <summary>
        ///// 插入数据（事务未提交）
        ///// </summary>
        ///// <param name="entity"></param>
        //public void InsertThen(T entity)
        //{
        //    _dbSet.Add(entity);
        //}
        ///// <summary>
        ///// 插入多条数据（事务未提交）
        ///// </summary>
        ///// <param name="entitys"></param>
        //public void InsertThen(List<T> entitys)
        //{
        //    foreach (var entity in entitys)
        //    {
        //        InsertThen(entity);
        //    }
        //}

        //#endregion

        //#region 更新实体
        ///// <summary>
        ///// 更新单条实体
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <returns>影响条数</returns>
        //public int Update(T entity)
        //{
        //    _dbSet.Attach(entity);
        //    DbContext.Entry(entity).State = EntityState.Modified;
        //    return  DbContext.SaveChanges();
        //}
        ///// <summary>
        ///// 更新多条实体
        ///// </summary>
        ///// <param name="entitys"></param>
        ///// <returns>影响条数</returns>
        //public int Update(List<T> entitys)
        //{
        //    foreach (var entity in entitys)
        //    {
        //        Update(entity);
        //    }
        //    return DbContext.SaveChanges();
        //}
        ///// <summary>
        ///// 更新单条实体（事务未提交）
        ///// </summary>
        ///// <param name="entitys"></param>
        //public void UpdateThen(T entity)
        //{
        //    _dbSet.Attach(entity);
        //    DbContext.Entry(entity).State = EntityState.Modified;
        //}
        ///// <summary>
        ///// 更新多条实体（事务未提交）
        ///// </summary>
        ///// <param name="entitys"></param>
        //public void UpdateThen(List<T> entitys)
        //{
        //    foreach (var entity in entitys)
        //    {
        //        UpdateThen(entity);
        //    }
        //}
        //#endregion

        //#region 删除实体
        ///// <summary>
        ///// 删除单条实体
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <returns>影响条数</returns>
        //public int Delete(T entity)
        //{
        //    _dbSet.Remove(entity);
        //    return DbContext.SaveChanges();
        //}
        ///// <summary>
        ///// 删除多条实体
        ///// </summary>
        ///// <param name="entitys"></param>
        ///// <returns>影响条数</returns>
        //public int Delete(List<T> entitys)
        //{
        //    foreach (var entity in entitys)
        //    {
        //        Delete(entity);
        //    }
        //    return DbContext.SaveChanges();
        //}
        ///// <summary>
        ///// 删除单条实体（事务未提交）
        ///// </summary>
        ///// <param name="entity"></param>
        //public void DeleteThen(T entity)
        //{
        //    _dbSet.Remove(entity);
        //}
        ///// <summary>
        ///// 删除多条实体（事务未提交）
        ///// </summary>
        ///// <param name="entitys"></param>
        //public void DeleteThen(List<T> entitys)
        //{
        //    foreach (var entity in entitys)
        //    {
        //        DeleteThen(entity);
        //    }
        //}
        //#endregion

        #endregion

    }
}
