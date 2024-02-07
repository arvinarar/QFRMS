using Microsoft.Extensions.Logging;
using QFRMS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFRMS.Services.Utils
{
    public class FileLogger : IFileLogger
    {
        public readonly ILogger<FileLogger> _logger;
        private static string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Logs").ToString();

        public FileLogger(ILogger<FileLogger> logger)
        {
            _logger = logger;
            if (!System.IO.Directory.Exists(filePath)) 
            { 
                System.IO.Directory.CreateDirectory(filePath); 
            }
        }

        public void Log(string message, bool flag = false)
        {
            Console.WriteLine(DateTime.Now.ToString() + " : " + message);
            if(flag)
            {
                string fullFilePath = Path.Combine(filePath, DateTime.Now.ToString("yyyy-MM-dd") + "_log.csv");
                string LogText = DateTime.Now.ToString() + ", " + message;
                File.AppendAllText(fullFilePath, LogText + Environment.NewLine);
            }
        }
    }
}
