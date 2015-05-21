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

        /// <summary>
        /// 按字符数 截取字符串
        /// </summary>
        /// <param name="s"></param>
        /// <param name="maxLength"></param>
        /// <param name="suffix"></param>
        /// <returns></returns>
        public static string Truncate(string s, int maxLength, string suffix = "")
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            if (s.Length <= maxLength)
                return s;

            return s.Substring(0, maxLength) + suffix;
        }

        /// <summary>
        /// 按字符（英文）宽度截取字符串
        /// </summary>
        /// <param name="s"></param>
        /// <param name="widthLength"></param>
        /// <param name="suffix"></param>
        /// <returns></returns>
        public static string TruncateWidth(string s, int widthLength, string suffix = "")
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            var cs = s.ToCharArray();
            int length = 0;
            int i = 0;
            while (i < cs.Length)
            {
                int charWidth = 2;
                if (cs[i] >= '\u0000' && cs[i] <= '\u007F')
                    charWidth = 1;

                if ((charWidth + length) <= widthLength)
                {
                    i ++;
                    length += charWidth;
                }
                else
                    break;
            }

            return s.Substring(0, i) + suffix;
        }
    }
}
