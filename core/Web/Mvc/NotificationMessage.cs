namespace LComplete.Framework.Web.Mvc
{
    public class NotificationMessage
    {
        public NotificationType NotificationType { get; set; }

        public string Message { get; set; }

        public NotificationMessage(NotificationType notificationType,string message)
        {
            NotificationType = notificationType;
            Message = message;
        }
    }
}