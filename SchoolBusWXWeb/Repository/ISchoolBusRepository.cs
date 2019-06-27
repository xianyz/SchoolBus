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
        Task<tcard> GetCardByCodeAsync(string fcode);
        Task<List<tsms>> GetSmsListBySendTimeAsync(string phone, int type,DateTime st,DateTime et);
        Task<int> InsertWxUserAsync(twxuser user);
        Task<int> UpdateUserCardAsync(string oldcard, string newcard);
        Task<int> UpdateWxUserAsync(twxuser user);
        Task<tconfig> GetSchoolConfig(string fcode);
        Task<int> UpdateTCardAsync(tcard card);
    }
}
