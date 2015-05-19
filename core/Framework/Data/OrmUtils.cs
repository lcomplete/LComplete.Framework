using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LComplete.Framework.Common;

namespace LComplete.Framework.Data
{
    public sealed class OrmUtils
    {
        public static TEntity ReadToEntity<TEntity>(DataTable table) where TEntity : class
        {
            return ReadToEntityList<TEntity>(table).FirstOrDefault();
        }

        public static IList<TEntity> ReadToEntityList<TEntity>(DataTable table) where TEntity : class
        {
            IList<TEntity> list = new List<TEntity>();
            Type entityType = typeof(TEntity);
            TEntity entity;
            if (table.Rows.Count > 0)
            {
                for (int row = 0; row < table.Rows.Count; row++)
                {
                    entity = (TEntity)Activator.CreateInstance(entityType);
                    for (int columnIndex = 0; columnIndex < table.Columns.Count; columnIndex++)
                    {
                        object value = table.Rows[row][columnIndex];
                        string columnName = table.Columns[columnIndex].ColumnName;
                        if (!Convert.IsDBNull(value) && entityType.GetProperty(columnName) != null)
                        {
                            ReflectUtils.SetPropertyValue(entity, columnName, value);
                        }
                    }
                    list.Add(entity);
                }
            }
            return list;
        }

        public static TEntity ReadToEntity<TEntity>(IDataReader reader) where TEntity : class
        {
            Type entityType = typeof(TEntity);
            if (reader.Read())
            {
                TEntity entity = (TEntity)Activator.CreateInstance(entityType);
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string columnName = reader.GetName(i);
                    if (!reader.IsDBNull(i) && entityType.GetProperty(columnName) != null)
                    {
                        ReflectUtils.SetPropertyValue(entity, columnName, reader.GetValue(i));
                    }
                }
                return entity;
            }
            return null;
        }

        public static IList<TEntity> ReadToEntityList<TEntity>(IDataReader reader) where TEntity : class
        {
            IList<TEntity> list = new List<TEntity>();
            TEntity entity;
            while ((entity = ReadToEntity<TEntity>(reader)) != null)
            {
                list.Add(entity);
            }
            return list;
        }
    }
}