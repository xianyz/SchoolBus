using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SchoolBusWXWeb.Models.SchollBusModels;

namespace SchoolBusWXWeb.Repository
{
    public interface ISchoolBusRepository
    {
        Task<twxuser> GetTwxuserAsync();
    }
}
