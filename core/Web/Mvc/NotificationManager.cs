using System.Web.Mvc;

namespace LComplete.Framework.Web.Mvc
{
    public static class NotificationManager
    {
        public static void Notice(Controller controller, NotificationType notificationType, string message)
        {
            string noticeKey = notificationType.ToString();
            controller.TempData[noticeKey] = message;
        }

    }
}