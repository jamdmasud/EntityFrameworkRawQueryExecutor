using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace RawQueryApp 
{
    public class QueryExecutor
    {
        public static List<T> ExecSQL<T>(string query, ApplicationDbContext context)
        {
            
                using (var command = context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;
                    context.Database.OpenConnection();

                    using (var result = command.ExecuteReader())
                    {
                        List<T> list = new List<T>();
                        T obj = default(T);
                        while (result.Read())
                        {
                            obj = Activator.CreateInstance<T>();
                            foreach (PropertyInfo prop in obj.GetType().GetProperties())
                            {
                                if (!object.Equals(result[prop.Name], DBNull.Value))
                                {
                                    prop.SetValue(obj, result[prop.Name], null);
                                }
                            }
                            list.Add(obj);
                        }
                        return list;

                    }
                } 
        }
    }
}
