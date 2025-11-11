using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskAPI2_1_SOLID
{
    // Реализация Push отправителя
    public class PushSender : INotificationSender
    {
        public void Send(string? recipient, string? message)
        {
            string formattedMessage = $"Push: {message}";
            Console.WriteLine($"Push уведомление для {recipient}: {formattedMessage}");
        }
    }
}
