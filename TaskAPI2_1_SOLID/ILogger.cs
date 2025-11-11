using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskAPI2_1_SOLID
{
    // Интерфейс для логирования
    public interface ILogger
    {
        void Log(string message);
    }
}
