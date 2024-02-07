using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFRMS.Data
{
    public static class Constants
    {
        public class LogType
        {
            public const string UserType = "User";
            public const string ErrorType = "Error";
            public const string DatabaseType = "Database";
        }

        public class ErrorType
        { 
            public const string Generic = "GenericError";
            public const string Argument = "ArgumentError";
        }

        public class SortDirection
        {
            public const string Ascending = "Ascending";
            public const string Descending = "Descending";
        }
    }
}
