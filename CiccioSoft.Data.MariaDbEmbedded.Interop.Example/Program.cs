// Copyright (c) 2026 Francesco Crimi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using CiccioSoft.Data.MariaDbEmbedded.Interop.Native;

namespace CiccioSoft.Data.MariaDbEmbedded.Interop.Example;

class Program
{
    static void Main(string[] args)
    {
        //Test1();
        Test2();
    }

    static void Test1( )
    {
        MySql mysql = MySql.Init();
        mysql.SetOption(MySqlOption.MYSQL_OPT_SSL_VERIFY_SERVER_CERT, false);
        mysql.Connect("localhost", 3306, "root", "password", "test");

        Console.WriteLine($"MariaDB client version: {mysql.GetClientInfo()}");
        Console.WriteLine($"MariaDB server version: {mysql.GetServerInfo()}");

        mysql.Query("CREATE TABLE IF NOT EXISTS test_table(id INT PRIMARY KEY AUTO_INCREMENT, nome VARCHAR(255))");
        mysql.Query("INSERT INTO test_table(nome) VALUES('MSYS2 User')");

        mysql.Query("SELECT * FROM test_table");
        MySqlResult? result = mysql.StoreResult();

        Console.WriteLine("Dati nella tabella MariaDB:\n");
        while ((result?.FetchRow()) is MySqlRow row)
        {
            for (int i = 0; i < result?.FieldCount; i++)
            {
                Console.WriteLine("{0} ", row.IsNull(i) ? "" : row.GetString(i));
            }
            Console.WriteLine();
        }

        result?.Dispose();
        mysql.Dispose();
    }

    static void Test2( )
    {
        MySql mysql = MySql.Init();
        mysql.SetOption(MySqlOption.MYSQL_OPT_SSL_VERIFY_SERVER_CERT, false);
        mysql.Connect("localhost", 3306, "root", "password", "Northwind");

        Console.WriteLine($"MariaDB client version: {mysql.GetClientInfo()}");
        Console.WriteLine($"MariaDB server version: {mysql.GetServerInfo()}");

        mysql.Query("SELECT ProductID, ProductName, Price FROM Products WHERE ProductID = 29");

        using var result = mysql.StoreResult()
            ?? throw new InvalidOperationException("Nessun result set.");

        // metadati
        foreach (var field in result.FetchFields())
            Console.WriteLine($"{field.Name,-20} {field.Type,-20} {(field.IsNotNull ? "NOT NULL" : "")}");

        // righe
        result.ForEachRow(row =>
        {
            int? id = row.GetInt32(0);
            string? nome = row.GetString(1);
            decimal? prezzo = row.GetDecimal(2);
            // DateTime? data = row.GetDateTime(3);
            // Console.WriteLine($"{id,5} {nome,-30} {prezzo,10:C} {data:d}");
            Console.WriteLine($"{id,5} {nome,-30} {prezzo,10:C}");
        });
        mysql.Dispose();
    }
}
