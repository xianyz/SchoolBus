using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SchoolBusWXWeb.Models.PmsData;
using SchoolBusWXWeb.Models.SchollBusModels;

namespace SchoolBusWXWeb.Business
{
    public interface ISchoolBusBusines
    {
        Task<twxuser> GetTwxuserAsync(string pkid);
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<int> DoRegister(RegisterModel user);
    }
}
