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
   public class PagingModel
    {
        public int PageSize { get; set; }

        /// <summary>
        /// 页索引
        /// </summary>
        public int PageIndex { get; set; }

        public int RecordCount { get; set; }

        public PagingModel(int pageIndex, int pageSize, int recordCount = 0)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            RecordCount = recordCount;
        }
    }

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

        /// <summary>
        /// 需要输出的链接
        /// </summary>
        public IList<PageUrl> RenderPageUrls { get; set; }

        /// <summary>
        /// 按钮位置是否固定
        /// </summary>
        public bool IsFixButtonPosition { get; set; }

        /// <summary>
        /// 左边的点 链接
        /// </summary>
        public PageUrl LeftDotPageUrl { get; set; }

        /// <summary>
        /// 右边的点 链接
        /// </summary>
        public PageUrl RightDotPageUrl { get; set; }

        public PageViewModel()
        {
            RenderPageUrls=new List<PageUrl>();
        }
    }

    /// <summary>
    /// 分页链接
    /// </summary>
    public class PageUrl
    {
        public int PageIndex { get; set; }

        public string Url { get; set; }
    }

    public class Pager
    {
        /// <summary>
        /// 分页参数名称
        /// </summary>
        internal const string PageIndexName = "page";

        /// <summary>
        /// 分页视图名称
        /// </summary>
        internal  const string pagerPartialView = "_Pager";

        public PageViewModel PageView { get; private set; }

        public HtmlHelper html { get; private set; }

        public bool IsIsAlwaysShow { get; private set; }

        public Pager(HtmlHelper html, PagingModel pagedModel, bool isAlwaysShow, int pageLinkCount, bool isFixLinkPosition)
        {
            this.html = html;
            IsIsAlwaysShow = isAlwaysShow;
            PageView = GetPageViewModel(html.ViewContext, pagedModel, pageLinkCount, isFixLinkPosition);
        }

        /// <summary>
        /// 获取分页视图模型
        /// </summary>
        /// <param name="viewContext"></param>
        /// <param name="pagedEntity"></param>
        /// <param name="pageLinkCount"></param>
        /// <param name="isFixLinkPosition"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private PageViewModel GetPageViewModel(ViewContext viewContext, PagingModel pagedEntity, int pageLinkCount, bool isFixLinkPosition)
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
                IsFixButtonPosition = isFixLinkPosition,
                CurrentPageUrl = GetPageUrl(viewContext, pagedEntity.PageIndex),
                FirstPageUrl = GetPageUrl(viewContext, 1),
                LastPageUrl = GetPageUrl(viewContext, pageCount),
                PrevPageUrl = GetPageUrl(viewContext, pagedEntity.PageIndex - 1),
                NextPageUrl = pageCount > pagedEntity.PageIndex ? GetPageUrl(viewContext, pagedEntity.PageIndex + 1) : null,
            };

            int currentPage = pagedEntity.PageIndex;
            int lowBoundPage;
            int upBoundPage;
            if (isFixLinkPosition)
            {
                //链接位置固定
                lowBoundPage = (currentPage / pageLinkCount - (currentPage % pageLinkCount == 0 ? 1 : 0) ) * pageLinkCount +
                               1;
                upBoundPage = Math.Min(lowBoundPage + pageLinkCount - 1, pageCount);
            }
            else
            {
                //链接始终在中间
                int lowOrderIndex = currentPage - pageLinkCount / 2;
                int upOrderIndex = lowOrderIndex + pageLinkCount - 1;
                lowBoundPage = Math.Max(1, lowOrderIndex);
                upBoundPage = Math.Min(pageCount, upOrderIndex);
                if (lowOrderIndex < 1)
                {
                    upBoundPage = Math.Min(pageCount, pageLinkCount);
                }
                if (upOrderIndex > pageCount)
                {
                    lowBoundPage = Math.Max(1, pageCount - pageLinkCount + 1);
                }
            }

            if (lowBoundPage > 1)
            {
                pageView.LeftDotPageUrl = GetPageUrl(viewContext, lowBoundPage - 1);
            }
            if (upBoundPage < pageCount)
            {
                pageView.RightDotPageUrl = GetPageUrl(viewContext, upBoundPage + 1);
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
        private PageUrl GetPageUrl(ViewContext viewContext, int pageIndex)
        {
            if (pageIndex < 1)
                return null;

            var queryString = viewContext.HttpContext.Request.QueryString;
            var dictRouteValues = new RouteValueDictionary(viewContext.RouteData.Values);
            foreach (string key in queryString.Keys)
                if (queryString[key] != null && !string.IsNullOrEmpty(key))
                    dictRouteValues[key] = queryString[key];
            dictRouteValues[PageIndexName] = pageIndex;

            return new PageUrl()
            {
                PageIndex = pageIndex,
                Url =
                    UrlHelper.GenerateUrl(null, null, null, dictRouteValues, RouteTable.Routes,
                        viewContext.RequestContext, true)
            };
        }

        /// <summary>
        /// 生成页码分页UI
        /// </summary>
        /// <returns></returns>
        public MvcHtmlString GenerateNumericPager()
        {
            if (PageView.PageCount > 1 || IsIsAlwaysShow)
            {
                if (ViewEngines.Engines.FindPartialView(html.ViewContext, pagerPartialView).View != null)
                {
                    return html.Partial(pagerPartialView, PageView);
                }

                return GenerateDefaultPagerHtml();
            }

            return new MvcHtmlString("");
        }

        private MvcHtmlString GenerateDefaultPagerHtml()
        {
            var output = new StringBuilder();
            const string emptyLinkFormat = "<a href=\"javascript:;\">{0}</a>";

            int currentPage = PageView.CurrentPageUrl.PageIndex;
            output.Append("<div class=\"pagination pagination-right\"><ul>");

            if (currentPage != 1)
            {
                //处理首页连接
                AppendLinkItem(output, string.Format("<a href=\"{0}\">首页</a>", PageView.FirstPageUrl.Url));
            }
            if (currentPage > 1)
            {
                //处理上一页的连接
                AppendLinkItem(output, string.Format("<a href=\"{0}\">上一页</a>", PageView.PrevPageUrl.Url));
            }
            else
            {
                AppendLinkItem(output, string.Format(emptyLinkFormat, "上一页"), "disabled");
            }

            if (PageView.IsFixButtonPosition && PageView.LeftDotPageUrl != null)
            {
                AppendLinkItem(output,
                       string.Format("<a href=\"{0}\">...</a>", PageView.LeftDotPageUrl.Url));
            }

            for (int i = 0; i < PageView.RenderPageUrls.Count; i++)
            {
                if (PageView.RenderPageUrls[i].PageIndex == currentPage)
                {
                    AppendLinkItem(output, string.Format(emptyLinkFormat, currentPage.ToString()), "active");
                }
                else
                {
                    AppendLinkItem(output,
                        string.Format("<a href=\"{0}\">{1}</a>", PageView.RenderPageUrls[i].Url,
                            PageView.RenderPageUrls[i].PageIndex));
                }
            }

            if (PageView.IsFixButtonPosition && PageView.RightDotPageUrl != null)
            {
                AppendLinkItem(output,
                       string.Format("<a href=\"{0}\">...</a>", PageView.RightDotPageUrl.Url));
            }

            if (currentPage < PageView.PageCount)
            {
                //处理下一页的链接
                AppendLinkItem(output, string.Format("<a href=\"{0}\">下一页</a>", PageView.NextPageUrl.Url));
            }
            else
            {
                AppendLinkItem(output, string.Format(emptyLinkFormat, "下一页"), "disabled");
            }
            if (currentPage != PageView.PageCount)
            {
                AppendLinkItem(output, string.Format("<a href=\"{0}\">末页</a>", PageView.LastPageUrl.Url));
            }
            output.Append("</ul>");
            output.AppendFormat("<div class=\"pull-left\">第 {0}/{1} 页，每页 {2} 条，共 {3} 条</div>", currentPage,
                                PageView.PageCount, PageView.PageSize,
                                PageView.RecordCount);
            output.Append("</div>");

            return MvcHtmlString.Create(output.ToString());
        }

        private static void AppendLinkItem(StringBuilder output, string link, string className = "")
        {
            output.AppendFormat("<li{0}>{1}</li>",
                                string.IsNullOrWhiteSpace(className) ? "" : " class=\"" + className + "\"", link);
        }
    }

    /// <summary>
    /// 提供一组用于分页的扩展方法。
    /// </summary>
    public static class PagerExtensions
    {
        /// <summary>
        /// 生成分页链接UI
        /// </summary>
        /// <param name="html"></param>
        /// <param name="pagedEntity"></param>
        /// <param name="alwaysShow">不管总页数，每次都显示</param>
        /// <param name="pageLinkCount">显示链接数量</param>
        /// <param name="isFixLinkPosition">按钮位置是否固定</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static MvcHtmlString NumericPager(this HtmlHelper html, PagingModel pagedEntity, bool alwaysShow = true, int pageLinkCount = 10, bool isFixLinkPosition = false)
        {
            if (pagedEntity == null)
                throw new ArgumentNullException("pagedEntity");

            Pager pager = new Pager(html, pagedEntity, alwaysShow, pageLinkCount, isFixLinkPosition);
            return pager.GenerateNumericPager();
        }

    }
}
