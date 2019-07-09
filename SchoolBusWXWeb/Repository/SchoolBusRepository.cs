using Microsoft.Extensions.Options;
using SchoolBusWXWeb.Models;
using SchoolBusWXWeb.Models.SchollBusModels;
using SchoolBusWXWeb.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SchoolBusWXWeb.Models.PmsData;
using SchoolBusWXWeb.Models.ViewData;

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
        public async Task<twxuser> GetTwxuserByOpenidAsync(string openid)
        {
            const string sql = "SELECT * from public.twxuser where fstate=0 and fwxid=@fwxid";
            var p = new DynamicParameters();
            p.Add("@fwxid", openid.TrimEnd());
            return await GetEntityAsync<twxuser>(sql, p);
        }

        /// <summary>
        /// 根据微信openid获取用户注册信息(不带用户状态)
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public async Task<twxuser> GetTwxuserNotStateByOpenidAsync(string openid)
        {
            const string sql = "SELECT * from public.twxuser where fwxid=@fwxid";
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
            const string sql = "SELECT tcard.pkid, tcard.fcode, tcard.fstatus,tcard.fname,tcard.ftrialdate,tcard.fboardingaddress,tdevice.fplatenumber,tschool.fname AS fschoolname FROM tcard LEFT JOIN tdevice ON tcard.fk_device_id = tdevice.pkid LEFT JOIN tschool ON tcard.fk_school_id = tschool.pkid WHERE tcard.fcode = @fcode";
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
            const string sql = "INSERT INTO public.twxuser(pkid,fwxid,fk_card_id,fname,frelationship,fphone,fstate,fremark,fcreatetime) VALUES(@pkid, @fwxid,@fk_card_id, @fname, @frelationship, @fphone, @fstate, @fremark, @fcreatetime)";
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
        /// 获取配置信息列表
        /// </summary>
        /// <param name="fcodes"></param>
        /// <returns></returns>
        public async Task<List<tconfig>> GetSchoolConfigListAsync(string fcodes)
        {
            string sql = "select * from public.tconfig where fcode in (" + fcodes + ")";
            var em = await GetAllEntityAsync<tconfig>(sql);
            return em.ToList();
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

        /// <summary>
        /// 插入发送短信验证码信息
        /// </summary>
        /// <param name="sms"></param>
        /// <returns></returns>
        public async Task<int> InsertSmsCodeAsync(tsms sms)
        {
            const string sql = "insert INTO public.tsms(pkid,fphone,fvcode,fsendtime,finvalidtime,ftype)values(@pkid,@fphone,@fvcode,@fsendtime,@finvalidtime,@ftype)";
            return await ExecuteEntityAsync(sql, sms);
        }

        /// <summary>
        /// 根据微信openid获取用户和绑定卡号详细信息
        /// </summary>
        /// <param name="wxopenid"></param>
        /// <returns></returns>
        public async Task<UserAndCardModel> GetUserAndCardByOpenidAsync(string wxopenid)
        {
            const string sql = @"select twxuser.pkid as wxpkid,twxuser.fphone,twxuser.frelationship,
            tcard.pkid, tcard.fcode, tcard.fstatus,tcard.fname,tcard.fbirthdate,tcard.fboardingaddress,
            tdevice.fplatenumber,tschool.fname AS fschoolname from public.twxuser
            left join public.tcard on twxuser.fk_card_id=tcard.pkid
            left join public.tdevice ON tcard.fk_device_id = tdevice.pkid
            left join public.tschool ON tcard.fk_school_id = tschool.pkid
            where twxuser.fwxid=@fwxid";
            var p = new DynamicParameters();
            p.Add("@fwxid", wxopenid.TrimEnd());
            return await GetEntityAsync<UserAndCardModel>(sql, p);
        }

        /// <summary>
        /// 根据车牌号获取托运的学校
        /// </summary>
        /// <param name="platenumber"></param>
        /// <returns></returns>
        public async Task<List<SchoolBaseInfo>> GetSchoolListByPlatenumberAsync(string platenumber)
        {
            const string sql = @"SELECT tschool.ftype, tschool.pkid AS value,tschool.fname AS text FROM tschool 
                                INNER JOIN tcompany_school ON tschool.pkid = tcompany_school.fk_school_id
                                INNER JOIN tdevice ON tcompany_school.fk_company_id = tdevice.fk_company_id
                                WHERE tdevice.fplatenumber = @fplatenumber ORDER BY text";
            var p = new DynamicParameters();
            p.Add("@fplatenumber", platenumber);
            var em = await GetAllEntityAsync<SchoolBaseInfo>(sql, p);
            return em.ToList();
        }

        /// <summary>
        /// 根据车牌号查询设备信息
        /// </summary>
        /// <param name="platenumber"></param>
        /// <returns></returns>
        public async Task<tdevice> GetDeviceByPlatenumberAsync(string platenumber)
        {
            const string sql = "SELECT * FROM tdevice WHERE fstate = 0 AND fplatenumber =@fplatenumber";
            var p = new DynamicParameters();
            p.Add("@fplatenumber", platenumber);
            return await GetEntityAsync<tdevice>(sql, p);
        }

        /// <summary>
        /// 根据主键查询设备信息
        /// </summary>
        /// <param name="pkid"></param>
        /// <returns></returns>
        public async Task<tdevice> GetDeviceByPkidAsync(string pkid)
        {
            const string sql = "SELECT * FROM tdevice WHERE fstate = 0 AND pkid =@pkid";
            var p = new DynamicParameters();
            p.Add("@pkid", pkid);
            return await GetEntityAsync<tdevice>(sql, p);
        }

        /// <summary>
        /// 根据学校名称获取学校信息
        /// </summary>
        /// <param name="fname"></param>
        /// <returns></returns>
        public async Task<tschool> GetSchoolByNameAsync(string fname)
        {
            const string sql = "select * from tschool where fname=@fname";
            var p = new DynamicParameters();
            p.Add("@fname", fname);
            return await GetEntityAsync<tschool>(sql, p);
        }

        /// <summary>
        /// 校车公司跟服务学校关系 多对多关系
        /// </summary>
        /// <param name="companid"></param>
        /// <param name="schoolid"></param>
        /// <returns></returns>
        public async Task<tcompany_school> GetCompanySchoolRelAsync(string companid, string schoolid)
        {
            const string sql = "select * from tcompany_school where fk_company_id =@fk_company_id and fk_school_id =@fk_school_id";
            var p = new DynamicParameters();
            p.Add("@fk_company_id", companid.TrimEnd());
            p.Add("@fk_school_id", schoolid.TrimEnd());
            return await GetEntityAsync<tcompany_school>(sql, p);
        }

        /// <summary>
        /// 查询该卡片绑定其他微信用户信息(只要第一条)
        /// </summary>
        /// <param name="cardid"></param>
        /// <param name="pkid"></param>
        /// <returns></returns>
        public async Task<twxuser> GetOtherUserByCardIdAsync(string cardid, string pkid)
        {
            const string sql = "select * from twxuser where fk_card_id = @fk_card_id and pkid <> @pkid";
            var p = new DynamicParameters();
            p.Add("@pkid", pkid.TrimEnd());
            p.Add("@fk_card_id", cardid.TrimEnd());
            return await GetEntityAsync<twxuser>(sql, p);
        }

        /// <summary>
        /// 删除微信绑定用户
        /// </summary>
        /// <param name="pkid"></param>
        /// <returns></returns>
        public async Task<int> DeleteWxUserAsync(string pkid)
        {
            const string sql = "delete from twxuser where pkid=@pkid";
            var p = new DynamicParameters();
            p.Add("@pkid", pkid.TrimEnd());
            return await ExecuteEntityAsync(sql, p);
        }

        /// <summary>
        /// 获取用户卡信息
        /// </summary>
        /// <param name="wxopenid"></param>
        /// <returns></returns>
        public async Task<StudentModel> GetUserCardInfoAsync(string wxopenid)
        {
            const string sql = @"SELECT tcard.fname AS student, tcard.fk_device_id,tcard.fcode,tcard.fstatus,tschool.fname AS school,CASE WHEN tcard.ftrialdate < now() THEN  0 ELSE 1 END AS fisovertime
                                FROM twxuser INNER JOIN tcard ON twxuser.fk_card_id = tcard.pkid LEFT JOIN tschool ON tcard.fk_school_id = tschool.pkid 
                                WHERE twxuser.fwxid =@fwxid";
            var p = new DynamicParameters();
            p.Add("@fwxid", wxopenid.TrimEnd());
            return await GetEntityAsync<StudentModel>(sql, p);
        }

        /// <summary>
        /// 根据主键获取打卡信息
        /// </summary>
        /// <param name="pkid"></param>
        /// <returns></returns>
        public async Task<tcardlog> GetCardLogBypkidAsync(string pkid)
        {
            const string sql = @"select * from tcardlog where pkid=@pkid";
            var p = new DynamicParameters();
            p.Add("@pkid", pkid);
            return await GetEntityAsync<tcardlog>(sql, p);
        }

        /// <summary>
        /// 获取该卡号最后一条打卡信息
        /// </summary>
        /// <param name="fcode"></param>
        /// <returns></returns>
        public async Task<tcardlog> GetLastCardLogAsync(string fcode)
        {
            const string sql = @"SELECT * FROM tcardlog WHERE fid = @fid ORDER BY fcreatetime DESC limit 1";
            var p = new DynamicParameters();
            p.Add("@fid", fcode);
            return await GetEntityAsync<tcardlog>(sql, p);
        }

        /// <summary>
        /// 获取该设备编码当天最后一条位置信息
        /// </summary>
        /// <param name="fcode">public.tdevice 表的 fcode 设备编码</param>
        /// <returns></returns>
        public async Task<tlocatelog> GetLastLocateLogAsync(string fcode)
        {
#if DEBUG
            const string sql = @"SELECT * FROM tlocatelog WHERE fcode = @fcode  ORDER BY fcreatetime DESC limit 1";
#else
            const string sql = @"SELECT * FROM tlocatelog WHERE fcode = @fcode AND to_char( fcreatetime, 'yyyy-mm-dd' ) = to_char( now(), 'yyyy-mm-dd' ) ORDER BY fcreatetime DESC limit 1";
#endif
            var p = new DynamicParameters();
            p.Add("@fcode", fcode);
            return await GetEntityAsync<tlocatelog>(sql, p);
        }

        /// <summary>
        /// 根据卡号获取学期缴费情况
        /// </summary>
        /// <param name="fcode"></param>
        /// <returns></returns>
        public async Task<tterm> GetTermPayRecordByFCodeAsync(string fcode)
        {
            const string sql = @"SELECT ttermpay.fcode AS cardNum,tterm.* FROM ttermpay LEFT JOIN tterm ON ttermpay.fk_term_id = tterm.pkid WHERE ttermpay.fcode = @fcode ORDER BY tterm.fenddate DESC LIMIT 1";
            var p = new DynamicParameters();
            p.Add("@fcode", fcode);
            return await GetEntityAsync<tterm>(sql, p);
        }

        /// <summary>
        /// 更新卡片状态
        /// </summary>
        /// <param name="pkid"></param>
        /// <param name="status">1:微信已经注册 2:挂失 3:注销 默认:0</param>
        /// <returns></returns>
        public async Task<int> UpdateCardStatusAsync(string pkid, int status)
        {
            const string sql = "update tcard set fstatus=@fstatus where pkid=@pkid";
            var p = new DynamicParameters();
            p.Add("@fstatus", status);
            p.Add("@pkid", pkid.TrimEnd());
            return await ExecuteEntityAsync(sql, p);
        }

        /// <summary>
        /// 获取该卡号该月份每天打卡次数列表
        /// </summary>
        /// <param name="cardNum"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public async Task<List<MonthCardLogModel>> GetCardLogTimesListAsync(string cardNum, int year, int month)
        {
            const string sql = @"SELECT EXTRACT (DAY FROM tcardlog.fcreatetime ) creatday,COUNT ( EXTRACT ( DAY FROM tcardlog.fcreatetime ) ) FROM tcardlog 
                                 WHERE tcardlog.fid = @fid
                                 AND EXTRACT ( YEAR FROM tcardlog.fcreatetime ) = @year
                                 AND EXTRACT ( MONTH FROM tcardlog.fcreatetime ) = @month
                                 GROUP BY EXTRACT ( DAY FROM tcardlog.fcreatetime ) 
                                 ORDER BY creatday";
            var p = new DynamicParameters();
            p.Add("@fid", cardNum);
            p.Add("@year", year);
            p.Add("@month", month);
            var em = await GetAllEntityAsync<MonthCardLogModel>(sql, p);
            return em.ToList();
        }

        /// <summary>
        /// 获取该卡号该天打卡信息
        /// </summary>
        /// <param name="cardNum"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<List<DayCardLogModel>> GetDateCardLogAsync(string cardNum, DateTime date)
        {
            const string sql = @"SELECT pkid,fcreatetime FROM tcardlog WHERE fid = @fid AND to_char(fcreatetime, 'yyyy-MM-dd') = @date ORDER BY fcreatetime";
            var p = new DynamicParameters();
            p.Add("@fid", cardNum);
            p.Add("@date", date.ToString("yyyy-MM-dd"));
            var em = await GetAllEntityAsync<DayCardLogModel>(sql, p);
            return em.ToList();
        }

        /// <summary>
        /// 查询系统微信绑定卡记录
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<List<UserBindCard>> GetUserBindCardAsync(string sql)
        {
            var em = await GetAllEntityAsync<UserBindCard>(sql);
            return em.ToList();
        }

        /// <summary>
        /// 更新tcardlog日志
        /// </summary>
        /// <param name="tcardlogs"></param>
        /// <returns></returns>
        public async Task<int> InsertCardLogListAsync(List<tcardlog> tcardlogs)
        {
            const string sql = @"insert INTO public.tcardlog(pkid,fcode,fid,fcreatetime,flng,flat,ftype)values(@pkid,@fcode,@fid,@fcreatetime,@flng,@flat,@ftype)";
            return await ExecuteMultipleEntityAsync(sql, tcardlogs);
        }
        /// <summary>
        /// 更新tcardlog日志
        /// </summary>
        /// <param name="tcardlog"></param>
        /// <returns></returns>
        public async Task<int> InsertCardLogAsync(tcardlog tcardlog)
        {
            const string sql = @"insert INTO public.tcardlog(pkid,fcode,fid,fcreatetime,flng,flat,ftype)values(@pkid,@fcode,@fid,@fcreatetime,@flng,@flat,@ftype)";
            return await ExecuteEntityAsync(sql, tcardlog);
        }
        /// <summary>
        /// 更新tlocatelog日志
        /// </summary>
        /// <param name="tlocatelogs"></param>
        /// <returns></returns>
        public async Task<int> InsertLocatelogListAsync(List<tlocatelog> tlocatelogs)
        {
            const string sql = @"insert INTO public.tlocatelog(pkid,fcode,fcreatetime,flng,flat)values(@pkid,@fcode,@fcreatetime,@flng,@flat)";
            return await ExecuteMultipleEntityAsync(sql, tlocatelogs);
        }
        /// <summary>
        /// 更新tlocatelog日志
        /// </summary>
        /// <param name="tlocatelog"></param>
        /// <returns></returns>
        public async Task<int> InsertLocatelogAsync(tlocatelog tlocatelog)
        {
            const string sql = @"insert INTO public.tlocatelog(pkid,fcode,fcreatetime,flng,flat)values(@pkid,@fcode,@fcreatetime,@flng,@flat)";
            return await ExecuteEntityAsync(sql, tlocatelog);
        }

        /// <summary>
        /// 更新twxpushlog日志 (刷卡日志)
        /// </summary>
        /// <param name="twxpushlogs"></param>
        /// <returns></returns>
        public async Task<int> InsertWXpushlogListAsync(List<twxpushlog> twxpushlogs)
        {
            const string sql = @"insert INTO public.twxpushlog(pkid,fk_id,fwxid,fstatus,fcreatetime,fmsg)values(@pkid,@fk_id,@fwxid,@fstatus,@fcreatetime,@fmsg)";
            return await ExecuteMultipleEntityAsync(sql, twxpushlogs);
        }

        /// <summary>
        /// 更新twxpushlog日志 (刷卡日志)
        /// </summary>
        /// <param name="twxpushlog"></param>
        /// <returns></returns>
        public async Task<int> InsertWXpushlogAsync(twxpushlog twxpushlog)
        {
            const string sql = @"insert INTO public.twxpushlog(pkid,fk_id,fwxid,fstatus,fcreatetime,fmsg)values(@pkid,@fk_id,@fwxid,@fstatus,@fcreatetime,@fmsg)";
            return await ExecuteEntityAsync(sql, twxpushlog);
        }
    }
}
