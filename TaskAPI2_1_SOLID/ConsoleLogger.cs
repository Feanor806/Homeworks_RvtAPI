using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskAPI2_1_SOLID
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine($" LOG: {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
        }
    }
}
