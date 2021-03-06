﻿/*****************************************************************
*Module Name:SysLinkTypeController
*Module Date:2018-11-21
*Module Auth:Jomzhang
*Description:链接类型控制器
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
    public class SysLinkTypeController : BaseController
    {
        #region Init
        private string _entityName = "链接类型";
        private UnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private IMapper _mapper;
        private ISysLinkTypeService _ISysLinkTypeService;
        public SysLinkTypeController(UnitOfWork unitOfWork, ILogger<SysLinkTypeController> logger, IMapper mapper, ISysLinkTypeService sysLinkTypeService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _ISysLinkTypeService = sysLinkTypeService;
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
            var model = _unitOfWork.SysLinkTypeRepository.Get(id);
            ViewBag.SysLinkTypeParentSelect = _ISysLinkTypeService.GetLinkTypeList();
            return View(model ?? new SysLinkType() { Status = 1 });
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
            var data = await _unitOfWork.SysLinkTypeRepository.GetAsync(id);
            return SuccessRes(data);
        }

        /// <summary>
        /// 查询数据集
        /// </summary>
        /// <param name="sysUserSearchViewModel"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPageData(SysLinkTypeSearchViewModel search)
        {
            var predicate = PredicateBuilder.New<SysLinkType>(true);//查询条件

            #region 添加条件查询
            if (!string.IsNullOrEmpty(search.SysLinkTypeName))
            {
                predicate = predicate.And(i => i.SysLinkTypeName.Contains(search.SysLinkTypeName));
            }
            if (!string.IsNullOrEmpty(search.Group))
            {
                predicate = predicate.And(i => i.Group.Contains(search.Group));
            }
            if (!string.IsNullOrEmpty(search.SysLinkTypeId))
            {
                predicate = predicate.And(i => i.SysLinkTypeId.Equals(search.SysLinkTypeId));
            }
            #endregion

            //查询数据
            var searchData = await _unitOfWork.SysLinkTypeRepository.GetPageAsync(predicate, search.Ordering, search.Page, search.Limit);

            //获得返回集合Dto
            search.ReturnData = searchData.Rows;
            return SuccessRes(search.ReturnData, searchData.Totals);
        }

        /// <summary>
        /// 保存，修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Save(SysLinkType model, string columns = "")
        {
            var errMsg = GetModelErrMsg();
            if (!string.IsNullOrEmpty(errMsg))
            {
                return ErrRes(errMsg);
            }
            model.Status = model.Status ?? 2;
            if (string.IsNullOrEmpty(model.SysLinkTypeId))
            {
                model.SysLinkTypeId = GuidKey;
                model.CreateTime = DateTime.Now;
                model.CreateUser = Id;

                result = await _unitOfWork.SysLinkTypeRepository.InsertAsync(model);
                if (result)
                {
                    _logger.LogInformation($"添加{_entityName}{model.SysLinkTypeName}");
                }
            }
            else
            {
                //定义可以修改的列
                var lstColumn = new List<string>()
                {
                    nameof(SysLinkType.Remark), nameof(SysLinkType.Sort), nameof(SysLinkType.Status), nameof(SysLinkType.SysLinkTypeName), nameof(SysLinkType.Group), nameof(SysLinkType.SysLinkTypeLogo)
                };
                if (!string.IsNullOrEmpty(columns))//固定过滤只修改某字段
                {
                    if (lstColumn.Count == 0)
                    {
                        lstColumn = columns.Split(',').ToList();
                    }
                    else
                    {
                        lstColumn = lstColumn.Where(o => columns.Split(',').Contains(o)).ToList();
                    }
                }
                result = await _unitOfWork.SysLinkTypeRepository.UpdateAsync(model, true, lstColumn);
                if (result)
                {
                    _logger.LogInformation($"修改{_entityName}{model.SysLinkTypeName}");
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
            var lstUpdateModel = await _unitOfWork.SysLinkTypeRepository.GetListAsync(o => ids.TrimEnd(',').Split(',', StringSplitOptions.None).Contains(o.SysLinkTypeId));
            bool result = false;
            if (lstUpdateModel.Count > 0)
            {
                for (int i = 0; i < lstUpdateModel.Count; i++)
                {
                    lstUpdateModel[i].Status = status;
                }
                result = await _unitOfWork.SysLinkTypeRepository.UpdateAsync(lstUpdateModel);
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
            var lstDelModel = await _unitOfWork.SysLinkTypeRepository.GetListAsync(o => ids.TrimEnd(',').Split(',', StringSplitOptions.None).Contains(o.SysLinkTypeId));
            bool result = false;
            if (lstDelModel.Count > 0)
            {
                result = await _unitOfWork.SysLinkTypeRepository.DeleteAsync(lstDelModel);
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