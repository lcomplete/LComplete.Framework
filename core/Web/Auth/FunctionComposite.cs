using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace LComplete.Framework.Web.Auth
{
    public class FunctionComposite<T> : FunctionComponent where T : Controller
    {
        /// <summary>
        /// 功能（MVC Action）表达式
        /// </summary>
        private Expression<Func<T, object>> ActionExpression { get; set; }

        private ActionIdentity _actionIdentity;
        public override ActionIdentity ActionIdentity
        {
            get { return _actionIdentity; }
        }

        public FunctionComposite(string name, Expression<Func<T, object>> actionExpression, params FunctionComponent[] functionItems)
            : base(name)
        {
            ActionExpression = actionExpression;
            AnalysisExpression();

            if (functionItems != null && functionItems.Length > 0)
            {
                ChildFunctionItems = new List<FunctionComponent>();
                foreach (var functionComponent in functionItems)
                {
                    functionComponent.ParentFunction = this;
                    ChildFunctionItems.Add(functionComponent);
                }
            }
        }

        /// <summary>
        /// 解析表达式树
        /// </summary>
        private void AnalysisExpression()
        {
            _actionIdentity = ActionExpressionParser.AnalysisExpression(ActionExpression);
        }

        public override void Add(FunctionComponent function)
        {
            if (ChildFunctionItems == null)
            {
                ChildFunctionItems = new List<FunctionComponent>();
            }
            ChildFunctionItems.Add(function);
        }

        public override void Remove(FunctionComponent function)
        {
            if (ChildFunctionItems != null)
                ChildFunctionItems.Remove(function);
        }
    }
}
