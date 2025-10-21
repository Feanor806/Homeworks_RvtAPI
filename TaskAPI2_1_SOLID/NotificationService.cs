using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskAPI2_1_SOLID
{
    public class NotificationService
    {
        private readonly INotificationSender _notificationSender;
        private readonly ILogger _logger;

        public NotificationService(INotificationSender notificationSender, ILogger logger)
        {
            _notificationSender = notificationSender;
            _logger = logger;
        }

        public void SendNotification(string? message, string? recipient)
        {
            try
            {
                // Логика подготовки уведомления
                string formattedMessage = $"Уведомление: {message}";

                // Отправка уведомления через выбранный канал
                _notificationSender.Send(recipient, formattedMessage);

                // Логирование через отдельный сервис
                _logger.Log($"Отправлено уведомление для {recipient}");
            }
            catch (Exception ex)
            {
                _logger.Log($"Ошибка при отправке уведомления для {recipient}: {ex.Message}");
                throw;
            }
        }
    }
}
