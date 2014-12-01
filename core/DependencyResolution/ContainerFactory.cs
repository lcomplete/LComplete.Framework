using LComplete.Framework.DependencyResolution.Impl;

namespace LComplete.Framework.DependencyResolution
{
    public class ContainerFactory
    {
        private static IObjectContainer _objectContainer;

        public static IObjectContainer Singleton
        {
            get
            {
                if(_objectContainer==null)
                    _objectContainer=new StructureMapContainer();

                return _objectContainer;
            }
        }
    }
}
