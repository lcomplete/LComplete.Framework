using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LComplete.Framework.Common
{
    public static class StringUtils
    {
        /// <summary>
        /// 转义sql中的'
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FilterSql(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return str;
            return str.Replace("'", "''").Trim();
        }
    }
}
