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
            p.Add("@pkid", pkid);
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
            p.Add("@fwxid", openid);
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
            p.Add("@pkid", pkid);
            return await GetEntityAsync<tcard>(sql, p);
        }

        /// <summary>
        /// 根据卡号返回信息
        /// </summary>
        /// <param name="fcode"></param>
        /// <returns></returns>
        public async Task<tcard> GetCardByCode(string fcode)
        {
            const string sql = "SELECT tcard.fcode, tcard.fstatus,tcard.fname, tdevice.fplatenumber,tschool.fname AS fschoolname FROM tcard LEFT JOIN tdevice ON tcard.fk_device_id = tdevice.pkid LEFT JOIN tschool ON tcard.fk_school_id = tschool.pkid WHERE tcard.fcode = @fcode";
            var p = new DynamicParameters();
            p.Add("@fcode", fcode);
            return await GetEntityAsync<tcard>(sql, p);
        }
    }
}
