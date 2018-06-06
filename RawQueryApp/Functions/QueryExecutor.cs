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
        public static List<T> ExecSQL<T>(string query)
        {
            using (var context = new ApplicationDbContext())
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
                                try
                                {
                                    if (!object.Equals(result[prop.Name], DBNull.Value))
                                    {
                                        prop.SetValue(obj, result[prop.Name], null);
                                    }
                                }
                                catch { }
                            }
                            list.Add(obj);
                        }
                        return list;
                    }
                }
            }
        }

        public static int ExecCommand(string query, object[] param)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var result = context.Database.ExecuteSqlCommand(query, param);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }

        }

        public static byte[] ExecEncrypt(string data)
        {
            try
            {
                byte[] encrypted;
                using (var context = new ApplicationDbContext())
                {

                    using (var command = context.Database.GetDbConnection().CreateCommand())
                    {
                        command.CommandText = "EXEC OpenKeys  SELECT dbo.Encrypt(@data)   EXEC ColseKeys ";
                        command.CommandType = CommandType.Text;


                        SqlParameter sqlParam = new SqlParameter("@data", data);
                        sqlParam.DbType = DbType.String;
                        command.Parameters.Add(sqlParam);

                        context.Database.OpenConnection();
                        var obj = command.ExecuteScalar();
                        if (obj != DBNull.Value && obj != null)
                            encrypted = (byte[])obj;
                        else
                            encrypted = new byte[] { };
                    }

                    context.Database.CloseConnection();

                    return encrypted;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new byte[] { };
            }
        }


        public static string ExecDecrypt(byte[] data)
        {
            try
            {
                string result;
                using (var context = new ApplicationDbContext())
                {

                    using (var command = context.Database.GetDbConnection().CreateCommand())
                    {
                        command.CommandText = "EXEC OpenKeys  SELECT dbo.Decrypt(@data)   EXEC ColseKeys ";
                        command.CommandType = CommandType.Text;

                        SqlParameter sqlParam = new SqlParameter("@data", data);
                        sqlParam.DbType = DbType.Binary;
                        command.Parameters.Add(sqlParam);

                        context.Database.OpenConnection();
                        result = command.ExecuteScalar().ToString();
                    }

                    context.Database.CloseConnection();

                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return string.Empty;
            }
        }
    }
}
