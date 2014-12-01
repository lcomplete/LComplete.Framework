using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LComplete.Framework.Dapper;

namespace LComplete.Framework.Dapper
{
    /// <summary>
    /// 数据访问工具类（所有的操作，必须使用参数化语句）
    /// usage: https://github.com/StackExchange/dapper-dot-net
    /// </summary>
    public static class DataUtils
    {
        /// <summary>
        /// 执行参数化SQL
        /// </summary>
        /// <returns>受影响的行数</returns>
        public static int Execute(string connectionString, string sql, object param = null,
            CommandType commandType = CommandType.Text)
        {
            using (IDbConnection cnn = new SqlConnection(connectionString))
            {
                return SqlMapper.Execute(cnn, sql, param, null, null, commandType);
            }
        }

        /// <summary>
        /// 执行查询, 返回指定的T数据类型
        /// </summary>
        public static IList<T> Query<T>(string connectionString, string sql, object param = null,
            CommandType commandType = CommandType.Text)
        {
            using (IDbConnection cnn = new SqlConnection(connectionString))
            {
                return SqlMapper.Query<T>(cnn, sql, param, null, true, null, commandType).ToList();
            }
        }

        /// <summary>
        /// 执行查询返回多结果集, 可依次访问 (注意，需要关闭GridReader)
        /// </summary>
        public static SqlMapper.GridReader QueryMultiple(string connectionString, string sql, object param = null,
            CommandType commandType = CommandType.Text)
        {
            IDbConnection cnn = new SqlConnection(connectionString);
            return SqlMapper.QueryMultiple(cnn, sql, param, null, null, commandType);
        }

        /// <summary>
        /// 执行查询，返回一个动态对象列表
        /// </summary>
        public static IList<dynamic> Query(string connectionString, string sql, object param = null,
            CommandType? commandType = CommandType.Text)
        {
            using (IDbConnection cnn = new SqlConnection(connectionString))
            {
                return SqlMapper.Query(cnn, sql, param, null, true, null, commandType).ToList();
            }
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="entity"></param>
        public static void Update<T>(string connectionString, T entity) where T : class
        {
            using (IDbConnection cnn = new SqlConnection(connectionString))
            {
                cnn.Open();
                cnn.Update(entity);
                cnn.Close();
            }
        }

        /// <summary>
        /// 插入实体
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="entity"></param>
        public static void Insert<T>(string connectionString, T entity) where T:class 
        {
            using (IDbConnection cnn = new SqlConnection(connectionString))
            {
                cnn.Open();
                cnn.Insert(entity);
                cnn.Close();
            }
        }

    }
}
