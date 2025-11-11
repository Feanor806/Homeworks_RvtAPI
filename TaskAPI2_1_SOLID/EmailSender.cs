using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskAPI2_1_SOLID
{
    public class EmailSender : INotificationSender
    {
        public void Send(string? recipient, string? message)
        {
            string formattedMessage = $"Email уведомление: {message}";
            Console.WriteLine($"Email для {recipient}: {formattedMessage}");
        }
    }
}
