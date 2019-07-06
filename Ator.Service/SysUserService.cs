﻿using Ator.Entity.Sys;
using Ator.IService;
using Ator.Model;
using Ator.Model.Constant;
using Ator.Repository;
using Ator.Utility.Ext;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Service
{
    public class SysUserService: ISysUserService
    {
        UnitOfWork _unitOfWork;
        public SysUserService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public string DoChangePwd(string UserName, string oldPwd, string newPwd)
        {
            var userModel = _unitOfWork.SysUserRepository.Get(o => o.UserName == UserName);
            if(userModel == null)
            {
                return "登陆信息丢失，请重新登陆";
            }
            if (string.IsNullOrEmpty(oldPwd) || string.IsNullOrEmpty(newPwd))
            {
                return "请填写原密码和新密码";
            }
            if (oldPwd.Md532() != userModel.Password)
            {
                return "原密码错误";
            }
            if(string.IsNullOrEmpty(newPwd) || newPwd.Length < 6)
            {
                return "新密码长度必须大于等于6位";
            }
            userModel.Password = newPwd.Md532();
            var isUpdate = _unitOfWork.SysUserRepository.Update(userModel);
            return isUpdate ? "" : "修改失败";
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns></returns>
        public string DoLogin(LoginViewModel loginViewModel)
        {
            if (string.IsNullOrWhiteSpace(loginViewModel.UserName))
            {
                return "用户名不能为空";
            }
            var userModel = _unitOfWork.SysUserRepository.Get(o => o.UserName == loginViewModel.UserName);
            if(userModel == null)
            {
                return "用户名不存在";
            }
            if(loginViewModel.Password.Md532() != userModel.Password)
            {
                _unitOfWork.SysOperateRecordRepository.InsertOperate(nameof(SysUserService), nameof(DoLogin), $"登录密码错误，用户名:{loginViewModel.UserName},密码{loginViewModel.Password},Ip:{loginViewModel.Ip}", "Sys_User", (short)EnumSysOperateRecordType.Login, (short)EnumSysOperateRecordResult.Fail, UserName: loginViewModel.UserName);//登录失败记录
                return "密码错误";
            }
            _unitOfWork.SysOperateRecordRepository.InsertOperate(nameof(SysUserService), nameof(DoLogin), $"{loginViewModel.Ip}", "Sys_User", (short)EnumSysOperateRecordType.Login, (short)EnumSysOperateRecordResult.Success,UserName:loginViewModel.UserName);//登录成功记录
            return "";
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns></returns>
        public string DoRegiste(RegisteViewModel registeViewModel)
        {
            if (string.IsNullOrWhiteSpace(registeViewModel.UserName))
            {
                return "用户名不能为空";
            }
            if (string.IsNullOrWhiteSpace(registeViewModel.Password))
            {
                return "密码不能为空";
            }
            if(_unitOfWork.SysUserRepository.Any(o=>o.UserName == registeViewModel.UserName))
            {
                return "该用户名已存在";
            }
            var userModel = new SysUser
            {
                UserName = registeViewModel.UserName,
                Password = registeViewModel.Password.Md532(),
            };
            var result = _unitOfWork.SysUserRepository.Insert(userModel);
            return result?"":"注册失败";
        }
    }
}
