using Microsoft.Extensions.Logging;
using QFRMS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QFRMS.Data.Constants;

namespace QFRMS.Services.Utils
{
    public class FileLogger : IFileLogger
    {
        public readonly ILogger<FileLogger> _logger;
        private static string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Logs").ToString();
        private static string InfoLogPath = Path.Combine(filePath, "User_Logs").ToString();
        private static string ErrorLogPath = Path.Combine(filePath, "Error_Logs").ToString();

        public FileLogger(ILogger<FileLogger> logger)
        {
            _logger = logger;
            if (!System.IO.Directory.Exists(InfoLogPath)) 
            { 
                System.IO.Directory.CreateDirectory(InfoLogPath); 
            }
            if (!System.IO.Directory.Exists(ErrorLogPath))
            {
                System.IO.Directory.CreateDirectory(ErrorLogPath);
            }
        }

        public void Log(string logType, string message, bool flag = false)
        {
            Console.WriteLine(DateTime.Now.ToString("hh:mm:ss tt") + " : " + message);
            if(flag)
            {
                if(logType != LogType.ErrorType)
                {
                    string fullFilePath = Path.Combine(InfoLogPath, DateTime.Now.ToString("yyyy-MM-dd") + "_info_log.csv");
                    string LogText = DateTime.Now.ToString("hh:mm:ss tt") + ", " + message;
                    File.AppendAllText(fullFilePath, LogText + Environment.NewLine);
                }
                else if(logType == LogType.ErrorType)
                {
                    string fullFilePath = Path.Combine(ErrorLogPath, DateTime.Now.ToString("yyyy-MM-dd") + "_error_log.csv");
                    string LogText = DateTime.Now.ToString("hh:mm:ss tt") + ", " + message;
                    File.AppendAllText(fullFilePath, LogText + Environment.NewLine + Environment.NewLine);
                }
            }
        }
    }
}
