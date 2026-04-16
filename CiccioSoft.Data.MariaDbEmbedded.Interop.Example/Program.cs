// Copyright (c) 2026 Francesco Crimi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using CiccioSoft.Data.MariaDbEmbedded.Interop.IA;

namespace CiccioSoft.Data.MariaDbEmbedded.Interop.Example;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        MySql mysql = MySql.Init();
        mysql.SetOption(MySqlOption.MYSQL_OPT_SSL_VERIFY_SERVER_CERT, false);
        mysql.Open("localhost", 3306, "root", "password", "testdb");
        // Console.WriteLine($"MariaDB client version: {mysql.GetClientInfo()}");
        // Console.WriteLine($"MariaDB server version: {mysql.GetServerInfo()}");
        mysql.Dispose();
    }
}
