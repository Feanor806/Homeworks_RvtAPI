using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskAPI2_1_SOLID
{
    internal class NotificationService
    {
        private readonly EmailSender _emailSender;

        public NotificationService()
        {
            _emailSender = new EmailSender(); // Нарушение: жесткая привязка
        }

        public void SendNotification(string message, string recipient)
        {
            // Логика подготовки уведомления
            string formattedMessage = $"Уведомление: {message}";

            // Отправка email
            _emailSender.SendEmail(recipient, formattedMessage);

            // Нарушение: запись в лог внутри сервиса
            File.WriteAllText("log.txt", $"Отправлено уведомление для {recipient}");
        }
    }
}
