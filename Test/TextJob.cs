using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;

namespace Test
{
    //实现IJob接口
    class TextJob : Quartz.IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("项目执行到了" + DateTime.Now);
        }
    }
}
