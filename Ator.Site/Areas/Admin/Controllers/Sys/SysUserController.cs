﻿/*****************************************************************
*Module Name:SysUserController
*Module Date:2018-10-30
*Module Auth:Jomzhang
*Description:用户控制器
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
    public class SysUserController : BaseController
    {
        #region Init
        private string _entityName = "用户";
        private UnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private ISysUserService _sysUserService;
        private IMapper _mapper;
        public SysUserController(UnitOfWork unitOfWork, ILogger<SysUserController> logger, ISysUserService sysUserService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _sysUserService = sysUserService;
            _mapper = mapper;
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
            var model = _unitOfWork.SysUserRepository.Get(id);
            return View(model ?? new SysUser() { Status=1});
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
            var data = await _unitOfWork.SysUserRepository.GetAsync(id);
            var dataDto = _mapper.Map<SysUserSearchDto>(data);
            if (dataDto == null)
                return ErrRes("暂无数据");
            return SuccessRes(dataDto);
        }

        /// <summary>
        /// 查询数据集
        /// </summary>
        /// <param name="sysUserSearchViewModel"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPageData(SysUserSearchViewModel search)
        {
            var predicate = PredicateBuilder.New<SysUser>(true);//查询条件

            #region 添加条件查询
            if (!string.IsNullOrEmpty(search.UserName))
            {
                predicate = predicate.And(i => i.UserName.Contains(search.UserName));
            }
            if (!string.IsNullOrEmpty(search.NikeName))
            {
                predicate = predicate.And(i => i.NikeName.Contains(search.NikeName));
            }
            if (!string.IsNullOrEmpty(search.Mobile))
            {
                predicate = predicate.And(i => i.Mobile.Contains(search.Mobile));
            } 
            #endregion

            //查询数据
            //var searchData = await _unitOfWork.SysUserRepository.GetPageAsync(predicate, search.Ordering, search.Page, search.Limit);
            //多条件排序
            var searchData = await _unitOfWork.SysUserRepository.GetPageAsync(predicate, $"{nameof(SysUser.Mobile)},-{nameof(SysUser.CreateTime)}", search.Page, search.Limit);

            //获得返回集合Dto
            search.ReturnData = searchData.Rows.Select(o => _mapper.Map<SysUserSearchDto>(o)).ToList();
            return SuccessRes(search.ReturnData, searchData.Totals);
        }

        /// <summary>
        /// 保存，修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Save(SysUser model,string columns="")
        {
            var errMsg = GetModelErrMsg();
            if (!string.IsNullOrEmpty(errMsg))
            {
                return ErrRes(errMsg);
            }
            model.Status = model.Status ?? 2;
            if (string.IsNullOrEmpty(model.SysUserId))
            {
                if (_unitOfWork.SysUserRepository.Any(o => o.UserName == model.UserName))
                {
                    return ErrRes($"{_entityName}已存在");
                }
                model.SysUserId = GuidKey;
                model.Password = model.Password.Md532();
                model.CreateTime = DateTime.Now;
                model.CreateUser = Id;
                
                result = await _unitOfWork.SysUserRepository.InsertAsync(model);
                if (result)
                {
                    _logger.LogInformation($"添加{_entityName}{model.UserName}");
                }
            }
            else
            {
                if(model.SysUserId == "e3a31ac69f7946ca9218f865e8a4b875" || model.UserName == "admin")//
                {
                    return ErrRes("管理员账户不允许修改");
                }
                //定义可以修改的列
                var lstColumn = new List<string>()
                {
                    nameof(SysUser.Email), nameof(SysUser.Mobile), nameof(SysUser.NikeName), nameof(SysUser.QQ), nameof(SysUser.TrueName), nameof(SysUser.Status)
                };
                if (!string.IsNullOrEmpty(columns))//固定过滤只修改某字段
                {
                    lstColumn = lstColumn.Where(o => columns.Split(',').Contains(o)).ToList();
                }
                result = await _unitOfWork.SysUserRepository.UpdateAsync(model,true, lstColumn);
                if (result)
                {
                    _logger.LogInformation($"修改{_entityName}{model.UserName}");
                }
            }
            return result?SuccessRes():ErrRes();
        }


        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status">数据状态1-正常，2-不通过</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Checks(string ids,int status = 1)
        {
            var lstUpdateModel = await _unitOfWork.SysUserRepository.GetListAsync(o => ids.TrimEnd(',').Split(',', StringSplitOptions.None).Contains(o.SysUserId));
            bool result = false;
            if (lstUpdateModel.Count > 0)
            {
                for (int i = 0; i < lstUpdateModel.Count; i++)
                {
                    lstUpdateModel[i].Status = status;
                }
                result = await _unitOfWork.SysUserRepository.UpdateAsync(lstUpdateModel);
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
            var lstId = ids.TrimEnd(',').Split(',', StringSplitOptions.None);
            var lstDelModel = await _unitOfWork.SysUserRepository.GetListAsync(o => lstId.Contains(o.SysUserId));
            if(lstDelModel.Any(o => o.UserName == "admin"))
            {
                return ErrRes("管理员账户你就不要删除了吧?");
            }
            bool result = false;
            if(lstDelModel.Count > 0)
            {
                result = await _unitOfWork.SysUserRepository.DeleteAsync(lstDelModel);
                if (result)
                {
                    _logger.LogInformation($"删除{lstDelModel.Count}个{_entityName}，{_entityName}编码：{ids}");
                }
                
            }
            return ResultRes(result);
        }
        #endregion

        [HttpPost]
        public async Task<IActionResult> CreateUserTest(int num)
        {
            List<SysUser> lstUser = new List<SysUser>();
            int count = num > 100000 ? 100000 : num;
            for (int i = 0; i < count; i++)
            {
                string mobile = "189" + (new Random()).Next(10000000, 99999999);
                lstUser.Add(new SysUser
                {
                    CreateTime = DateTime.Now,
                    CreateUser = "test",
                    Mobile = mobile,
                    NikeName = mobile,
                    SysUserId = GuidKey,
                    UserName = mobile,
                    Password = "123456".Md532(),
                    TrueName = mobile
                });
            }
            var s = await _unitOfWork.SysUserRepository.InsertAsync(lstUser);
            return s ? SuccessRes() : ErrRes();
        }
    }
}