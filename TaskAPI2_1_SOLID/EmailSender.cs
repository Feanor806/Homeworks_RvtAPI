using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskAPI2_1_SOLID
{
    public class EmailSender
    {
        public void SendEmail(string to, string message)
        {
            // Симуляция отправки email
            Console.WriteLine($"Email для {to}: {message}");
        }
    }
}
