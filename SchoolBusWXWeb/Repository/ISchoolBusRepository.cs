using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SchoolBusWXWeb.Models.SchollBusModels;

namespace SchoolBusWXWeb.Repository
{
    public interface ISchoolBusRepository
    {
        Task<twxuser> GetTwxuserBypkidAsync(string pkid);
        Task<twxuser> GetTwxuserBytOpenidAsync(string openid);
        Task<tcard> GetCardBypkidAsync(string pkid);
        Task<tcard> GetCardByCode(string fcode);
    }
}
