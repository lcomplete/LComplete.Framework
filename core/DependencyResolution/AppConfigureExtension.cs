using LComplete.Framework.IoC;

namespace LComplete.Framework.DependencyResolution
{
    public static class AppConfigureExtension
    {
        public static BootConfigure UseStructureMap(this BootConfigure configure,params string[] scanAssembles)
        {
            ContainerManager.ContainerFactory = new StructureMapContainerFactory(scanAssembles);
            return configure;
        }
    }
}