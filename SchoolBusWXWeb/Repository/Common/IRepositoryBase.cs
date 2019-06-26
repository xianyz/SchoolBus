using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using SchoolBusWXWeb.Models;
using SchoolBusWXWeb.Utilities.PageHelper;

namespace SchoolBusWXWeb.Repository.Common
{
    /// <summary>
    /// 基类数据仓储(Dapper.Helper)
    /// </summary>
    public interface IRepositoryBase
    {
        /// <summary>
        /// 返回第一行第一列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="pms"></param>
        /// <returns></returns>
        Task<T> GetExecuteScalarAsync<T>(string sql, DynamicParameters pms = null);

        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="sql">自定义拼接sql</param>
        /// <param name="pms">动态参数</param>
        /// <returns>返回实体</returns>
        Task<T> GetEntityAsync<T>(string sql, DynamicParameters pms = null) where T : class, new();

        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <param name="sql">自定义拼接sql</param>
        /// <param name="pms">动态参数</param>
        /// <returns>返回实体集合</returns>
        Task<IEnumerable<T>> GetAllEntityAsync<T>(string sql, DynamicParameters pms = null) where T : class, new();

        /// <summary>
        /// 根据多条语句返回多个结果集
        /// </summary>
        /// <typeparam name="T">返回实体</typeparam>
        /// <param name="sql">自定义拼接sql多个sql用;隔开</param>
        /// <param name="pms">动态参数</param>
        /// <param name="isnull">是否去除空实体(某一条语句没有查到数据)</param>
        /// <returns></returns>
        Task<List<T>> GetMultipleEntityAsync<T>(string sql, DynamicParameters pms = null, bool isnull = false) where T : class, new();

        /// <summary>
        /// 获取dynamic(动态)类型的集合
        /// </summary>
        /// <param name="sql">自定义拼接sql</param>
        /// <param name="pms">动态参数</param>
        /// <returns></returns>
        Task<IEnumerable<dynamic>> GetDynamicAsync(string sql, DynamicParameters pms = null);

        /// <summary>
        /// 增删改,实体方法
        /// </summary>
        /// <param name="sql">自定义拼接sql</param>
        /// <param name="t">实体</param>
        /// <returns>返回受影响的行数</returns>
        Task<int> ExecuteEntityAsync<T>(string sql, T t) where T : class, new();

        /// <summary>
        /// 增删改,参数方法
        /// </summary>
        /// <param name="sql">自定义拼接sql</param>
        /// <param name="pms">动态参数</param>
        /// <returns>返回受影响的行数</returns>
        Task<int> ExecuteEntityAsync(string sql, DynamicParameters pms = null);

        /// <summary>
        /// 批量增删改
        /// </summary>
        /// <param name="sql">自定义拼接sql</param>
        /// <param name="t">实体集合</param>
        /// <returns>true成功,false失败</returns>
        Task<int> ExecuteMultipleEntityAsync<T>(string sql, IEnumerable<T> t) where T : class, new();
        Task<int> ExecuteMultipleEntityAsync(string sql, object obj);
        /// <summary>
        /// 新增实体返回对应主键Id
        /// </summary>
        /// <param name="sql">自定义拼接sql</param>
        /// <param name="t">实体</param>
        /// <returns>实体主键</returns>
        Task<int> GetAddEntityIdAsync<T>(string sql, T t) where T : class, new();

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="criteria">表名等集合</param>
        /// <returns>分页数据</returns>
        Task<PageDataView<T>> GetPageDataAsync<T>(PageCriteria criteria) where T : class, new();

        Task<int> ChangeOrderNum(ChangeOrder t);
    }
}