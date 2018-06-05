using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RawQueryApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ApplicationDbContext _context = new ApplicationDbContext();
            Console.WriteLine("Read TodoItem from Database.");
            ReadTodoItem(_context);
            Console.WriteLine("\n\nRead TodoDetail from Database.");
            ReadTodoDetail(_context);
            Console.WriteLine("\n\nRead Todo View model from Database.");
            ReadTodoViewModel(_context);

            Console.ReadLine();
        }

        private static void ReadTodoItem(ApplicationDbContext _context)
        {
            var lst = new List<TodoItem>();
            string query = @"SELECT * FROM   dbo.TodoItem";
            lst = QueryExecutor.ExecSQL<TodoItem>(query, _context);
            foreach (TodoItem item in lst)
            {
                Console.WriteLine("__________________________________________________________________");
                Console.WriteLine($"Task: {item.Task}. \nIs Done: {item.IsCompleted}.");
            }
        }

        private static void ReadTodoDetail(ApplicationDbContext _context)
        {
            var lst = new List<TodoDetail>();
            string query = @"SELECT * FROM   dbo.TodoDetail";
            lst = QueryExecutor.ExecSQL<TodoDetail>(query, _context);
            foreach (TodoDetail item in lst)
            {
                if (!string.IsNullOrEmpty(item.Description))
                {
                    Console.WriteLine("__________________________________________________________________");
                    string replacement = Regex.Replace(item.Description, @"\t|\n|\r", "");
                    Console.WriteLine($"\nDescription: {replacement}.");
                }
            }
        }

        private static void ReadTodoViewModel(ApplicationDbContext _context)
        {
            var lst = new List<VmTask>();
            string query = @"select ti.Task, td.Description, ti.IsCompleted from TodoItem ti left join TodoDetail td on ti.Id = td.TodoId";
            lst = QueryExecutor.ExecSQL<VmTask>(query, _context);

            foreach (VmTask item in lst)
            {
                if (!string.IsNullOrEmpty(item.Description))
                {
                    Console.WriteLine("__________________________________________________________________");
                    string replacement = Regex.Replace(item.Description, @"\t|\n|\r", "");
                    Console.WriteLine($"Task: {item.Task}. \nDescription: {replacement} \nIs Done: {item.IsCompleted}.");
                }
            }
        }
    }
}
