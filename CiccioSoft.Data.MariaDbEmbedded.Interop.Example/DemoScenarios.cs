// Copyright (c) 2026 Francesco Crimi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;

namespace CiccioSoft.Data.MariaDbEmbedded.Interop.Example;

internal static class DemoScenarios
{
    internal static void RunAll(MySql mysql)
    {
        ExecuteDdl(mysql);
        ulong mozzarellaId = ExecuteInsertWithEscape(mysql);
        ExecuteSelect(mysql);
        ExecuteUpdate(mysql);
        ExecuteDelete(mysql, mozzarellaId);
        ExecutePreparedInsert(mysql);
        ExecutePreparedSelect(mysql);
        ExecuteTransaction(mysql);
        ExecuteMultiStatement(mysql);
        ExecuteStreamingSelect(mysql);
        ExecutePing(mysql);
        Cleanup(mysql);
    }

    private static void ExecuteDdl(MySql mysql)
    {
        ConsoleOutput.Section("DDL");

        mysql.Query("""
            CREATE TABLE IF NOT EXISTS prodotti (
                id       INT AUTO_INCREMENT PRIMARY KEY,
                nome     VARCHAR(100) NOT NULL,
                prezzo   DECIMAL(10,2) NOT NULL,
                attivo   TINYINT(1) NOT NULL DEFAULT 1,
                creato   DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4
            """);

        ConsoleOutput.Message("Tabella 'prodotti' creata (o già esistente).");
    }

    private static ulong ExecuteInsertWithEscape(MySql mysql)
    {
        ConsoleOutput.Section("INSERT con escape");

        string escapedName = mysql.RealEscapeString("Mozzarella \"D.O.P.\"");
        mysql.Query($"INSERT INTO prodotti (nome, prezzo) VALUES ('{escapedName}', 4.50)");

        ulong id = mysql.InsertId();
        ulong affected = mysql.AffectedRows();
        ConsoleOutput.Message($"Inserito id={id}, affected={affected}");
        return id;
    }

    private static void ExecuteSelect(MySql mysql)
    {
        ConsoleOutput.Section("SELECT");

        mysql.Query("SELECT id, nome, prezzo, attivo, creato FROM prodotti");

        using MySqlResult result = mysql.StoreResult()
            ?? throw new InvalidOperationException("Nessun result set.");

        ConsoleOutput.Message($"Righe={result.NumRows}  Colonne={result.NumFields}");
        ConsoleOutput.Message("Campi:");
        foreach (MySqlField field in result.FetchFields())
            ConsoleOutput.Message($"  {field.Name,-20} tipo={field.Type,-12} PK={field.IsPrimaryKey}  NN={field.IsNotNull}");

        MySqlField firstField = result.FetchField(0);
        ConsoleOutput.Message($"Campo #0: {firstField.Name}  tipo={firstField.Type}  PK={firstField.IsPrimaryKey}  NN={firstField.IsNotNull}");

        ConsoleOutput.Message("Righe:");
        while (result.FetchRow(out var row))
        {
            ConsoleOutput.Message(
                $"  id={row.GetInt32("id"),-4} " +
                $"nome={row.GetString("nome"),-30} " +
                $"prezzo={row.GetDecimal("prezzo"):F2}  " +
                $"attivo={row.GetBool("attivo")}  " +
                $"creato={row.GetDateTime("creato")}");
        }
    }

    private static void ExecuteUpdate(MySql mysql)
    {
        ConsoleOutput.Section("UPDATE");

        mysql.Query("UPDATE prodotti SET prezzo = prezzo * 1.10 WHERE attivo = 1");

        ConsoleOutput.Message($"Righe aggiornate : {mysql.AffectedRows()}");
        ConsoleOutput.Message($"Warning          : {mysql.WarningCount()}");
        ConsoleOutput.Message($"Info server      : {mysql.Info()}");
    }

    private static void ExecuteDelete(MySql mysql, ulong id)
    {
        ConsoleOutput.Section("DELETE");

        mysql.Query($"DELETE FROM prodotti WHERE id = {id}");
        ConsoleOutput.Message($"Righe eliminate: {mysql.AffectedRows()}");
    }

    private static void ExecutePreparedInsert(MySql db)
    {
        ConsoleOutput.Section("Prepared INSERT");

        using MySqlStmt stmt = db.StmtInit();
        stmt.Prepare("INSERT INTO prodotti (nome, prezzo, attivo) VALUES (?, ?, ?)");
        ConsoleOutput.Message($"Parametri attesi: {stmt.ParamCount()}");

        using (var bind = new MySqlBindBuilder(3))
        {
            bind[0].SetString("Parmigiano Reggiano 24 mesi");
            bind[1].SetDecimal(18.90m);
            bind[2].SetInt32(1);
            stmt.BindParam(bind);
            stmt.Execute();
            ConsoleOutput.Message($"Inserito id={stmt.InsertId()}");
        }

        stmt.Reset();
        using (var bind = new MySqlBindBuilder(3))
        {
            bind[0].SetString("Pecorino Romano");
            bind[1].SetDecimal(12.50m);
            bind[2].SetInt32(1);
            stmt.BindParam(bind);
            stmt.Execute();
            ConsoleOutput.Message($"Inserito id={stmt.InsertId()}");
        }
    }

    private static void ExecutePreparedSelect(MySql db)
    {
        ConsoleOutput.Section("Prepared SELECT");

        using MySqlStmt stmt = db.StmtInit();
        stmt.Prepare("SELECT id, nome, prezzo FROM prodotti WHERE attivo = ?");

        using var paramBuilder = new MySqlBindBuilder(1);
        paramBuilder[0].SetInt32(1);
        stmt.BindParam(paramBuilder);

        using var resultBuilder = new MySqlBindBuilder(3);
        resultBuilder[0].SetInt32(0);
        resultBuilder[1].SetString(100);
        resultBuilder[2].SetDouble(0.0);
        stmt.BindResult(resultBuilder);

        stmt.Execute();
        stmt.StoreResult();
        ConsoleOutput.Message($"Righe nel result: {stmt.NumRows()}");

        while (stmt.Fetch())
        {
            int id = resultBuilder[0].GetInt32();
            string name = resultBuilder[1].GetString() ?? "NULL";
            double price = resultBuilder[2].GetDouble();
            ConsoleOutput.Message($"  id={id,-4} nome={name,-30} prezzo={price:F2}");
        }

        stmt.FreeResult();
    }

    private static void ExecuteTransaction(MySql mysql)
    {
        ConsoleOutput.Section("Transazione");

        mysql.AutoCommit(false);
        try
        {
            mysql.Query("UPDATE prodotti SET prezzo = 99.99 WHERE id = 1");
            mysql.Query("UPDATE prodotti SET prezzo = 88.88 WHERE id = 2");
            mysql.Commit();
            ConsoleOutput.Message("Commit eseguito.");
        }
        catch
        {
            mysql.Rollback();
            ConsoleOutput.Message("Rollback eseguito.");
            throw;
        }
        finally
        {
            mysql.AutoCommit(true);
        }
    }

    private static void ExecuteMultiStatement(MySql mysql)
    {
        ConsoleOutput.Section("Multi-statement");

        mysql.Query("""
            SELECT COUNT(*) AS totale FROM prodotti;
            SELECT SUM(prezzo) AS totale_prezzi FROM prodotti WHERE attivo = 1
            """);

        int resultIndex = 0;
        do
        {
            using var result = mysql.StoreResult();
            while (result?.FetchRow(out var row) == true)
                ConsoleOutput.Message($"Result #{resultIndex}: {row.ToString()}");

            resultIndex++;
        }
        while (mysql.NextResult());
    }

    private static void ExecuteStreamingSelect(MySql mysql)
    {
        ConsoleOutput.Section("Streaming SELECT (mysql_use_result)");

        mysql.Query("SELECT id, nome FROM prodotti ORDER BY id");

        using MySqlResult? result = mysql.UseResult();
        if (result is null)
            return;

        while (result.FetchRow(out MySqlRow row))
            ConsoleOutput.Message($"id={row.GetInt32("id")}  nome={row.GetString("nome")}");
    }

    private static void ExecutePing(MySql mysql)
    {
        ConsoleOutput.Section("Ping");
        mysql.Ping();
        ConsoleOutput.Message("mysql_ping → OK");
        ConsoleOutput.Message($"Stato server: {mysql.Stat()}");
    }

    private static void Cleanup(MySql mysql)
    {
        ConsoleOutput.Section("Cleanup");
        mysql.Query("DROP TABLE IF EXISTS prodotti");
        ConsoleOutput.Message("Tabella 'prodotti' eliminata.");
        ConsoleOutput.Message("Fine esempi.");
    }
}
