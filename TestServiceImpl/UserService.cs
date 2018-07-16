using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestService;

namespace TestServiceImpl
{
    public class UserService : IUserService
    {
        //用到其他的实现类
        public INewsService NesService { get; set; }
        
        public bool CheckLogin(string userName, string password)
        {
            NesService.AddNews("123","456");
            return true;
        }

        public bool CheckUserNameExists(string userName)
        {
            return true;
        }
    }
}
