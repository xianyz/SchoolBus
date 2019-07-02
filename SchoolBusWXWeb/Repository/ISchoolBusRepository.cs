using SchoolBusWXWeb.Models.PmsData;
using SchoolBusWXWeb.Models.SchollBusModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SchoolBusWXWeb.Models.ViewData;


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
        Task<tconfig> GetSchoolConfigAsync(string fcode);
        Task<List<tconfig>> GetSchoolConfigListAsync(string fcodes);
        Task<int> UpdateTCardAsync(tcard card);
        Task<int> InsertSMSCodeAsync(tsms sms);
        Task<UserAndCardModel> GetUserAndCardByOpenidAsync(string wxopenid);
        Task<List<SchoolBaseInfo>> GetSchoolListByPlatenumber(string platenumber);
    }
}
