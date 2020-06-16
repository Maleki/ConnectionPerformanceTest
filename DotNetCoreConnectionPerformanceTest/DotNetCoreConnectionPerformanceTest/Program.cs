using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace DotNetCoreConnectionPerformanceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Testing performance for estabilishing one connection vs many and old using vs new using.");
            string connectionstring = "Server=localhost;Database=YOURDATABASEHERE;User Id=sa;Password=YOURPASSWORDHERE;MultipleActiveResultSets=true;";
            var sql = "Select * from YOURSMALLTABLEHERE(NOLOCK)";
            int counter = 1000;
            var watch1 = new System.Diagnostics.Stopwatch();
            var watch2 = new System.Diagnostics.Stopwatch();
            var watch3 = new System.Diagnostics.Stopwatch();
            var watch4 = new System.Diagnostics.Stopwatch();

            watch1.Start();
            TestDapper.dbEachConnection(connectionstring, sql, counter);
            watch1.Stop();

            watch2.Start();
            TestDapper.dbOneConnection(connectionstring, sql, counter);
            watch2.Stop();

            watch3.Start();
            TestDapper.dbEachConnection2(connectionstring, sql, counter);
            watch3.Stop();

            watch4.Start();
            TestDapper.dbOneConnection2(connectionstring, sql, counter);
            watch4.Stop();

            Console.WriteLine($"Many connections new using format: {watch1.ElapsedMilliseconds}");
            Console.WriteLine($"One connection new using format: {watch2.ElapsedMilliseconds}");
            Console.WriteLine($"Many connections old using format: {watch3.ElapsedMilliseconds}");
            Console.WriteLine($"One connection old using format: {watch4.ElapsedMilliseconds}");
        }
    }
    public static class TestDapper
    {
        public static void dbEachConnection(string connectionstring, string sql, int counter)
        {
            for (int i = 0; i < counter; i++)
            {
                using IDbConnection db = new SqlConnection(connectionstring);
                db.Execute(sql);
            }
        }

        public static void dbEachConnection2(string connectionstring, string sql, int counter)
        {
            for (int i = 0; i < counter; i++)
            {
                using (IDbConnection db = new SqlConnection(connectionstring))
                {
                    db.Execute(sql);
                }
            }
        }
        public static void dbOneConnection(string connectionstring, string sql, int counter)
        {
            using IDbConnection db = new SqlConnection(connectionstring);
            for (int i = 0; i < counter; i++)
            {
                db.Execute(sql);
            }
        }
        public static void dbOneConnection2(string connectionstring, string sql, int counter)
        {
            using (IDbConnection db = new SqlConnection(connectionstring))
            {
                for (int i = 0; i < counter; i++)
                {
                    db.Execute(sql);
                }
            }
        }
    }
}
