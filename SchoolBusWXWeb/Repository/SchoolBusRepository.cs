using Microsoft.Extensions.Options;
using SchoolBusWXWeb.Models;
using SchoolBusWXWeb.Models.SchollBusModels;
using SchoolBusWXWeb.Repository.Common;
using System;
using System.Threading.Tasks;
using Dapper;

namespace SchoolBusWXWeb.Repository
{
    public class SchoolBusRepository : RepositoryBase, ISchoolBusRepository
    {
        public SchoolBusRepository(IOptions<SiteConfig> settings) : base(settings.Value.DefaultConnection) { }
        public async Task<twxuser> GetTwxuserAsync()
        {
            const string sql = "select * from public.twxuser where pkid=@pkid";
            var p=new DynamicParameters();
            p.Add("@pkid", "2c9ab45969dc19990169dd5bb9ea08b5");
            var entity= await GetEntityAsync<twxuser>(sql, p);
            return entity;
        }
    }
}
