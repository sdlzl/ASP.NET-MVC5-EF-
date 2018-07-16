using CaptchaGen;
using CodeCarvings.Piczard;
using CodeCarvings.Piczard.Filters.Watermarks;
using log4net;
using MyIBLL;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Common;
using MyBllImpl;
using Autofac;
using System.Reflection;
using ZSZService;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            /*using (MailMessage mailmessage=new MailMessage())
            {
                using (SmtpClient stmpClient=new SmtpClient("smtp.163.com"))//用什么邮箱就要用对应邮箱的服务器
                {
                    mailmessage.To.Add("sobed@qq.com");
                    mailmessage.Body = "请集合哈哈,罗智炼很帅";
                    mailmessage.From = new MailAddress("LZL547169553@163.com");
                    mailmessage.Subject = "罗智炼的粉丝";
                    stmpClient.Credentials = new System.Net.NetworkCredential("LZL547169553@163.com", "123456lzl");
                    stmpClient.Send(mailmessage);
                   }
            }*/

            //nuget找缩略图  Code
            /* ImageProcessingJob job = new ImageProcessingJob();
             //设置要压缩的图片比例
             job.Filters.Add(new FixedResizeConstraint(200,200));
             //图片路径，以及缩略图要保存的路径
             job.SaveProcessedImageToFileSystem(@"E:\123.jpg", @"E:\ll.jpg");
             Console.WriteLine("缩略成功");
             Console.ReadKey();*/

            //生成水印
            /*ImageWatermark img = new ImageWatermark(@"E:\ll.jpg");
            img.ContentAlignment = System.Drawing.ContentAlignment.BottomRight;
            img.Alpha = 50;
            ImageProcessingJob job = new ImageProcessingJob();
             job.Filters.Add(new FixedResizeConstraint(600,600));
            job.Filters.Add(img);
            job.SaveProcessedImageToFileSystem(@"E:\123.jpg", @"E:\shuiying.jpg");
            Console.Write("OK");
            Console.ReadKey();*/

            //验证码生成组件
            /*  using (MemoryStream ms = ImageFactory.GenerateImage("1ha3", 80, 100,30,10))
              { 
                  using (FileStream fs = File.OpenWrite(@"E:\hah.jpg"))
                  {
                      ms.CopyTo(fs);
                  }
              }
              Console.WriteLine("OK");
              Console.ReadKey();*/
            /*
            log4net.Config.XmlConfigurator.Configure();
            ILog log= LogManager.GetLogger(typeof(Program));
            log.Debug("飞行高度400米");
            log.Warn("油压不足");
            log.Error("引擎失灵");
            log.DebugFormat("当前数据有问题,{0}", "性能好一点点");
            */

            /* IScheduler sched = new StdSchedulerFactory().GetScheduler();
            JobDetailImpl jdBossReport = new JobDetailImpl("jdTest", typeof(TextJob));
           IMutableTrigger triggerBossReport = CronScheduleBuilder.DailyAtHourAndMinute(11,
             03).Build();//每天 23:45 执行一次
             triggerBossReport.Key = new TriggerKey("triggerTest");*/
            //先设置好触发器的触发事件
            /*  var builder = CalendarIntervalScheduleBuilder.Create();
              builder.WithInterval(3, IntervalUnit.Second);*/
            //创建一个触发器
            //IMutableTrigger triggerBossReport = builder.Build();
            //给触发器创建名称
            /* triggerBossReport.Key = new TriggerKey("triggerTest");
             sched.ScheduleJob(jdBossReport, triggerBossReport);
             sched.Start();*/

            //基于接口的编程
            /* IUserBLL bll = new UserBll();
             bll.AddNew("aa", "123");*/

            //Autofac 容器 不用自己new
            // ContainerBuilder buider = new ContainerBuilder();
            //有多少个BLL就需要注册多个IBLL，很麻烦
            /* buider.RegisterType<UserBll>().As<IUserBLL>();*/


            //一次加载所有的BLL
            /*  Assembly asm = Assembly.Load("MyBllImpl");
              buider.RegisterAssemblyTypes(asm).AsImplementedInterfaces().SingleInstance();
              IContainer container = buider.Build();
              IUserBLL bll = container.Resolve<IUserBLL>();
              IDogBll dogbll = container.Resolve<IDogBll>();
              bll.AddNew("aaa", "123");
              dogbll.Bark();*/
            using (MyDbContent ctx=new MyDbContent())
            {
                ctx.Database.Delete();
                ctx.Database.Create();
            }




            Console.WriteLine("OK");
            Console.ReadKey();

        }
    }
}
