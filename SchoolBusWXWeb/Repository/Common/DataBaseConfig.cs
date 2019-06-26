using Npgsql;
using SchoolBusWXWeb.Utilities;
using System.Threading.Tasks;

namespace SchoolBusWXWeb.Repository.Common
{
    /// <summary>
    /// 数据库配置
    /// </summary>
    public static class DataBaseConfig
    {
        public static async Task<NpgsqlConnection> GetNpgSqlConnectionAsync(string npgsqlConnectionString)
        {
            return await Task.Run(() =>
            {
                if (string.IsNullOrWhiteSpace(npgsqlConnectionString)) npgsqlConnectionString = AppSetting.DbConnection;
                var conn = new NpgsqlConnection(npgsqlConnectionString);
                return conn;
            });
        }
    }
}