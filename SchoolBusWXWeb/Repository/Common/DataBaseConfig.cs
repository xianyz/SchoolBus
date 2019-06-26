using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Npgsql;
using SchoolBusWXWeb.Utilities;
using StackExchange.Profiling;

namespace SchoolBusWXWeb.Repository.Common
{
    /// <summary>
    /// 数据库配置
    /// </summary>
    public static class DataBaseConfig
    {
        public static async Task<NpgsqlConnection> GetNpgSqlConnectionAsync(string npgsqlConnectionString)
        {
            if (string.IsNullOrWhiteSpace(npgsqlConnectionString)) npgsqlConnectionString = AppSetting.DbConnection;
            IDbConnection conn = new NpgsqlConnection(npgsqlConnectionString);
            if (MiniProfiler.Current != null)
            {
                conn = new StackExchange.Profiling.Data.ProfiledDbConnection((DbConnection)conn, MiniProfiler.Current);
            }
            await Task.CompletedTask;
            // await conn.OpenAsync(); //dapper 会自动管理链接开关 dapper最佳实践https://www.cnblogs.com/zhaopei/p/dapper.html
            return (NpgsqlConnection)conn;
        }
    }
}