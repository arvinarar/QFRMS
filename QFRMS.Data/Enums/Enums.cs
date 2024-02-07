using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFRMS.Data.Enums
{
    public class Enums
    {
        public enum UserRoles
        {
            [Description("Admin")]
            Admin,

            [Description("Registrar")]
            Registrar,

            [Description("Trainor")]
            Trainor
        }
    }
}
