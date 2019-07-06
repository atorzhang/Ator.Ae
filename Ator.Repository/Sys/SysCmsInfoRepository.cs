using Ator.Entity;
using Ator.Entity.Sys;
using Ator.Model.ViewModel.Sys;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ator.Repository.Sys
{
    public class SysCmsInfoRepository : RepositoryBase<SysCmsInfo>
    {
        public SysCmsInfoRepository(AeDbContext context) : base(context)
        {
        }
        public List<SysCmsInfoSearchDto> GetInfoPage(string SysCmsColumnId,string InfoTitle, string InfoType, string Status,  string ordering, int Page,int Limit)
        {
            var data = GetInfoQuery(SysCmsColumnId, InfoTitle, InfoType, Status,   ordering, Page, Limit).ToList();
            return data;
        }
        public async Task<List<SysCmsInfoSearchDto>> GetInfoPageAsync(string SysCmsColumnId, string InfoTitle, string InfoType, string Status,  string ordering, int Page, int Limit)
        {
            return await GetInfoQuery(SysCmsColumnId, InfoTitle, InfoType, Status, ordering, Page, Limit).ToListAsync();
        }

        private IQueryable<SysCmsInfoSearchDto> GetInfoQuery(string SysCmsColumnId, string InfoTitle, string InfoType, string Status,  string ordering, int Page, int Limit)
        {
            var predicate = PredicateBuilder.New<SysCmsInfo>(true);//查询条件

            #region 添加条件查询
            if (!string.IsNullOrEmpty(InfoTitle))
            {
                predicate = predicate.And(i => i.InfoTitle.Contains(InfoTitle));
            }
            if (!string.IsNullOrEmpty(SysCmsColumnId))
            {
                predicate = predicate.And(i => i.SysCmsColumnId.Equals(SysCmsColumnId));
            }
            if (!string.IsNullOrEmpty(InfoType))
            {
                predicate = predicate.And(i => i.InfoType.Equals(InfoType));
            }
            if (!string.IsNullOrEmpty(Status))
            {
                predicate = predicate.And(i => i.Status.Equals(int.Parse(Status)));
            }
            #endregion
            var queryData = Load(predicate)
                .Where(predicate)
                .Select(o => new SysCmsInfoSearchDto
                {
                    CreateTime = o.CreateTime,
                    CreateUser = o.CreateUser,
                    InfoTop = o.InfoTop,
                    InfoAuthor = o.InfoAuthor,
                    InfoClicks = o.InfoClicks,
                    InfoComments = o.InfoComments,
                    InfoGoods = o.InfoGoods,
                    InfoLable = o.InfoLable,
                    InfoPublishTime = o.InfoPublishTime,
                    InfoSource = o.InfoSource,
                    InfoTitle = o.InfoTitle,
                    InfoType = o.InfoType,
                    Remark = o.Remark,
                    Sort = o.Sort,
                    Status = o.Status,
                    SysCmsInfoId = o.SysCmsInfoId,
                    InfoAbstract = o.InfoAbstract,
                    SysCmsColumnId = o.SysCmsColumnId,
                    InfoImage = o.InfoImage
                    
                })
                .OrderByBatch(ordering)
                .Skip((Page - 1) * Limit)
                .Take(Limit);
            return queryData;
        }
    }
}
