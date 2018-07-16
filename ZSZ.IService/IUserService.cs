using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;

namespace ZSZ.IService
{
    public interface IUserService:IServiceSupport
    {
        long AddNew(String phoneNum, String password);
        UserDTO GetById(long id);
        UserDTO GetByPhoneNum(String phoneNum);

        ////检查用户名密码是否正确（很好体现了分层的思想）
        bool CheckLogin(String phoneNum, String password);
        void UpdatePwd(long userId, String newPassword);

        //设置用户userId的城市id
        void SetUserCityId(long userId, long cityId);

        //登陆错误次数加一
        void IncLoginErrorTimes(long userId);

        //重置错误次数
        void ResetLoginErrorTimes(long userId);

        /// <summary>
        /// 锁定帐号
        /// </summary>
        /// <param name="userId"></param>
        /// <returns> true 为锁定 false为不锁定</returns>

        bool IsLockUser(long userId);
        

        
    }

}
