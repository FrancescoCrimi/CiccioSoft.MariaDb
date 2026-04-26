// Copyright (c) 2026 Francesco Crimi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Runtime.InteropServices;
using System.Text;
using CiccioSoft.Data.MariaDbEmbedded.Interop.Native;

namespace CiccioSoft.Data.MariaDbEmbedded.Interop.Example;

class Program
{
    // ----------------------------------------------------------------
    //  Entry point
    // ----------------------------------------------------------------
    static void Main(string[] args)
    {
        using MySql mysql = ApriConnessione();
        EseguiDdl(mysql);
        EseguiInsertConEscape(mysql);
        EseguiSelect(mysql);
        EseguiUpdate(mysql);
        EseguiDelete(mysql, 1);
        // EseguiPreparedInsert(mysql);
        // EseguiPreparedSelect(mysql);
        // EseguiTransazione(mysql);
        // EseguiMultiStatement(mysql);
        // EseguiStreamingSelect(mysql);
        // EseguiPing(mysql);
        // Cleanup(mysql);
    }

    // ================================================================
    // 1. Connessione
    // ================================================================
    private static MySql ApriConnessione()
    {
        MySql mysql = MySql.Init();
        mysql.SetOption(MySqlOption.MYSQL_OPT_SSL_VERIFY_SERVER_CERT, false);
        mysql.Connect("localhost", 3306, "root", "password", "test");

        Console.WriteLine("=== Connessione aperta ===");
        Console.WriteLine($"  Server  : {mysql.GetServerInfo()}");
        Console.WriteLine($"  Client  : {MySql.GetClientInfo()}");
        Console.WriteLine($"  Host    : {mysql.GetHostInfo()}");
        Console.WriteLine($"  Thread  : {mysql.ThreadId()}");
        Console.WriteLine($"  Proto   : {mysql.GetProtoInfo()}");

        return mysql;
    }


    // ================================================================
    // 2. DDL – CREATE TABLE
    // ================================================================
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

    // ================================================================
    // 3. INSERT con mysql_query + escape manuale
    // ================================================================
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

    // ================================================================
    // 4. SELECT con mysql_store_result
    // ================================================================
    private static void EseguiSelect(MySql mysql)
    {
        Console.WriteLine("\n=== SELECT ===");

        mysql.Query("SELECT id, nome, prezzo, attivo, creato FROM prodotti");

        using MySqlResult? result = mysql.StoreResult()
            ?? throw new InvalidOperationException("Nessun result set.");

        Console.WriteLine($"  Righe={result.RowCount}  Colonne={result.FieldCount}");

        // Metadati: mysql_fetch_fields non avanza il cursore di riga
        Console.WriteLine("  Campi:");
        foreach (MySqlField f in result.FetchFields())
            Console.WriteLine($"    {f.Name,-20} tipo={f.Type,-12} PK={f.IsPrimaryKey}  NN={f.IsNotNull}");

        // Iterazione righe tramite IEnumerable<MySqlRow>
        Console.WriteLine("  Righe:");
        // foreach (MySqlRow row in result)
        while (result?.FetchRow(out var row) == true)
        {
            Console.WriteLine(
                // $"    id={row.GetInt32("id"),-4} " +
                // $"nome={row["nome"],-30} " +
                // $"prezzo={row.GetDecimal("prezzo"):F2}  " +
                // $"attivo={row.GetBool("attivo")}  " +
                // $"creato={row.GetDateTime("creato")}"
                $"    id={row.GetInt32(0),-4} " +
                $"nome={row.GetString(1),-30} " +
                $"prezzo={row.GetDecimal(2):F2}  " +
                $"attivo={row.GetBool(3)}  " +
                $"creato={row.GetDateTime(4)}"
                );
        }
    }


    // ================================================================
    // 5. UPDATE
    // ================================================================
    private static void EseguiUpdate(MySql mysql)
    {
        Console.WriteLine("\n=== UPDATE ===");

        mysql.Query("UPDATE prodotti SET prezzo = prezzo * 1.10 WHERE attivo = 1");

        Console.WriteLine($"  Righe aggiornate : {mysql.AffectedRows()}");
        Console.WriteLine($"  Warning          : {mysql.WarningCount()}");
        Console.WriteLine($"  Info server      : {mysql.Info()}");
    }

    // ================================================================
    // 6. DELETE
    // ================================================================
    private static void EseguiDelete(MySql mysql, ulong id)
    {
        Console.WriteLine("\n=== DELETE ===");

        mysql.Query($"DELETE FROM prodotti WHERE id = {id}");
        Console.WriteLine($"  Righe eliminate: {mysql.AffectedRows()}");
    }

    // // ================================================================
    // // 7. Prepared statement – INSERT parametrizzato
    // // ================================================================
    // private static void EseguiPreparedInsert(MySql db)
    // {
    //     Console.WriteLine("\n=== Prepared INSERT ===");

    //     using MySqlStmt stmt = db.StmtInit();
    //     stmt.Prepare("INSERT INTO prodotti (nome, prezzo, attivo) VALUES (?, ?, ?)");
    //     Console.WriteLine($"  Parametri attesi: {stmt.ParamCount}");

    //     // Primo inserimento
    //     using (var bind = new MySqlBindBuilder(3))
    //     {
    //         bind.SetString(0, "Parmigiano Reggiano 24 mesi");
    //         bind.SetDecimal(1, 18.90m);
    //         bind.SetInt32(2, 1);

    //         stmt.BindParams(bind);
    //         stmt.Execute();
    //         Console.WriteLine($"  Inserito id={stmt.InsertId()}");
    //     }

    //     // Secondo inserimento – reset e riuso dello stesso statement
    //     stmt.Reset();
    //     using (var bind = new MysqlBind(3))
    //     {
    //         bind.SetString(0, "Pecorino Romano");
    //         bind.SetDecimal(1, 12.50m);
    //         bind.SetInt32(2, 1);

    //         stmt.mysql_stmt_bind_param(bind);
    //         stmt.Execute();
    //         Console.WriteLine($"  Inserito id={stmt.InsertId()}");
    //     }
    // }

    // // ================================================================
    // // 8. Prepared statement – SELECT con bind_result
    // //
    // // I buffer di output devono restare pinned per tutta la durata
    // // del loop di fetch: si usa GCHandle.Alloc(..., Pinned) invece
    // // di fixed{} che scade al termine del suo blocco lessicale.
    // // ================================================================
    // private static void EseguiPreparedSelect(MySql db)
    // {
    //     Console.WriteLine("\n=== Prepared SELECT ===");

    //     using MySqlStmt stmtSel = db.StmtInit();
    //     stmtSel.Prepare(
    //         "SELECT id, nome, prezzo FROM prodotti WHERE attivo = ?");

    //     // Parametro di input
    //     using var paramBind = new MysqlBind(1);
    //     paramBind.SetInt32(0, 1);
    //     stmtSel.mysql_stmt_bind_param(paramBind);

    //     // Buffer di output (uno array per colonna, compatibile con GCHandle.Pinned)
    //     var bufId      = new int[1];
    //     var bufNome    = new byte[101];
    //     var bufPrezzo  = new double[1];     // DECIMAL → double via driver C
    //     var lenId      = new nuint[1];
    //     var lenNome    = new nuint[1];      // driver scrive qui la lunghezza effettiva
    //     var lenPrezzo  = new nuint[1];
    //     var nullId     = new byte[1];       // 0 = valore presente, 1 = SQL NULL
    //     var nullNome   = new byte[1];
    //     var nullPrezzo = new byte[1];

    //     // Pin a lunga durata: restano validi per tutto il loop di fetch
    //     var hBufId      = GCHandle.Alloc(bufId,      GCHandleType.Pinned);
    //     var hBufNome    = GCHandle.Alloc(bufNome,    GCHandleType.Pinned);
    //     var hBufPrezzo  = GCHandle.Alloc(bufPrezzo,  GCHandleType.Pinned);
    //     var hLenId      = GCHandle.Alloc(lenId,      GCHandleType.Pinned);
    //     var hLenNome    = GCHandle.Alloc(lenNome,    GCHandleType.Pinned);
    //     var hLenPrezzo  = GCHandle.Alloc(lenPrezzo,  GCHandleType.Pinned);
    //     var hNullId     = GCHandle.Alloc(nullId,     GCHandleType.Pinned);
    //     var hNullNome   = GCHandle.Alloc(nullNome,   GCHandleType.Pinned);
    //     var hNullPrezzo = GCHandle.Alloc(nullPrezzo, GCHandleType.Pinned);

    //     try
    //     {
    //         using var resBind = new MysqlBind(3);
    //         unsafe
    //         {
    //             MysqlBindNative* p = resBind.NativeArray.Ptr;

    //             // colonna 0: id INT
    //             p[0].BufferType   = MysqlFieldType.Long;
    //             p[0].Buffer       = (void*)hBufId.AddrOfPinnedObject();
    //             p[0].BufferLength = sizeof(int);
    //             p[0].Length       = (uint*)hLenId.AddrOfPinnedObject();
    //             p[0].IsNull       = (byte*)hNullId.AddrOfPinnedObject();

    //             // colonna 1: nome VARCHAR
    //             p[1].BufferType   = MysqlFieldType.VarString;
    //             p[1].Buffer       = (void*)hBufNome.AddrOfPinnedObject();
    //             p[1].BufferLength = (uint)bufNome.Length;
    //             p[1].Length       = (uint*)hLenNome.AddrOfPinnedObject();
    //             p[1].IsNull       = (byte*)hNullNome.AddrOfPinnedObject();

    //             // colonna 2: prezzo DECIMAL → DOUBLE
    //             p[2].BufferType   = MysqlFieldType.Double;
    //             p[2].Buffer       = (void*)hBufPrezzo.AddrOfPinnedObject();
    //             p[2].BufferLength = sizeof(double);
    //             p[2].Length       = (uint*)hLenPrezzo.AddrOfPinnedObject();
    //             p[2].IsNull       = (byte*)hNullPrezzo.AddrOfPinnedObject();
    //         }

    //         stmtSel.mysql_stmt_bind_result(resBind);
    //         stmtSel.Execute();
    //         stmtSel.StoreResult();
    //         Console.WriteLine($"  Righe nel result: {stmtSel.NumRows()}");

    //         // Il driver popola i buffer ad ogni mysql_stmt_fetch()
    //         while (stmtSel.Fetch())
    //         {
    //             int    id   = nullId[0]     == 0 ? bufId[0]    : 0;
    //             string nome = nullNome[0]   == 0
    //                 ? Encoding.UTF8.GetString(bufNome, 0, (int)lenNome[0])
    //                 : "NULL";
    //             double prz  = nullPrezzo[0] == 0 ? bufPrezzo[0] : 0.0;
    //             Console.WriteLine($"    id={id,-4} nome={nome,-30} prezzo={prz:F2}");
    //         }

    //         stmtSel.FreeResult();
    //     }
    //     finally
    //     {
    //         // Libera i pin sempre, anche in caso di eccezione
    //         hBufId.Free();  hBufNome.Free();  hBufPrezzo.Free();
    //         hLenId.Free();  hLenNome.Free();  hLenPrezzo.Free();
    //         hNullId.Free(); hNullNome.Free(); hNullPrezzo.Free();
    //     }
    // }

    // ================================================================
    // 9. Transazione – autocommit / commit / rollback
    // ================================================================
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

    // // ================================================================
    // // 10. Multi-statement (richiede ClientFlags.MultiStatements)
    // // ================================================================
    // private static void EseguiMultiStatement(MySql db)
    // {
    //     Console.WriteLine("\n=== Multi-statement ===");

    //     db.Query("""
    //         SELECT COUNT(*) AS totale FROM prodotti;
    //         SELECT SUM(prezzo) AS totale_prezzi FROM prodotti WHERE attivo = 1
    //         """);

    //     int resultIndex = 0;
    //     do
    //     {
    //         using MysqlResult? res = db.mysql_store_result();
    //         if (res is not null)
    //         {
    //             MysqlRow? r = res.mysql_fetch_row();
    //             if (r is not null)
    //                 Console.WriteLine($"  Result #{resultIndex}: {r}");
    //         }
    //         resultIndex++;
    //     }
    //     while (db.mysql_next_result());
    // }


    // // ================================================================
    // // 11. USE RESULT – lettura streaming riga per riga
    // // ================================================================
    // private static void EseguiStreamingSelect(MySql db)
    // {
    //     Console.WriteLine("\n=== Streaming SELECT (mysql_use_result) ===");

    //     db.mysql_query("SELECT id, nome FROM prodotti ORDER BY id");

    //     using MysqlResult? res = db.mysql_use_result();
    //     if (res is null) return;

    //     MysqlRow? row;
    //     while ((row = res.mysql_fetch_row()) is not null)
    //         Console.WriteLine($"  id={row["id"]}  nome={row["nome"]}");
    // }

    // ================================================================
    // 12. Ping
    // ================================================================
    private static void EseguiPing(MySql mysql)
    {
        Console.WriteLine("\n=== Ping ===");
        int rc = mysql.Ping();
        Console.WriteLine($"  mysql_ping → {(rc == 0 ? "OK" : $"ERRORE (rc={rc})")}");
        Console.WriteLine($"  Stato server: {mysql.Stat()}");
    }

    // ================================================================
    // 13. Cleanup – DROP TABLE
    // ================================================================
    private static void Cleanup(MySql mysql)
    {
        Console.WriteLine("\n=== Cleanup ===");
        mysql.Query("DROP TABLE IF EXISTS prodotti");
        Console.WriteLine("  Tabella 'prodotti' eliminata.");
        Console.WriteLine("\nFine esempi.");
    }



}
