using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SchoolBusWXWeb.Models.SchollBusModels;

namespace SchoolBusWXWeb.Business
{
    public interface ISchoolBusBusines
    {
        Task<twxuser> GetTwxuserAsync();
    }
}
