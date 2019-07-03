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
        /// <summary>
        /// 根据主键获取用户信息
        /// </summary>
        /// <param name="pkid"></param>
        /// <returns></returns>
        Task<twxuser> GetTwxuserBypkidAsync(string pkid);

        /// <summary>
        /// 根据微信openid获取用户注册信息
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        Task<twxuser> GetTwxuserBytOpenidAsync(string openid);

        /// <summary>
        /// 根据主键返回卡信息
        /// </summary>
        /// <param name="pkid"></param>
        /// <returns></returns>
        Task<tcard> GetCardBypkidAsync(string pkid);

        /// <summary>
        /// 根据卡号返回信息
        /// </summary>
        /// <param name="fcode"></param>
        /// <returns></returns>
        Task<tcard> GetCardByCodeAsync(string fcode);

        /// <summary>
        /// 返回当前时间前10分钟的发送短信列表
        /// </summary>
        /// <param name="phone">0</param>
        /// <param name="type">0</param>
        /// <param name="st">发送时间的开始时间</param>
        /// <param name="et">发送时间的结束时间</param>
        /// <returns></returns>
        Task<List<tsms>> GetSmsListBySendTimeAsync(string phone, int type,DateTime st,DateTime et);

        /// <summary>
        /// 添加注册用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<int> InsertWxUserAsync(twxuser user);

        /// <summary>
        /// 更新用户持有卡
        /// </summary>
        /// <param name="oldcard"></param>
        /// <param name="newcard"></param>
        /// <returns></returns>
        Task<int> UpdateUserCardAsync(string oldcard, string newcard);

        /// <summary>
        /// 更新微信用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<int> UpdateWxUserAsync(twxuser user);

        /// <summary>
        /// 获取一些专用配置
        /// </summary>
        /// <param name="fcode"></param>
        /// <returns></returns>
        Task<tconfig> GetSchoolConfigAsync(string fcode);

        /// <summary>
        /// 获取配置信息列表
        /// </summary>
        /// <param name="fcodes"></param>
        /// <returns></returns>
        Task<List<tconfig>> GetSchoolConfigListAsync(string fcodes);

        /// <summary>
        /// 更新卡片信息
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        Task<int> UpdateTCardAsync(tcard card);

        /// <summary>
        /// 插入发送短信验证码信息
        /// </summary>
        /// <param name="sms"></param>
        /// <returns></returns>
        Task<int> InsertSmsCodeAsync(tsms sms);

        /// <summary>
        /// 根据微信openid获取用户和绑定卡号详细信息
        /// </summary>
        /// <param name="wxopenid"></param>
        /// <returns></returns>
        Task<UserAndCardModel> GetUserAndCardByOpenidAsync(string wxopenid);

        /// <summary>
        /// 根据车牌号获取托运的学校
        /// </summary>
        /// <param name="platenumber"></param>
        /// <returns></returns>
        Task<List<SchoolBaseInfo>> GetSchoolListByPlatenumber(string platenumber);

        /// <summary>
        /// 根据车牌号查询设备信息
        /// </summary>
        /// <param name="platenumber"></param>
        /// <returns></returns>
        Task<tdevice> GetDeviceByPlatenumber(string platenumber);

        /// <summary>
        /// 根据学校名称获取学校信息
        /// </summary>
        /// <param name="fname"></param>
        /// <returns></returns>
        Task<tschool> GetSchoolByName(string fname);

        /// <summary>
        /// 校车公司跟服务学校关系 多对多关系
        /// </summary>
        /// <param name="companid"></param>
        /// <param name="schoolid"></param>
        /// <returns></returns>
        Task<tcompany_school> GetCompanySchoolRel(string companid, string schoolid);
    }
}
