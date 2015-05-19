using System;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using LComplete.Framework.Data;
using LComplete.Framework.Web.Common;

namespace Admin.Extensions
{
    public static class OrderFieldExtensions
    {
        private const string ORDER_PARAM_KEY = "order";

        public static IHtmlString OrderField<TModel>(this HtmlHelper htmlHelper, Expression<Func<TModel, object>> filedExpression, OrderPagingQuery<TModel> orderPagingQuery, string fieldText=null) where TModel : class
        {
            return OrderField(htmlHelper.ViewContext.RequestContext, filedExpression, orderPagingQuery, fieldText);
        }

        private static IHtmlString OrderField<TModel>(RequestContext context, Expression<Func<TModel, object>> filedExpression, OrderPagingQuery<TModel> orderPagingQuery,string fieldText)
            where TModel : class
        {
            OrderFieldStore<TModel> orderFieldStore = orderPagingQuery.OrderFieldStore;
            OrderField<TModel> orderField = orderFieldStore.GetOrderField(filedExpression);

            StringBuilder builder = new StringBuilder();
            if (orderField != null)
            {
                bool isOrdered = true;
                string url;
                if (orderField.OrderType != OrderType.Ascending)
                {
                    url = RequestUrlUtils.CombinationRequestUrl(context, ORDER_PARAM_KEY,
                                                                orderFieldStore.MakeOrderFlags(orderField.OrderKey,
                                                                                               OrderType.Ascending));
                    isOrdered = orderField.OrderType != OrderType.None;
                }
                else
                {
                    url = RequestUrlUtils.CombinationRequestUrl(context, ORDER_PARAM_KEY,
                                                                orderFieldStore.MakeOrderFlags(orderField.OrderKey,
                                                                                               OrderType.Descending));
                }

                TagBuilder wrapper = new TagBuilder("div");
                wrapper.AddCssClass("sortable");
                TagBuilder a = new TagBuilder("a");
                a.AddCssClass("main-text");
                a.Attributes.Add("href", url);

                builder.Append(wrapper.ToString(TagRenderMode.StartTag));
                builder.Append(a.ToString(TagRenderMode.StartTag));
                builder.Append(fieldText);
                builder.Append(a.ToString(TagRenderMode.EndTag));
                
                //已排序 则添加排序选项
                if(isOrdered)
                {
                    TagBuilder sortOptions = new TagBuilder("div");
                    sortOptions.AddCssClass("sort-options");

                    //移除排序链接
                    TagBuilder removeLink = new TagBuilder("a");
                    removeLink.AddCssClass("sort-remove");
                    removeLink.Attributes.Add("href",
                                              RequestUrlUtils.CombinationRequestUrl(context, ORDER_PARAM_KEY,
                                                                                    orderFieldStore.MakeOrderFlags(
                                                                                        orderField.OrderKey,
                                                                                        OrderType.None)));
                    TagBuilder removeIcon = new TagBuilder("i");
                    removeIcon.AddCssClass("icon-remove-circle");

                    //优先级文本
                    TagBuilder prioritySpan = new TagBuilder("span");
                    prioritySpan.AddCssClass("sort-priority");
                    prioritySpan.SetInnerText(orderField.Priority.ToString());

                    //切换排序链接
                    TagBuilder toggleLink = new TagBuilder("a");
                    toggleLink.Attributes.Add("href", url);
                    toggleLink.AddCssClass("sort-toggle");
                    TagBuilder sortIcon=new TagBuilder("i");
                    sortIcon.AddCssClass(orderField.OrderType == OrderType.Descending
                                               ? "icon-chevron-down"
                                               : "icon-chevron-up");

                    builder.Append(sortOptions.ToString(TagRenderMode.StartTag));
                    {
                        builder.Append(removeLink.ToString(TagRenderMode.StartTag));
                        builder.Append(removeIcon.ToString(TagRenderMode.Normal));
                        builder.Append(removeLink.ToString(TagRenderMode.EndTag));

                        builder.Append(prioritySpan.ToString(TagRenderMode.Normal));

                        builder.Append(toggleLink.ToString(TagRenderMode.StartTag));
                        builder.Append(sortIcon.ToString(TagRenderMode.Normal));
                        builder.Append(toggleLink.ToString(TagRenderMode.EndTag));
                    }
                    builder.Append(sortOptions.ToString(TagRenderMode.EndTag));
                }

                builder.Append(wrapper.ToString(TagRenderMode.EndTag));
            }

            return new MvcHtmlString(builder.ToString());
        }
    }
}