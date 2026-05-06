// Copyright (c) 2026 Francesco Crimi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Buffers;
using System.Runtime.InteropServices;
using System.Text;
using CiccioSoft.Data.MariaDbEmbedded.Interop.Native;

namespace CiccioSoft.Data.MariaDbEmbedded.Interop.Example;

class Program
{

    /// <summary>
    /// Entry point
    /// </summary>
    static void Main(string[] args)
    {
        using MySql mysql = ApriConnessione();

        EseguiDdl(mysql);
        var idMozzarella = EseguiInsertConEscape(mysql);
        EseguiSelect(mysql);
        EseguiUpdate(mysql);
        EseguiDelete(mysql, idMozzarella);
        EseguiPreparedInsert(mysql);
        EseguiPreparedSelect(mysql);
        EseguiTransazione(mysql);
        EseguiMultiStatement(mysql);
        EseguiStreamingSelect(mysql);
        EseguiPing(mysql);
        Cleanup(mysql);
    }

    /// <summary>
    /// 1. Connessione
    /// </summary>
    private static MySql ApriConnessione()
    {
        MySql mysql = MySql.Init();
        mysql.SetOption(MySqlOption.MYSQL_OPT_SSL_VERIFY_SERVER_CERT, false);
        mysql.SetOption(MySqlOption.MARIADB_OPT_MULTI_STATEMENTS, true);
        mysql.Connect("localhost", 3306, "root", "password", "test");

        Console.WriteLine("=== Connessione aperta ===");
        Console.WriteLine($"  Server  : {mysql.GetServerInfo()}");
        Console.WriteLine($"  Client  : {MySql.GetClientInfo()}");
        Console.WriteLine($"  Host    : {mysql.GetHostInfo()}");
        Console.WriteLine($"  Thread  : {mysql.ThreadId()}");
        Console.WriteLine($"  Proto   : {mysql.GetProtoInfo()}");

        return mysql;
    }

    /// <summary>
    /// 2. DDL – CREATE TABLE
    /// </summary>
    private static void EseguiDdl(MySql mysql)
    {
        Console.WriteLine("\n=== DDL ===");

        mysql.Query("""
            CREATE TABLE IF NOT EXISTS prodotti (
                id       INT AUTO_INCREMENT PRIMARY KEY,
                nome     VARCHAR(100) NOT NULL,
                prezzo   DECIMAL(10,2) NOT NULL,
                attivo   TINYINT(1) NOT NULL DEFAULT 1,
                creato   DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4
            """);

        Console.WriteLine("  Tabella 'prodotti' creata (o già esistente).");
    }

    /// <summary>
    /// 3. INSERT con mysql_query + escape manuale
    /// </summary>
    private static ulong EseguiInsertConEscape(MySql mysql)
    {
        Console.WriteLine("\n=== INSERT con escape ===");

        // mysql_real_escape_string gestisce apici, backslash e caratteri binari
        string nomeEsc = mysql.RealEscapeString("Mozzarella \"D.O.P.\"");
        mysql.Query($"INSERT INTO prodotti (nome, prezzo) VALUES ('{nomeEsc}', 4.50)");

        ulong id = mysql.InsertId();
        ulong affected = mysql.AffectedRows();
        Console.WriteLine($"  Inserito id={id}, affected={affected}");
        return id;
    }

    /// <summary>
    /// 4. SELECT con mysql_store_result
    /// </summary>
    private static void EseguiSelect(MySql mysql)
    {
        Console.WriteLine("\n=== SELECT ===");

        mysql.Query("SELECT id, nome, prezzo, attivo, creato FROM prodotti");

        using MySqlResult? result = mysql.StoreResult()
            ?? throw new InvalidOperationException("Nessun result set.");

        Console.WriteLine($"  Righe={result.NumRows}  Colonne={result.NumFields}");

        // Metadati: mysql_fetch_fields non avanza il cursore di riga
        Console.WriteLine("  Campi:");
        foreach (MySqlField f in result.FetchFields())
            Console.WriteLine($"    {f.Name,-20} tipo={f.Type,-12} PK={f.IsPrimaryKey}  NN={f.IsNotNull}");

        // Metadati: mysql_fetch_field con indice diretto
        var fields = result.FetchField(0);
        Console.WriteLine($"  Campo #0: {fields.Name}  tipo={fields.Type}  PK={fields.IsPrimaryKey}  NN={fields.IsNotNull}");

        // Iterazione righe tramite IEnumerable<MySqlRow>
        Console.WriteLine("  Righe:");
        // foreach (MySqlRow row in result)
        while (result?.FetchRow(out var row) == true)
        {
            Console.WriteLine(
                $"    id={row.GetInt32("id"),-4} " +
                $"nome={row.GetString("nome"),-30} " +
                $"prezzo={row.GetDecimal("prezzo"):F2}  " +
                $"attivo={row.GetBool("attivo")}  " +
                $"creato={row.GetDateTime("creato")}");
        }
    }

    /// <summary>
    /// 5. UPDATE
    /// </summary>
    private static void EseguiUpdate(MySql mysql)
    {
        Console.WriteLine("\n=== UPDATE ===");

        mysql.Query("UPDATE prodotti SET prezzo = prezzo * 1.10 WHERE attivo = 1");

        Console.WriteLine($"  Righe aggiornate : {mysql.AffectedRows()}");
        Console.WriteLine($"  Warning          : {mysql.WarningCount()}");
        Console.WriteLine($"  Info server      : {mysql.Info()}");
    }

    /// <summary>
    /// 6. DELETE
    /// </summary>
    private static void EseguiDelete(MySql mysql, ulong id)
    {
        Console.WriteLine("\n=== DELETE ===");

        mysql.Query($"DELETE FROM prodotti WHERE id = {id}");
        Console.WriteLine($"  Righe eliminate: {mysql.AffectedRows()}");
    }

    /// <summary>
    /// 7. Prepared statement – INSERT parametrizzato
    /// </summary>
    private static void EseguiPreparedInsert(MySql db)
    {
        Console.WriteLine("\n=== Prepared INSERT ===");

        using MySqlStmt stmt = db.StmtInit();
        stmt.Prepare("INSERT INTO prodotti (nome, prezzo, attivo) VALUES (?, ?, ?)");
        Console.WriteLine($"  Parametri attesi: {stmt.ParamCount()}");

        // Primo inserimento
        using (var bind = new MySqlBindBuilder(3))
        {
            bind[0].SetString("Parmigiano Reggiano 24 mesi");
            bind[1].SetDecimal(18.90m);
            bind[2].SetInt32(1);

            stmt.BindParam(bind);
            stmt.Execute();
            Console.WriteLine($"  Inserito id={stmt.InsertId()}");
        }

        // Secondo inserimento – reset e riuso dello stesso statement
        stmt.Reset();
        using (var bind = new MySqlBindBuilder(3))
        {
            bind[0].SetString("Pecorino Romano");
            bind[1].SetDecimal(12.50m);
            bind[2].SetInt32(1);

            stmt.BindParam(bind);
            stmt.Execute();
            Console.WriteLine($"  Inserito id={stmt.InsertId()}");
        }
    }

    /// <summary>
    /// 8. Prepared statement – SELECT con bind_result
    /// </summary>
    /// <remarks> 
    /// I buffer di output devono restare pinned per tutta la durata
    /// del loop di fetch: si usa GCHandle.Alloc(..., Pinned) invece
    /// di fixed{} che scade al termine del suo blocco lessicale.
    /// </remarks>
    private static void EseguiPreparedSelect(MySql db)
    {
        Console.WriteLine("\n=== Prepared SELECT ===");

        using MySqlStmt stmt = db.StmtInit();
        stmt.Prepare(
            "SELECT id, nome, prezzo FROM prodotti WHERE attivo = ?");

        // Parametro di input
        using var paramBuilder = new MySqlBindBuilder(1);
        paramBuilder[0].SetInt32(1);
        stmt.BindParam(paramBuilder);

        using var resultBuilder = new MySqlBindBuilder(3);
        resultBuilder[0].SetInt32(0);       // id
        resultBuilder[1].SetString(100);    // nome
        resultBuilder[2].SetDouble(0.0);    // prezzo
        stmt.BindResult(resultBuilder);

        stmt.Execute();
        stmt.StoreResult();
        Console.WriteLine($"  Righe nel result: {stmt.NumRows()}");

        // Il driver popola i buffer ad ogni mysql_stmt_fetch()
        while (stmt.Fetch())
        {
            int id = resultBuilder[0].GetInt32();
            string nome = resultBuilder[1].GetString() ?? "NULL";
            double prz = resultBuilder[2].GetDouble();
            Console.WriteLine($"    id={id,-4} nome={nome,-30} prezzo={prz:F2}");
        }

        stmt.FreeResult();
    }

    /// <summary>
    /// 9. Transazione – autocommit / commit / rollback
    /// </summary>
    private static void EseguiTransazione(MySql mysql)
    {
        Console.WriteLine("\n=== Transazione ===");

        mysql.AutoCommit(false);
        try
        {
            mysql.Query("UPDATE prodotti SET prezzo = 99.99 WHERE id = 1");
            mysql.Query("UPDATE prodotti SET prezzo = 88.88 WHERE id = 2");
            mysql.Commit();
            Console.WriteLine("  Commit eseguito.");
        }
        catch
        {
            mysql.Rollback();
            Console.WriteLine("  Rollback eseguito.");
            throw;
        }
        finally
        {
            mysql.AutoCommit(true);
        }
    }

    /// <summary>
    /// 10. Multi-statement (richiede ClientFlags.MultiStatements)
    /// </summary>
    private static void EseguiMultiStatement(MySql mysql)
    {
        Console.WriteLine("\n=== Multi-statement ===");

        mysql.Query("""
            SELECT COUNT(*) AS totale FROM prodotti;
            SELECT SUM(prezzo) AS totale_prezzi FROM prodotti WHERE attivo = 1
            """);

        int resultIndex = 0;
        do
        {
            // using MySqlResult? res = mysql.StoreResult();
            // if (res is not null)
            // {
            //     MySqlRow? r = res.FetchRow();
            //     if (r is not null)
            //         Console.WriteLine($"  Result #{resultIndex}: {r}");
            // }

            using (MySqlResult? res = mysql.StoreResult())
            {
                while (res?.FetchRow(out var r) == true)
                    Console.WriteLine($"  Result #{resultIndex}: {r.ToString()}");
            }

            resultIndex++;
        }
        while (mysql.NextResult());
    }


    /// <summary>
    /// 11. USE RESULT – lettura streaming riga per riga
    /// </summary>
    private static void EseguiStreamingSelect(MySql mysql)
    {
        Console.WriteLine("\n=== Streaming SELECT (mysql_use_result) ===");

        mysql.Query("SELECT id, nome FROM prodotti ORDER BY id");

        using MySqlResult? res = mysql.UseResult();
        if (res is null) return;

        while (res.FetchRow(out MySqlRow row) == true)
            Console.WriteLine($"  id={row.GetInt32("id")}  nome={row.GetString("nome")}");
    }

    /// <summary>
    /// 12. Ping
    /// </summary>
    private static void EseguiPing(MySql mysql)
    {
        Console.WriteLine("\n=== Ping ===");
        mysql.Ping();
        Console.WriteLine("  mysql_ping → OK");
        Console.WriteLine($"  Stato server: {mysql.Stat()}");
    }

    /// <summary>
    /// 13. Cleanup – DROP TABLE
    /// </summary>
    private static void Cleanup(MySql mysql)
    {
        Console.WriteLine("\n=== Cleanup ===");
        mysql.Query("DROP TABLE IF EXISTS prodotti");
        Console.WriteLine("  Tabella 'prodotti' eliminata.");
        Console.WriteLine("\nFine esempi.");
    }
}
