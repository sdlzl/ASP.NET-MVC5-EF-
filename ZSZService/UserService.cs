﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Common;
using ZSZ.DTO;
using ZSZ.IService;
using ZSZ.Service.Entities;
using ZSZService;

namespace ZSZ.Service
{
    public class UserService : IUserService
    {
        public long AddNew(string phoneNum, string password)
        {
            using (MyDbContent ctx = new MyDbContent())
            {
                //检查手机号不能重复

                BaseService<UserEntity> bs = new BaseService<UserEntity>(ctx);
                bool exists = bs.GetAll().Any(u=>u.PhoneNum==phoneNum);
                if(exists)
                {
                    throw new ArgumentException("手机号已经存在");
                }
                UserEntity user = new UserEntity();
                user.PhoneNum = phoneNum;
                string salt = CommonHelper.CreateVerifyCode(5);
                string pwdHash = CommonHelper.CalcMD5(salt + password);
                user.PasswordHash = pwdHash;
                user.PasswordSalt = salt;
                ctx.Users.Add(user);
                ctx.SaveChanges();
                return user.Id;
            }
        }

        public bool CheckLogin(string phoneNum, string password)
        {
            using (MyDbContent ctx = new MyDbContent())
            {
                BaseService<UserEntity> bs = new BaseService<UserEntity>(ctx);
                var user = bs.GetAll().SingleOrDefault(u=>u.PhoneNum==phoneNum);
                if(user==null)
                {
                    return false;
                }
                else
                {
                    string dbPwdHash = user.PasswordHash;
                    string salt = user.PasswordSalt;
                    string userPwdHash = CommonHelper.CalcMD5(salt + password);
                    return dbPwdHash == userPwdHash;
                }
            }
        }

        private UserDTO ToDTO(UserEntity user)
        {
            UserDTO dto = new UserDTO();
            dto.CityId = user.CityId;
            dto.CreateDateTime = user.CreateDateTime;
            dto.Id = user.Id;
            dto.LastLoginErrorDateTime = user.LastLoginErrorDateTime;
            dto.LoginErrorTimes = user.LoginErrorTimes;
            dto.PhoneNum = user.PhoneNum;
            return dto;
        }

        public UserDTO GetById(long id)
        {
            using (MyDbContent ctx = new MyDbContent())
            {
                BaseService<UserEntity> bs = new BaseService<UserEntity>(ctx);
                var user = bs.GetById(id);
                return user == null ? null : ToDTO(user);
            }
        }

        public UserDTO GetByPhoneNum(string phoneNum)
        {
            using (MyDbContent ctx = new MyDbContent())
            {
                BaseService<UserEntity> bs = new BaseService<UserEntity>(ctx);
                var user = bs.GetAll().SingleOrDefault(u => u.PhoneNum == phoneNum);
                return user == null ? null : ToDTO(user);
            }
        }

        public void SetUserCityId(long userId, long cityId)
        {
            using (MyDbContent ctx = new MyDbContent())
            {
                BaseService<UserEntity> bs = new BaseService<UserEntity>(ctx);
                var user = bs.GetById(userId);
                if(user==null)
                {
                    throw new ArgumentException("用户id不存在"+userId);
                }
                user.CityId = cityId;
                ctx.SaveChanges();
            }
        }

        public void UpdatePwd(long userId, string newPassword)
        {
            using (MyDbContent ctx = new MyDbContent())
            {
                //检查手机号不能重复
                BaseService<UserEntity> bs = new BaseService<UserEntity>(ctx);
                var user = bs.GetById(userId);
                if (user==null)
                {
                    throw new ArgumentException("用户不存在 "+ userId);
                }
                string salt = user.PasswordSalt;// CommonHelper.CreateVerifyCode(5);
                string pwdHash = CommonHelper.CalcMD5(salt + newPassword);
                user.PasswordHash = pwdHash;
                user.PasswordSalt = salt;
                ctx.SaveChanges();
            }
        }

        public void IncLoginErrorTimes(long userId)
        {
            using (MyDbContent ctx = new MyDbContent())
            {
                //检查手机号不能重复
                BaseService<UserEntity> bs = new BaseService<UserEntity>(ctx);
                var user = bs.GetById(userId);
                if (user == null)
                {
                    throw new ArgumentException("用户不存在 " + userId);
                }

                user.LoginErrorTimes++;
                user.LastLoginErrorDateTime = DateTime.Now;
                ctx.SaveChanges();
            }

        }

        public void ResetLoginErrorTimes(long userId)
        {
            using (MyDbContent ctx = new MyDbContent())
            {
                //检查手机号不能重复
                BaseService<UserEntity> bs = new BaseService<UserEntity>(ctx);
                var user = bs.GetById(userId);
                if (user == null)
                {
                    throw new ArgumentException("用户不存在 " + userId);
                }

                user.LoginErrorTimes = 0;
                user.LastLoginErrorDateTime = null;
                ctx.SaveChanges();
            }
        }

        //判断是否锁定帐号
        public bool IsLockUser(long userId)
        {
            using (MyDbContent ctx = new MyDbContent())
            {
                //检查手机号不能重复
                BaseService<UserEntity> bs = new BaseService<UserEntity>(ctx);
                var user = bs.GetById(userId);
                if (user == null)
                {
                    throw new ArgumentException("用户不存在 " + userId);
                }
                //错误大于等于三次就要30分钟后再登录
                if (user.LoginErrorTimes >= 3 && user.LastLoginErrorDateTime > DateTime.Now.AddMinutes(-30))
                {
                    return true;//锁定
                }

                return false;

               
            }
        }
    }
}
