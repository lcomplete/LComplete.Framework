namespace LComplete.Framework
{
    /// <summary>
    /// app启动配置，可通过扩展方法设置组件
    /// </summary>
    public class BootConfigure
    {
        public static BootConfigure Boot()
        {
            return new BootConfigure();
        }
    }
}