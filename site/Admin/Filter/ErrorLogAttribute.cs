using System.Web.Mvc;
using LComplete.Framework.Logging;

namespace Admin.Filter
{
    public class ErrorLogAttribute:HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            ILog logger = LogManager.GetLogger(this.GetType());
            logger.Error(filterContext.Exception.Message, filterContext.Exception);

            base.OnException(filterContext);
        }
    }
}