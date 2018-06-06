using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;

namespace RawQueryApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //byte[] enc = QueryExecutor.ExecEncrypt("EXEC OpenKeys select dbo.Encrypt('second way you can do so is via a database transaction that essentially')  EXEC ColseKeys ");
            //if(enc != null && enc.Length > 0)
            //{
            //    var obj = QueryExecutor.ExecDecrypt(enc);
            //}
                
            Console.WriteLine("Read TodoItem from Database.");
            ReadTodoItem();

            Console.WriteLine("\n\nRead TodoDetail from Database.");
            ReadTodoDetail();

            Console.WriteLine("\n\nRead Todo View model from Database.");
            ReadTodoViewModel();

            Console.WriteLine("\n\nRead Todo View model from Database.");
            WriteTodoDetail();

            Console.ReadLine();
        }

        private static void ReadTodoItem()
        {
            var lst = new List<TodoItem>();
            string query = @"SELECT * FROM   dbo.TodoItem";
            lst = QueryExecutor.ExecSQL<TodoItem>(query);
            foreach (TodoItem item in lst)
            {
                Console.WriteLine("__________________________________________________________________");
                Console.WriteLine($"Task: {item.Task}. \nIs Done: {item.IsCompleted}.");
            }
        }

        private static void ReadTodoDetail()
        {
            var lst = new List<TodoDetail>();
            string query = @"SELECT * FROM   dbo.TodoDetail";
            lst = QueryExecutor.ExecSQL<TodoDetail>(query);
            //foreach (TodoDetail item in lst)
            //{
            //    if (!string.IsNullOrEmpty(item.Description))
            //    {
            //        Console.WriteLine("__________________________________________________________________");
            //        string replacement = Regex.Replace(item.Description, @"\t|\n|\r", "");
            //        Console.WriteLine($"\nDescription: {replacement}.");
            //    }
            //}
        }

        private static void ReadTodoViewModel()
        {
            var lst = new List<VmTask>();
            string query = @"select ti.Task, td.Description, ti.IsCompleted from TodoItem ti left join TodoDetail td on ti.Id = td.TodoId";
            lst = QueryExecutor.ExecSQL<VmTask>(query);

            //foreach (VmTask item in lst)
            //{
            //    if (!string.IsNullOrEmpty(item.Descriptions))
            //    {
            //        Console.WriteLine("__________________________________________________________________");
            //        string replacement = Regex.Replace(item.Descriptions, @"\t|\n|\r", "");
            //        Console.WriteLine($"Task: {item.Task}. \nDescription: {replacement} \nIs Done: {item.IsCompleted}.");
            //    }
            //}
        }

        private static void WriteTodoDetail()
        {
            string path = Directory.GetCurrentDirectory() + "\\InputString.txt";
            if (File.Exists(path))
            {
                string description = File.ReadAllText(path);
                int len = description.Length;
                string todoId = "E80726D7-79A8-4EC5-A872-93BF5CC2F111";
                byte[] enc = QueryExecutor.ExecEncrypt(description);
                var desc = new SqlParameter("@description", enc);
                var todoID = new SqlParameter("@TodoId", todoId);
                var id = new SqlParameter("@Id", Guid.NewGuid());

                string query = String.Format("INSERT INTO [dbo].[TodoDetail] (Id, TodoId, Description) VALUES(@Id, @TodoId, @description)", id, todoID, desc);
                int result = QueryExecutor.ExecCommand(query, new object[] { id, todoID, desc});
            }
            else
            {
                Console.WriteLine("File path not found.");
            }
        }

    }
}
