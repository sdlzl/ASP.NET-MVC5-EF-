using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Common;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
          string md5=  CommonHelper.CalcMD5("1234123456");
            Console.WriteLine(md5);
            File.WriteAllText(@"C:\Users\Public\Desktop\1.txt", md5);
            Console.ReadKey();
        
        }
    }
}
