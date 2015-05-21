using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using LComplete.Framework.IoC;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using IContainer = LComplete.Framework.IoC.IContainer;

namespace LComplete.Framework.DependencyResolution
{
    public class StructureMapContainerFactory : IContainerFactory
    {
        private IContainer _container;

        private IList<string> _scanAssembles;

        public StructureMapContainerFactory(params string[] scanAssembles)
        {
            _scanAssembles = scanAssembles.ToList();
        }


        public IContainer GetContainer()
        {
            if (_container == null)
            {
                _container = new StructureMapContainer(GetInitContainer());
            }

            return _container;
        }

        private StructureMap.IContainer GetInitContainer()
        {
            //要扫描依赖注入的程序集
            IList<string> listAssembles = GetScanAssembles();

            //要注入的条目
            IList<Registry> listRegistries = GetRegistries();

            //条目转换对象
            IList<IRegistrationConvention> listConvertions = GetRegistrationConvention();

            Container container = new Container(expression =>
            {
                expression.Scan(
                    assemblyScanner =>
                    {
                        //扫描程序集
                        foreach (string assemble in listAssembles)
                        {
                            assemblyScanner.Assembly(assemble.Trim());
                        }

                        //设置转换对象
                        if (listConvertions == null || listConvertions.Count == 0)
                            assemblyScanner.WithDefaultConventions();
                        else
                        {
                            foreach (IRegistrationConvention convention in listConvertions)
                            {
                                assemblyScanner.With(convention);
                            }
                        }
                    });

                //增加注入条目
                foreach (Registry registry in listRegistries)
                {
                    expression.AddRegistry(registry);
                }

            });

            return container;
        }


        private IList<string> GetScanAssembles()
        {
            IList<string> listAssembels = _scanAssembles != null ? _scanAssembles : new List<string>();
            
            string setting = ConfigurationManager.AppSettings["DI_Assembles"];
            var configAssembles = string.IsNullOrWhiteSpace(setting)
                                         ? null
                                         : setting.Split(',');
            
            if (configAssembles != null)
            {
                //添加配置中的程序集
                Array.ForEach(configAssembles, listAssembels.Add);
            }

            return listAssembels.Distinct().ToList();
        }

        private IList<Registry> GetRegistries()
        {
            IList<Registry> listRegistries = new List<Registry>();
            string setting = ConfigurationManager.AppSettings["DI_Registries"];
            var configRegistries = string.IsNullOrWhiteSpace(setting) ? null : setting.Split(';');
            if (configRegistries != null)
            {
                Array.ForEach(configRegistries, s =>
                {
                    Registry registry =
                        (Registry)Activator.CreateInstance(Type.GetType(s));
                    listRegistries.Add(registry);
                });
            }

            return listRegistries;
        }

        private IList<IRegistrationConvention> GetRegistrationConvention()
        {
            IList<IRegistrationConvention> listConvertions = new List<IRegistrationConvention>();
            string setting = ConfigurationManager.AppSettings["DI_Convertions"];
            var configConvertions = string.IsNullOrWhiteSpace(setting) ? null : setting.Split(';');
            if (configConvertions != null)
            {
                Array.ForEach(configConvertions, s =>
                {
                    IRegistrationConvention convention =
                        (IRegistrationConvention)Activator.CreateInstance(Type.GetType(s));
                    listConvertions.Add(convention);
                });
            }

            return listConvertions;
        }
    }
}
