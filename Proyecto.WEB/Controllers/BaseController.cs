using Microsoft.AspNetCore.Mvc;

namespace Proyecto.WEB.Controllers
{
    public abstract class BaseController : Controller
    {
        protected enum NotificationType
        {
            Success,
            Error,
            Info
        }

        protected void SetNotification(string message, NotificationType type)
        {
            TempData["Notification.Message"] = message;
            TempData["Notification.Type"] = type.ToString().ToLowerInvariant();
        }
    }
}
