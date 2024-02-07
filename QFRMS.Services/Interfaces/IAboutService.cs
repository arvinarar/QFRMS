using QFRMS.Data.Models;
using QFRMS.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFRMS.Services.Interfaces
{
    public interface IAboutService
    {
        public Task<InstituteInfo> GetInstituteInfoAsync();
        public Task<Work> UpdateInstituteInfoAsync(InstituteInfo model);
    }
}
