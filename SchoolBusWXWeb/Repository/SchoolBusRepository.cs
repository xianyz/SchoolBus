using Microsoft.Extensions.Options;
using SchoolBusWXWeb.Models;
using SchoolBusWXWeb.Models.SchollBusModels;
using SchoolBusWXWeb.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace SchoolBusWXWeb.Repository
{
    public class SchoolBusRepository : RepositoryBase, ISchoolBusRepository
    {
        public SchoolBusRepository(IOptions<SiteConfig> settings) : base(settings.Value.DefaultConnection) { }
        /// <summary>
        /// 根据主键获取用户信息
        /// </summary>
        /// <param name="pkid"></param>
        /// <returns></returns>
        public async Task<twxuser> GetTwxuserBypkidAsync(string pkid)
        {
            const string sql = "select * from public.twxuser where pkid=@pkid";
            var p = new DynamicParameters();
            p.Add("@pkid", pkid.TrimEnd());
            return await GetEntityAsync<twxuser>(sql, p);
        }

        /// <summary>
        /// 根据微信openid获取用户注册信息
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public async Task<twxuser> GetTwxuserBytOpenidAsync(string openid)
        {
            const string sql = "SELECT * from public.twxuser where fstate=0 and fwxid=@fwxid";
            var p = new DynamicParameters();
            p.Add("@fwxid", openid.TrimEnd());
            return await GetEntityAsync<twxuser>(sql, p);
        }

        /// <summary>
        /// 根据主键返回卡信息
        /// </summary>
        /// <param name="pkid"></param>
        /// <returns></returns>
        public async Task<tcard> GetCardBypkidAsync(string pkid)
        {
            const string sql = "select * from public.tcard where pkid=@pkid";
            var p = new DynamicParameters();
            p.Add("@pkid", pkid.TrimEnd());
            return await GetEntityAsync<tcard>(sql, p);
        }

        /// <summary>
        /// 根据卡号返回信息
        /// </summary>
        /// <param name="fcode"></param>
        /// <returns></returns>
        public async Task<tcard> GetCardByCodeAsync(string fcode)
        {
            const string sql = "SELECT tcard.pkid, tcard.fcode, tcard.fstatus,tcard.fname,tcard.ftrialdate, tdevice.fplatenumber,tschool.fname AS fschoolname FROM tcard LEFT JOIN tdevice ON tcard.fk_device_id = tdevice.pkid LEFT JOIN tschool ON tcard.fk_school_id = tschool.pkid WHERE tcard.fcode = @fcode";
            var p = new DynamicParameters();
            p.Add("@fcode", fcode.TrimEnd());
            return await GetEntityAsync<tcard>(sql, p);
        }

        /// <summary>
        /// 返回当前时间前10分钟的发送短信列表
        /// </summary>
        /// <param name="phone">0</param>
        /// <param name="type">0</param>
        /// <param name="st">发送时间的开始时间</param>
        /// <param name="et">发送时间的结束时间</param>
        /// <returns></returns>
        public async Task<List<tsms>> GetSmsListBySendTimeAsync(string phone, int type, DateTime st, DateTime et)
        {
            const string sql = "SELECT * from tsms where fphone = @phone and fsendtime >= @st and fsendtime <= @et and ftype = @ftype order by fsendtime desc";
            var p = new DynamicParameters();
            p.Add("@phone", phone);
            p.Add("@ftype", type);
            p.Add("@st", st);
            p.Add("@et", et);
            var em = await GetAllEntityAsync<tsms>(sql, p);
            return em.ToList();
        }

        /// <summary>
        /// 添加注册用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<int> InsertWxUserAsync(twxuser user)
        {
            const string sql = "INSERT INTO public.twxuser(pkid,fwxid,fname,frelationship,fphone,fstatus,fremark,fcreatetime) VALUES(@pkid, @fwxid, @fname, @frelationship, @fphone, @fstatus, @fremark, @fcreatetime)";
            return await ExecuteEntityAsync(sql, user);
        }

        /// <summary>
        /// 更新用户持有卡
        /// </summary>
        /// <param name="oldcard"></param>
        /// <param name="newcard"></param>
        /// <returns></returns>
        public async Task<int> UpdateUserCardAsync(string oldcard, string newcard)
        {
            const string sql = "UPDATE public.twxuser SET fk_card_id = @newcard WHERE pkid IN (SELECT pkid FROM public.twxuser WHERE fk_card_id = @oldcard)";
            var p = new DynamicParameters();
            p.Add("@oldcard", oldcard.TrimEnd());
            p.Add("@newcard", newcard.TrimEnd());
            return await ExecuteEntityAsync(sql, p);
        }

        /// <summary>
        /// 更新微信用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<int> UpdateWxUserAsync(twxuser user)
        {
            const string sql = "UPDATE public.twxuser SET fk_card_id=@fk_card_id,frelationship=@frelationship,fphone=@fphone WHERE pkid=@pkid";
            return await ExecuteEntityAsync(sql, user);
        }

        /// <summary>
        /// 获取一些专用配置
        /// </summary>
        /// <param name="fcode"></param>
        /// <returns></returns>
        public async Task<tconfig> GetSchoolConfigAsync(string fcode)
        {
            const string sql = "select * from public.tconfig where fcode=@fcode";
            var p = new DynamicParameters();
            p.Add("@fcode", fcode.TrimEnd());
            return await GetEntityAsync<tconfig>(sql, p);
        }

        /// <summary>
        /// 更新卡片信息
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public async Task<int> UpdateTCardAsync(tcard card)
        {
            const string sql = @"update public.tcard set fname=@fname,fsex=@fsex,fk_school_id=@fk_school_id,fk_device_id=@fk_device_id,fboardingaddress=@fboardingaddress
                , fbirthdate = @fbirthdate, ftrialdate = @ftrialdate, fstatus = @fstatus where pkid = @pkid";
            return await ExecuteEntityAsync(sql, card);
        }
    }
}
