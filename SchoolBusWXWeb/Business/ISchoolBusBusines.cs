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

        /// <summary>
        /// 获取用户和卡信息
        /// </summary>
        /// <param name="wxid"></param>
        /// <returns></returns>
        Task<UserAndCardModel> GetCardInfoByCodeAsync(string wxid);
        
        /// <summary>
        /// 根据车牌号获取托运的学校
        /// </summary>
        /// <param name="platenumber"></param>
        /// <returns></returns>
        Task<SchoolVD> GetSchoolListByPlatenumberAsync(string platenumber);

        /// <summary>
        /// 完善用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<SaveCardInfoVD> SavaCardAndUserInfoAsync(UserAndCardModel model);

        /// <summary>
        /// 解绑
        /// </summary>
        /// <param name="wxid"></param>
        /// <returns></returns>
        Task<BaseVD> UntringAsync(string wxid);
    }
}
