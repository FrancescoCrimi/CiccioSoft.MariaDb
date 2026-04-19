// Copyright (c) 2026 Francesco Crimi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using CiccioSoft.Data.MariaDbEmbedded.Interop;
using CiccioSoft.Data.MariaDbEmbedded.Interop.Native;

namespace CiccioSoft.Data.MariaDbEmbedded.Interop.Example;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        MySql mysql = MySql.Init();
        mysql.SetOption(MySqlOption.MYSQL_OPT_SSL_VERIFY_SERVER_CERT, false);
        mysql.Connect("localhost", 3306, "root", "password", "test");

        Console.WriteLine($"MariaDB client version: {mysql.GetClientInfo()}");
        Console.WriteLine($"MariaDB server version: {mysql.GetServerInfo()}");

        mysql.Query("CREATE TABLE IF NOT EXISTS test_table(id INT PRIMARY KEY AUTO_INCREMENT, nome VARCHAR(255))");
        mysql.Query("INSERT INTO test_table(nome) VALUES('MSYS2 User')");
        mysql.Query("SELECT * FROM test_table");

            MySqlResult result = mysql.mysql_store_result();

        mysql.Dispose();
    }
}
