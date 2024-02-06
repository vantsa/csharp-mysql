using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace C_AndMySQL
{
    internal static class GenericDAL<T>
    {
        #region Fields
        private static readonly string connectionString = Constants.connectionString;
        #endregion

        public static void Insert(T entity)
        {
            using(MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        #region Attributes
                        DbTable dbTableAttribute = (DbTable)Attribute.GetCustomAttribute(typeof(T), typeof(DbTable));
                        string tableName = dbTableAttribute?.TableName ?? typeof(T).Name;

                        PropertyInfo[] properties = typeof(T).GetProperties();
                        string fieldNames = string.Join(", ", properties.Select(prop =>
                        {
                            DbField dbFieldAttribute = (DbField)Attribute.GetCustomAttribute(prop, typeof(DbField));
                            return dbFieldAttribute?.FieldName ?? prop.Name;
                        }));
                        #endregion

                        string query = $"INSERT INTO {tableName} ({fieldNames}) VALUES ({string.Join(", ", properties.Select(prop => $"@{prop.Name}"))})";

                        using(MySqlCommand command = new MySqlCommand(query, connection, transaction))
                        {
                            foreach(var prop in properties)
                            {
                                command.Parameters.AddWithValue($"@{prop.Name}", prop.GetValue(entity)); 
                            }

                            command.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"Error: {e.Message}");
                    }
                }
            }
        }

        public static void Update(T entity)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        #region Attributes
                        DbTable dbTableAttribute = (DbTable)Attribute.GetCustomAttribute(typeof(T), typeof(DbTable));
                        string tableName = dbTableAttribute?.TableName ?? typeof(T).Name;

                        PropertyInfo[] properties = typeof(T).GetProperties();
                        string setClause = string.Join(", ", properties
                            .Where(prop => !IsPrimaryKey(prop))
                            .Select(prop =>
                            {
                                DbField dbFieldAttribute = (DbField)Attribute.GetCustomAttribute(prop, typeof(DbField));
                                return $"{(dbFieldAttribute?.FieldName ?? prop.Name)} = @{prop.Name}";
                            }));
                        #endregion

                        string query = $"UPDATE {tableName} SET {setClause} WHERE {GetPrimaryKeyCondition()}";

                        using (MySqlCommand command = new MySqlCommand(query, connection, transaction))
                        {
                            foreach (var prop in properties)
                            {
                                command.Parameters.AddWithValue($"@{prop.Name}", prop.GetValue(entity));
                            }

                            command.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"Error: {e.Message}");
                    }
                }
            }
        }

        public static void Delete(T entity)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        #region Attributes
                        DbTable dbTableAttribute = (DbTable)Attribute.GetCustomAttribute(typeof(T), typeof(DbTable));
                        string tableName = dbTableAttribute?.TableName ?? typeof(T).Name;

                        PropertyInfo primaryKeyProperty = GetPrimaryKeyProperty();
                        object primaryKeyValue = primaryKeyProperty.GetValue(entity);
                        #endregion

                        // Build DELETE query
                        string query = $"DELETE FROM {tableName} WHERE {GetPrimaryKeyCondition()}";

                        using (MySqlCommand command = new MySqlCommand(query, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@ID", primaryKeyValue);
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"Error in Delete: {e.Message}");
                    }
                }
            }
        }

        public static List<T> GetCollection()
        {
            List<T> entityList = new List<T>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        #region Attributes
                        DbTable dbTableAttribute = (DbTable)Attribute.GetCustomAttribute(typeof(T), typeof(DbTable));
                        string tableName = dbTableAttribute?.TableName ?? typeof(T).Name;
                        #endregion

                        string query = $"SELECT * FROM {tableName}";

                        using (MySqlCommand command = new MySqlCommand(query, connection, transaction))
                        {
                            using (MySqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    T entity = Activator.CreateInstance<T>();
                                    foreach (PropertyInfo property in typeof(T).GetProperties())
                                    {
                                        if (!reader.IsDBNull(reader.GetOrdinal(property.Name)))
                                        {
                                            property.SetValue(entity, reader[property.Name]);
                                        }
                                    }
                                    entityList.Add(entity);
                                }
                            }
                        }
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"Error in GetCollection: {e.Message}");
                    }
                }
            }
            return entityList;
        }

        private static bool IsPrimaryKey(PropertyInfo property)
        {
            DbField dbFieldAttribute = (DbField)Attribute.GetCustomAttribute(property, typeof(DbField));
            return dbFieldAttribute?.FieldName.Equals("ID", StringComparison.OrdinalIgnoreCase) ?? false;
        }

        private static string GetPrimaryKeyCondition()
        {
            PropertyInfo idProperty = typeof(T).GetProperties().FirstOrDefault(IsPrimaryKey);
            DbField dbFieldAttribute = (DbField)Attribute.GetCustomAttribute(idProperty, typeof(DbField));

            return $"{dbFieldAttribute?.FieldName} = @{idProperty?.Name}";
        }

        private static PropertyInfo GetPrimaryKeyProperty()
        {
            PropertyInfo primaryKeyProperty = typeof(T)
                .GetProperties()
                .FirstOrDefault(IsPrimaryKey);

            if (primaryKeyProperty == null)
            {
                throw new InvalidOperationException($"Primary key property not found in {typeof(T).Name}");
            }

            return primaryKeyProperty;
        }
    }
}
