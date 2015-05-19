namespace LComplete.Framework.Common
{
    public class IpUtils
    {
        public static long IpToLong(string ip)
        {
            string[] ipArr = ip.Split('.');
            if (ipArr.Length < 3)
                return 0;

            long ipLong = 256*256*256*long.Parse(ipArr[0]) + 256*256*long.Parse(ipArr[1]) + 256*long.Parse(ipArr[2]) +
                          long.Parse(ipArr[3]);
            return ipLong;
        }
    }
}
