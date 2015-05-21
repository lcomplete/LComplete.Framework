using LComplete.Framework.Logging;

namespace LComplete.Framework.Log4Net
{
    public static class AppConfigureExtension
    {
        public static BootConfigure UseLog4Net(this BootConfigure configure, string path)
        {
            LogManager.LogFactory = new Log4NetFactory(path);
            return configure;
        }
    }
}