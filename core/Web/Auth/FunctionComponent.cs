using System;
using System.Collections.Generic;

namespace LComplete.Framework.Web.Auth
{
    public abstract class FunctionComponent
    {
        /// <summary>
        /// 功能名称
        /// </summary>
        public String Name { get; private set; }

        /// <summary>
        /// 父功能节点
        /// </summary>
        public FunctionComponent ParentFunction { get; set; }

        /// <summary>
        /// 图标css类名
        /// </summary>
        public string IconClass { get; set; }

        /// <summary>
        /// 是否在导航上显示
        /// </summary>
        public bool IsShowOnNav { get; set; }

        /// <summary>
        /// 功能键（唯一，当无对应动作时返回NULL）
        /// </summary>
        public string FunctionKey
        {
            get
            {
                if (ActionIdentity != null)
                    return ActionIdentity.FunctionKey;
                return null;
            }
        }

        /// <summary>
        /// 功能所属控制器
        /// </summary>
        public string ControllerName
        {
            get
            {
                if (ActionIdentity != null)
                    return ActionIdentity.ControllerName;
                return null;
            }
        }

        /// <summary>
        /// 功能对应的动作
        /// </summary>
        public string ActionName
        {
            get
            {
                if (ActionIdentity != null)
                    return ActionIdentity.ActionName;
                return null;
            }
        }

        public string AreaName
        {
            get
            {
                if (ActionIdentity != null)
                    return ActionIdentity.AreaName;
                return null;
            }
        }

        /// <summary>
        /// 动作标识
        /// </summary>
        public abstract ActionIdentity ActionIdentity { get; }

        public bool HasUrl { get { return !string.IsNullOrEmpty(ControllerName) && !string.IsNullOrEmpty(ActionName); } }

        /// <summary>
        /// 子功能集合
        /// </summary>
        public IList<FunctionComponent> ChildFunctionItems { get; set; }

        protected FunctionComponent(string name)
        {
            IsShowOnNav = true;
            Name = name;
        }

        public abstract void Add(FunctionComponent function);

        public abstract void Remove(FunctionComponent function);

    }
}
