using SchoolBusWXWeb.Models.PmsData;
using SchoolBusWXWeb.Models.SchollBusModels;
using SchoolBusWXWeb.Models.ViewData;
using System.Threading.Tasks;

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
        Task<RegisVD> DoRegisterAsync(RegisterModel user);
        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="sms"></param>
        /// <returns></returns>
        Task<SmsVD> SendSmsCodeAsync(SmsModel sms);
    }
}
