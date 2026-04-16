using System;
using CiccioSoft.Data.MariaDbEmbedded.Interop.IA;

namespace CiccioSoft.Data.MariaDbEmbedded.Interop.Example;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        MySql mysql = MySql.Init();
        mysql.SetOption(MySqlOption.ConnectTimeout, 5u);
        mysql.Open("localhost", 3306, "root", "password", "testdb");
    }
}
