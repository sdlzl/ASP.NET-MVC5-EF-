using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.IService;

namespace ZSZ.FrontWeb
{
    public class FrontUtils
    {
        public static long? GetUserId(HttpContextBase ctx)
        {
            return (long?)ctx.Session["UserId"];
        }

        public static long GetCityId(HttpContextBase ctx)
        {
            //先判断有无用户登录
            long? userId = GetUserId(ctx);
            if (userId == null)
            {
                //如果为空，判断sessionCityId是否有值
                long? CityId = (long?)ctx.Session["CityId"];
                if (CityId == null)
                {
                    //去数据库的第一个城市
                    var cityService = DependencyResolver.Current.GetService<ICityService>();
                    return cityService.GetAll()[0].Id;
                }
                else
                {
                    return CityId.Value;
                }

            }
            else//用户不等于空
            {
                var cityService = DependencyResolver.Current.GetService<ICityService>();
                var UserService= DependencyResolver.Current.GetService<IUserService>();
                long? cityId = UserService.GetById(userId.Value).CityId;
                if (cityId == null)
                {
                    return cityService.GetAll()[0].Id;
                }
                else
                {
                    return cityId.Value;
                }

            }
          
        }

       
    }
}