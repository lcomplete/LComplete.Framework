namespace LComplete.Framework.IoC
{
    public class ContainerManager
    {
        private static IContainerFactory _containerFactory;

        public static IContainerFactory ContainerFactory
        {
            get { return _containerFactory; }
            set { _containerFactory = value; }
        }

        public static IContainer GetContainer()
        {
            return ContainerFactory.GetContainer();
        }

        public static T Resolve<T>()
        {
            return GetContainer().Resolve<T>();
        }
    }
}