using System.Text;

namespace LComplete.Framework.Data
{
    public static class SqlBuildHelper
    {
        public static string BuildPageSql<T>(string withSql, OrderPagingQuery<T> query,string tableName="TResult") where T : class
        {
            var sqlCommand = new StringBuilder();
            sqlCommand.Append(withSql);
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("*");
            sqlCommand.Append(" FROM (SELECT ROW_NUMBER() OVER(");

            StringBuilder orderFileds= BuildOrderField(query.OrderFieldStore);
            sqlCommand.Append(orderFileds);

            sqlCommand.Append(") AS ROW_NUMBER,");
            sqlCommand.Append("*");
            sqlCommand.Append(" FROM ");
            sqlCommand.Append(tableName);
            sqlCommand.Append(") AS T0 WHERE ROW_NUMBER BETWEEN ");
            sqlCommand.Append((((query.Page - 1) * query.PageSize) + 1).ToString());
            sqlCommand.Append(" AND ");
            sqlCommand.Append((query.Page * query.PageSize).ToString());

            return sqlCommand.ToString();
        }

        public static StringBuilder BuildOrderField<T>(OrderFieldStore<T> orderFieldStore) where T : class
        {
            StringBuilder sqlCommand=new StringBuilder();
            bool ordered = false;
            foreach (OrderField<T> orderField in orderFieldStore.OrderFields)
            {
                string orderType = string.Empty;
                if (orderField.OrderType == OrderType.Descending)
                {
                    orderType = "DESC";
                }
                else if (orderField.OrderType == OrderType.Ascending)
                {
                    orderType = "ASC";
                }

                if (!string.IsNullOrEmpty(orderType))
                {
                    sqlCommand.Append(ordered ? "," : "ORDER BY ");

                    string sortField = orderField.OrderKey;
                    sqlCommand.Append(sortField + " " + orderType);

                    ordered = true;
                }
            }
            return sqlCommand;
        }

        public static string BuildCountSql(string withSql,string tableName="TResult")
        {
            string countSql = "SELECT COUNT(1) FROM " + tableName;
            return withSql + countSql;
        }
    }
}
