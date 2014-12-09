using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using LComplete.Framework.Data;
using LComplete.Framework.Web.Common;

namespace LComplete.Framework.Web.Mvc
{
    /// <summary>
    /// 分页视图模型
    /// </summary>
    public class PageViewModel
    {
        public PageUrl FirstPageUrl { get; set; }

        public PageUrl PrevPageUrl { get; set; }

        public PageUrl NextPageUrl { get; set; }

        public PageUrl LastPageUrl { get; set; }

        public int PageCount { get; set; }

        public int RecordCount { get; set; }

        public int PageSize { get; set; }

        public int PageLinkCount { get; set; }

        public PageUrl CurrentPageUrl { get; set; }

        public IList<PageUrl> RenderPageUrls { get; set; }
    }

    /// <summary>
    /// 分页链接
    /// </summary>
    public class PageUrl
    {
        public int PageIndex { get; set; }

        public string Url { get; set; }
    }

    /// <summary>
    /// 提供一组用于分页的扩展方法。
    /// </summary>
    public static class PagerExtensions
    {
        internal const string PAGE_INDEX_NAME = "page";

        public static MvcHtmlString Pager(this HtmlHelper<PagingModel> htmlHelper, PagingModel pagedEntity)
        {
            return GeneratePagerHtml(htmlHelper, htmlHelper.ViewContext.RequestContext, pagedEntity);
        }

        public static MvcHtmlString Pager(this HtmlHelper htmlHelper, PagingModel pagedEntity)
        {
            return GeneratePagerHtml(htmlHelper, htmlHelper.ViewContext.RequestContext, pagedEntity);
        }

        private static string GeneratePagingLink(bool hasLink, string linkUrl, string linkText)
        {
            if (hasLink)
                return string.Format("<a href=\"{0}\">{1}</a>", linkUrl, linkText);
            else
                return string.Format("<span class=\"nopaging\">{0}</span>", linkText);
        }

        private static string GeneratePagingLinkUrl(RequestContext context, int page)
        {
            return RequestUrlUtils.CombinationRequestUrl(context, PAGE_INDEX_NAME, page.ToString());
        }

        private static MvcHtmlString GeneratePagerHtml(HtmlHelper htmlHelper, RequestContext context, PagingModel pagedEntity)
        {
            if (pagedEntity == null)
                throw new ArgumentNullException("pagedEntity");

            StringBuilder html = new StringBuilder();

            int pageNumber = pagedEntity.PageIndex;
            int totalItemCount = pagedEntity.RecordCount;
            int totalpage = (int)Math.Ceiling(totalItemCount / (double)pagedEntity.PageSize);
            int currentPageIndex = pagedEntity.PageIndex;
            bool hasPreviousPage = currentPageIndex > 1;
            bool hasNextPage = currentPageIndex < totalpage;

            // 呈现分页信息
            html.AppendLine(GeneratePagingLink(currentPageIndex > 1, GeneratePagingLinkUrl(context, 1), "第一页"));
            html.AppendLine(GeneratePagingLink(hasPreviousPage, GeneratePagingLinkUrl(context, pageNumber - 1), "前一页"));
            html.AppendLine(string.Format("<span class=\"page-num\">总计 {0} 记录 当前第 {1} 页共 {2} 页</span>",
                totalItemCount, pageNumber, totalpage));
            html.AppendLine(GeneratePagingLink(hasNextPage, GeneratePagingLinkUrl(context, pageNumber + 1), "下一页"));
            html.AppendLine(GeneratePagingLink(currentPageIndex < (totalpage - 1), GeneratePagingLinkUrl(context, totalpage), "最后一页"));

            html.Append("<form method=\"get\">");
            var queryString = context.HttpContext.Request.QueryString;
            foreach (var name in queryString.AllKeys)
            {
                if (!string.Equals(name, "page", StringComparison.CurrentCultureIgnoreCase))
                    html.Append(htmlHelper.Hidden(name, queryString[name]).ToString());
            }
            html.Append(" 跳至第 ");
            html.Append(htmlHelper.TextBox(PAGE_INDEX_NAME, pageNumber, new { @class = "gotoBox", style = "width:30px" }).ToString());
            html.Append(" 页 <input type=\"submit\" value=\"Go\" /></form>");
            return MvcHtmlString.Create(html.ToString());
        }

        /// <summary>
        /// 获取分页视图模型
        /// </summary>
        /// <param name="viewContext"></param>
        /// <param name="pagedEntity"></param>
        /// <param name="pageLinkCount"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static PageViewModel GetPageViewModel(ViewContext viewContext, PagingModel pagedEntity, int pageLinkCount)
        {
            if (pagedEntity == null)
                throw new ArgumentNullException("pagedEntity");

            var pageCount = Math.Max((pagedEntity.RecordCount + pagedEntity.PageSize - 1) / pagedEntity.PageSize, 1); //总页数
            PageViewModel pageView = new PageViewModel()
            {
                PageCount = pageCount,
                PageSize = pagedEntity.PageSize,
                PageLinkCount = pageLinkCount,
                RecordCount = pagedEntity.RecordCount,
                CurrentPageUrl = GetPageUrl(viewContext, pagedEntity.PageIndex),
                FirstPageUrl = GetPageUrl(viewContext, 1),
                LastPageUrl = GetPageUrl(viewContext, pageCount),
                PrevPageUrl = GetPageUrl(viewContext, pagedEntity.PageIndex - 1),
                NextPageUrl = pageCount > pagedEntity.PageIndex ? GetPageUrl(viewContext, pagedEntity.PageIndex + 1) : null,
                RenderPageUrls = new List<PageUrl>()
            };

            int currentPage = pagedEntity.PageIndex;
            int lowOrderIndex = currentPage - pageLinkCount / 2;
            int upOrderIndex = lowOrderIndex + pageLinkCount - 1;
            int lowBoundPage = Math.Max(1, lowOrderIndex);
            int upBoundPage = Math.Min(pageCount, upOrderIndex);
            if (lowOrderIndex < 1)
            {
                upBoundPage = Math.Min(pageCount, pageLinkCount);
            }
            if (upOrderIndex > pageCount)
            {
                lowBoundPage = Math.Max(1, pageCount - pageLinkCount + 1);
            }

            for (int i = lowBoundPage; i <= upBoundPage; i++)
            {
                pageView.RenderPageUrls.Add(GetPageUrl(viewContext, i));
            }

            return pageView;
        }

        /// <summary>
        /// 获取分页url
        /// </summary>
        /// <param name="viewContext"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        private static PageUrl GetPageUrl(ViewContext viewContext, int pageIndex)
        {
            if (pageIndex < 1)
                return null;

            var queryString = viewContext.HttpContext.Request.QueryString;
            var dictRouteValues = new RouteValueDictionary(viewContext.RouteData.Values);
            foreach (string key in queryString.Keys)
                if (queryString[key] != null && !string.IsNullOrEmpty(key))
                    dictRouteValues[key] = queryString[key];
            dictRouteValues[PAGE_INDEX_NAME] = pageIndex;

            return new PageUrl()
            {
                PageIndex = pageIndex,
                Url =
                    UrlHelper.GenerateUrl(null, null, null, dictRouteValues, RouteTable.Routes,
                        viewContext.RequestContext, true)
            };
        }


        /// <summary>
        /// 生成分页链接UI
        /// </summary>
        /// <param name="html"></param>
        /// <param name="pagedEntity"></param>
        /// <param name="alwaysShow">不管总页数，每次都显示</param>
        /// <param name="pageLinkCount">显示链接数量</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static MvcHtmlString NumericPager(this HtmlHelper html, PagingModel pagedEntity, bool alwaysShow = true, int pageLinkCount = 10)
        {
            if (pagedEntity == null)
                throw new ArgumentNullException("pagedEntity");

            PageViewModel pageView = GetPageViewModel(html.ViewContext, pagedEntity, pageLinkCount);

            if (pageView.PageCount > 1 || alwaysShow)
            {
                const string pagerPartialView = "_Pager";
                if (ViewEngines.Engines.FindPartialView(html.ViewContext, pagerPartialView).View != null)
                {
                    return html.Partial("_Pager", pageView);
                }

                return GenerateDefaultPagerUI(html, pageView, alwaysShow);
            }

            return new MvcHtmlString("");
        }

        private static MvcHtmlString GenerateDefaultPagerUI(HtmlHelper html, PageViewModel pageView, bool alwaysShow)
        {
            var output = new StringBuilder();

            const string emptyLinkFormat = "<a href=\"javascript:;\">{0}</a>";
            if (pageView.PageCount > 1 || alwaysShow)
            {
                int currentPage = pageView.CurrentPageUrl.PageIndex;
                output.Append("<div class=\"pagination pagination-right\"><ul>");

                if (currentPage != 1)
                {
                    //处理首页连接
                    AppendLinkItem(output, string.Format("<a href=\"{0}\">首页</a>", pageView.FirstPageUrl.Url));
                }
                if (currentPage > 1)
                {
                    //处理上一页的连接
                    AppendLinkItem(output, string.Format("<a href=\"{0}\">上一页</a>", pageView.PrevPageUrl.Url));
                }
                else
                {
                    AppendLinkItem(output, string.Format(emptyLinkFormat, "上一页"), "disabled");
                }

                for (int i = 0; i < pageView.RenderPageUrls.Count; i++)
                {
                    if (pageView.RenderPageUrls[i].PageIndex == currentPage)
                    {
                        AppendLinkItem(output, string.Format(emptyLinkFormat, currentPage.ToString()), "active");
                    }
                    else
                    {
                        AppendLinkItem(output,
                            string.Format("<a href=\"{0}\">{1}</a>", pageView.RenderPageUrls[i].Url,
                                pageView.RenderPageUrls[i].PageIndex));
                    }
                }

                if (currentPage < pageView.PageCount)
                {
                    //处理下一页的链接
                    AppendLinkItem(output, string.Format("<a href=\"{0}\">下一页</a>", pageView.NextPageUrl.Url));
                }
                else
                {
                    AppendLinkItem(output, string.Format(emptyLinkFormat, "下一页"), "disabled");
                }
                if (currentPage != pageView.PageCount)
                {
                    AppendLinkItem(output, string.Format("<a href=\"{0}\">末页</a>", pageView.LastPageUrl.Url));
                }
                output.Append("</ul>");
                output.AppendFormat("<div class=\"pull-left\">第 {0}/{1} 页，每页 {2} 条，共 {3} 条</div>", currentPage,
                                    pageView.PageCount, pageView.PageSize,
                                    pageView.RecordCount);
                output.Append("</div>");
            }

            return MvcHtmlString.Create(output.ToString());
        }

        private static void AppendLinkItem(StringBuilder output, string link, string className = "")
        {
            output.AppendFormat("<li{0}>{1}</li>",
                                string.IsNullOrWhiteSpace(className) ? "" : " class=\"" + className + "\"", link);
        }
    }
}
