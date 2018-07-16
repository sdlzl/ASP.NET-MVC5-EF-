using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestService
{
    public interface IUserService
    {
        bool CheckLogin(string userName,string password);
        bool CheckUserNameExists(string userName);
    }
}
