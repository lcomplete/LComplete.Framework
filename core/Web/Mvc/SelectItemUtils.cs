using System;
using System.Collections.Generic;
using System.Web.Mvc;
using LComplete.Framework.Common;

namespace LComplete.Framework.Web.Mvc
{
    public class SelectItemUtils
    {
        public static IList<SelectListItem> EnumTypeToItems(Type enumType, int? value)
        {
            var enumValues = Enum.GetValues(enumType);
            var result = new List<SelectListItem>();
            foreach (var enumValue in enumValues)
            {
                int key= Convert.ToInt32(enumValue);
                string desc = ReflectUtils.GetDescription((Enum) enumValue);
                var item = new SelectListItem()
                {
                    Text = desc,
                    Value = key.ToString(),
                    Selected = key == value
                };
                result.Add(item);
            }

            return result;
        }
    }
}