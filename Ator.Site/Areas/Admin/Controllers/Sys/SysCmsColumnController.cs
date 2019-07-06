/*****************************************************************
*Module Name:SysCmsColumnController
*Module Date:2018-11-21
*Module Auth:Jomzhang
*Description:页面控制器
*Others:
*evision History:
*DateRel VerNotes
*Blog:https://www.cnblogs.com/jomzhang/
*****************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ator.Entity.Sys;
using Ator.IService;
using Ator.Model.ViewModel.Sys;
using Ator.Repository;
using Ator.Utility.Ext;
using AutoMapper;
using LinqKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Ator.Site.Areas.Admin.Controllers.Sys
{
    [Area("Admin")]
    public class SysCmsColumnController : BaseController
    {
        #region Init
        private string _entityName = "页面";
        private UnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private IMapper _mapper;
        private ISysCmsColumnService _SysCmsColumnService;
        public SysCmsColumnController(UnitOfWork unitOfWork, ILogger<SysCmsColumnController> logger, IMapper mapper, ISysCmsColumnService SysCmsColumnService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _SysCmsColumnService = SysCmsColumnService;
        }
        #endregion

        #region Page
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Form(string id)
        {
            ViewBag.id = id;
            ViewBag.isCreate = string.IsNullOrEmpty(id);
            var model = _unitOfWork.SysCmsColumnRepository.Get(id);
            ViewBag.SysCmsColumnParentSelect = _SysCmsColumnService.GetColumnList();
            return View(model ?? new SysCmsColumn() { Status = 1 });
        }

        [HttpGet]
        public IActionResult Detail(string id)
        {
            return View();
        }
        #endregion

        #region Operate
        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetData(string id)
        {
            var data = await _unitOfWork.SysCmsColumnRepository.GetAsync(id);
            return SuccessRes(data);
        }

        /// <summary>
        /// 查询数据集
        /// </summary>
        /// <param name="sysUserSearchViewModel"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPageData(SysCmsColumnSearchViewModel search)
        {
            var predicate = PredicateBuilder.New<SysCmsColumn>(true);//查询条件

            #region 添加条件查询
            if (!string.IsNullOrEmpty(search.ColumnName))
            {
                predicate = predicate.And(i => i.ColumnName.Contains(search.ColumnName));
            }

            #endregion

            //查询数据
            var searchData = await _unitOfWork.SysCmsColumnRepository.GetPageAsync(predicate, search.Ordering, search.Page, search.Limit);

            //获得返回集合Dto
            search.ReturnData = searchData.Rows;
            foreach (var item in search.ReturnData)
            {
                item.ColumnParent = (await _unitOfWork.SysCmsColumnRepository.LoadAsync(o => o.SysCmsColumnId == item.ColumnParent)).FirstOrDefault()?.ColumnName;
            }
            return SuccessRes(search.ReturnData, searchData.Totals);
        }

        /// <summary>
        /// 保存，修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Save(SysCmsColumn model, string columns = "")
        {
            var errMsg = GetModelErrMsg();
            if (!string.IsNullOrEmpty(errMsg))
            {
                return ErrRes(errMsg);
            }
            model.Status = model.Status ?? 2;
            if (string.IsNullOrEmpty(model.SysCmsColumnId))
            {
                model.SysCmsColumnId = GuidKey;
                model.CreateTime = DateTime.Now;
                model.CreateUser = Id;

                result = await _unitOfWork.SysCmsColumnRepository.InsertAsync(model);
                if (result)
                {
                    _logger.LogInformation($"添加{_entityName}{model.ColumnName}");
                }
            }
            else
            {
                //定义可以修改的列
                var lstColumn = new List<string>()
                {
                    nameof(SysCmsColumn.Remark), nameof(SysCmsColumn.Sort), nameof(SysCmsColumn.Status), nameof(SysCmsColumn.ColumnName), nameof(SysCmsColumn.ColumnLogo), nameof(SysCmsColumn.ColumnDescript),nameof(SysCmsColumn.ColumnParent)
                };
                if (!string.IsNullOrEmpty(columns))//固定过滤只修改某字段
                {
                    if(lstColumn.Count == 0)
                    {
                        lstColumn = columns.Split(',').ToList();
                    }
                    else
                    {
                        lstColumn = lstColumn.Where(o => columns.Split(',').Contains(o)).ToList();
                    }
                }
                result = await _unitOfWork.SysCmsColumnRepository.UpdateAsync(model, true, lstColumn);
                if (result)
                {
                    _logger.LogInformation($"修改{_entityName}{model.ColumnName}");
                }
            }
            return result ? SuccessRes() : ErrRes();
        }


        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status">数据状态1-正常，2-不通过</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Checks(string ids, int status = 1)
        {
            var lstUpdateModel = await _unitOfWork.SysCmsColumnRepository.GetListAsync(o => ids.TrimEnd(',').Split(',', StringSplitOptions.None).Contains(o.SysCmsColumnId));
            bool result = false;
            if (lstUpdateModel.Count > 0)
            {
                for (int i = 0; i < lstUpdateModel.Count; i++)
                {
                    lstUpdateModel[i].Status = status;
                }
                result = await _unitOfWork.SysCmsColumnRepository.UpdateAsync(lstUpdateModel);
            }
            return ResultRes(result);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Deletes(string ids)
        {
            var lstDelModel = await _unitOfWork.SysCmsColumnRepository.GetListAsync(o => ids.TrimEnd(',').Split(',', StringSplitOptions.None).Contains(o.SysCmsColumnId));
            bool result = false;
            if (lstDelModel.Count > 0)
            {
                result = await _unitOfWork.SysCmsColumnRepository.DeleteAsync(lstDelModel);
                if (result)
                {
                    _logger.LogInformation($"删除{lstDelModel.Count}个{_entityName}，{_entityName}编码：{ids}");
                }

            }
            return ResultRes(result);
        }
        #endregion
    }
}