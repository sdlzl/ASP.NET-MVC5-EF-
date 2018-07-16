using MyIBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBllImpl
{
    public class DogBLL : IDogBll
    {
        public void Bark()
        {
            Console.WriteLine("汪汪");
        }
    }
}
