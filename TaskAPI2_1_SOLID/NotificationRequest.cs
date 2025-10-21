using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskAPI2_1_SOLID
{
    // Модель для конфигурации уведомления
    public class NotificationRequest
    {
        public string? Message { get; set; }
        public string? Recipient { get; set; }
        public NotificationType Type { get; set; }
    }

    // Типы уведомлений
    public enum NotificationType
    {
        Email,
        SMS,
        Push
    }
}
