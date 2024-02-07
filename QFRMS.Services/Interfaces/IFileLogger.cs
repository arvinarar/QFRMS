using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFRMS.Services.Interfaces
{
    public interface IFileLogger
    {
        void Log(string message, bool flag = false);
    }
}
