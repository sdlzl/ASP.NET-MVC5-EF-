using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.CommonMVC
{
    public class MyPager
    {
        /// <summary>
        /// 每页的大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总数据条数
        /// </summary>
        public int TotalCount { get; set; }


        /// <summary>
        /// 当前页的样式
        /// </summary>
        public string CurrentPageClassName { get; set; }


        /// <summary>
        /// 地址的格式
        /// </summary>
        public string UrlPattern { get; set; }


        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; }




        /// <summary>
        /// 最大页数
        /// </summary>
        public int MaxPageCount { get; set; }


        public MyPager()
        {
            this.MaxPageCount = 10;
            this.PageSize = 5;

        }

        public string GetPageHtml()
        {

            StringBuilder html = new StringBuilder();
            //总页数
            int  PageCount = (int)Math.Ceiling(TotalCount * 1.0 / PageSize);
            int  StartPageIndex = Math.Max(1, PageIndex - MaxPageCount / 2);
            int  EndPageIndex = Math.Min(PageCount,PageIndex+MaxPageCount-1);
            html.AppendLine("<ul>");
            for (int i=StartPageIndex;i<=EndPageIndex;i++)
            {
                if (i == PageIndex)
                {
                    html.Append("<li class='").Append(CurrentPageClassName).Append("'>").Append(i.ToString()).Append("</li>");
                }
                else
                {
                    html.Append("<li><a href='").Append(UrlPattern.Replace("{pn}", i.ToString())).Append("'").Append(">")
                        .Append(i.ToString()).Append("</a></li>");
                }

            }
            html.AppendLine("</ul>");
            return html.ToString();

        }

    }
}
