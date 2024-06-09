using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QFRMS.Data.Constants;

namespace QFRMS.Services.Interfaces
{
    public interface IFileLogger
    {
        void Log(string logType, string message, bool flag = false);
    }
}
