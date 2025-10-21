using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskAPI2_1_SOLID
{
    // Реализация SMS отправителя
    public class SmsSender : INotificationSender
    {
        public void Send(string? recipient, string? message)
        {
            string formattedMessage = $"SMS: {message}";
            Console.WriteLine($"SMS для {recipient}: {formattedMessage}");
        }
    }
}
