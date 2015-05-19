using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Admin.Extensions
{
    public static class FormExtensions
    {
        public static IHtmlString RenderControlGroupFor<T>(this HtmlHelper<T> html,
                                                           Expression<Func<T, object>> modelProperty,
                                                           object htmlAttributes = null, object editorAttributes = null,
                                                           object validationAttributes = null)
        {
            return RenderControlGroupFor<T>(html, modelProperty,
                                            HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes),
                                            HtmlHelper.AnonymousObjectToHtmlAttributes(editorAttributes),
                                            HtmlHelper.AnonymousObjectToHtmlAttributes(validationAttributes));
        }

        public static IHtmlString RenderControlGroupFor<T>(this HtmlHelper<T> html,
                                                           Expression<Func<T, object>> modelProperty,
                                                           IDictionary<string, object> htmlAttributes,
                                                           IDictionary<string, object> editorAttributes,
                                                           IDictionary<string, object> validationAttributes)
        {
            var meta = ModelMetadata.FromLambdaExpression(modelProperty, html.ViewData);

            var controlGroupWrapper = new TagBuilder("div");
            controlGroupWrapper.MergeAttributes(htmlAttributes);
            controlGroupWrapper.AddCssClass("control-group");
            string propertyName = ExpressionHelper.GetExpressionText(modelProperty);
            string partialFieldName = propertyName;
            string fullHtmlFieldName =
                html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(partialFieldName);
            if (!html.ViewData.ModelState.IsValidField(fullHtmlFieldName))
            {
                controlGroupWrapper.AddCssClass("error");
            }

            var label = new TagBuilder("label");
            label.AddCssClass("control-label");
            label.Attributes.Add("for", fullHtmlFieldName);
            label.InnerHtml = meta.DisplayName;

            var controls = new TagBuilder("div");
            controls.AddCssClass("controls");
            if (editorAttributes == null)
                editorAttributes = new Dictionary<string, object>();
            if (!editorAttributes.ContainsKey("class"))
                editorAttributes["class"] = "input-xlarge";
            //TODO editorAttributes is not work
            MvcHtmlString editor = html.EditorFor(modelProperty, null, fullHtmlFieldName, editorAttributes);
            if (validationAttributes == null)
                validationAttributes = new Dictionary<string, object>();
            if (!validationAttributes.ContainsKey("class"))
                validationAttributes["class"] = "help-block";
            MvcHtmlString validation = html.ValidationMessageFor(modelProperty, null, validationAttributes);

            StringBuilder builder = new StringBuilder(7);
            builder.Append(controlGroupWrapper.ToString(TagRenderMode.StartTag));
            builder.Append(label.ToString());
            builder.Append(controls.ToString(TagRenderMode.StartTag));
            builder.Append(editor.ToString());
            builder.Append(validation.ToString());
            builder.Append(controls.ToString(TagRenderMode.EndTag));
            builder.Append(controlGroupWrapper.ToString(TagRenderMode.EndTag));

            return new MvcHtmlString(builder.ToString());
        }
    }

}