namespace Admin.Extensions
{
    public static class StringExtensions
    {
        public static string TrimEndAt(this string str, string atChar)
        {
            if (str.IndexOf(atChar) >= 0)
            {
                return str.Substring(0,str.IndexOf(atChar));
            }

            return str;
        }
    }
}