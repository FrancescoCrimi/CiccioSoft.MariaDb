// ============================================================
//  MariaDbClient – Esempi d'uso completi
//  Copertura: connessione, DDL, CRUD, prepared statements,
//  transazioni, multi-result, streaming, escape, metadati
// ============================================================

using System;
using System.Runtime.InteropServices;
using System.Text;
using MariaDbClient;
using MariaDbClient.Native;

namespace MariaDbClient.Samples;

internal static class Program
{
    // ----------------------------------------------------------------
    //  Parametri di connessione – modificare prima di eseguire
    // ----------------------------------------------------------------
    private const string Host = "127.0.0.1";
    private const string User = "root";
    private const string Passwd = "password";
    private const string Db = "test";
    private const uint Port = 3306;

    // ----------------------------------------------------------------
    //  Entry point
    // ----------------------------------------------------------------
    public static void Main(string[] args)
    {
        // mysql_library_init è facoltativo in single-thread,
        // obbligatorio in ambienti multi-thread prima del primo MySql()
        // MySql.mysql_library_init();

        try
        {
            using var db = AprireConnessione();

            EseguiDdl(db);
            ulong idMozzarella = EseguiInsertConEscape(db);
            EseguiSelect(db);
            EseguiUpdate(db);
            EseguiDelete(db, idMozzarella);
            EseguiPreparedInsert(db);
            EseguiPreparedSelect(db);
            EseguiTransazione(db);
            EseguiMultiStatement(db);
            EseguiStreamingSelect(db);
            EseguiPing(db);
            Cleanup(db);
        }
        finally
        {
            // MySql.mysql_library_end();
        }
    }

    // ================================================================
    // 1. Connessione
    // ================================================================
    private static MySql AprireConnessione()
    {
        var db = new MySql();

        // Opzioni prima di chiamare mysql_real_connect
        db.mysql_options(MysqlOption.OptConnectTimeout, 5);
        db.mysql_options(MysqlOption.SetCharsetName, "utf8mb4");
        db.mysql_options(MysqlOption.OptReconnect, true);
        db.mysql_options(MysqlOption.OptSslCa, false);

        // MultiStatements è obbligatorio per la sezione multi-result
        db.mysql_real_connect(
            host: Host,
            user: User,
            passwd: Passwd,
            db: Db,
            port: Port,
            clientFlag: ClientFlags.Protocol41 | ClientFlags.MultiStatements
        );

        Console.WriteLine("=== Connessione aperta ===");
        Console.WriteLine($"  Server  : {db.mysql_get_server_info()}");
        Console.WriteLine($"  Client  : {MySql.mysql_get_client_info()}");
        Console.WriteLine($"  Host    : {db.mysql_get_host_info()}");
        Console.WriteLine($"  Thread  : {db.mysql_thread_id()}");
        Console.WriteLine($"  Proto   : {db.mysql_get_proto_info()}");

        return db;
    }

    // ================================================================
    // 2. DDL – CREATE TABLE
    // ================================================================
    private static void EseguiDdl(MySql db)
    {
        Console.WriteLine("\n=== DDL ===");

        db.mysql_query("""
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
    private static ulong EseguiInsertConEscape(MySql db)
    {
        Console.WriteLine("\n=== INSERT con escape ===");

        // mysql_real_escape_string gestisce apici, backslash e caratteri binari
        string nomeEsc = db.mysql_real_escape_string("Mozzarella \"D.O.P.\"");
        db.mysql_query($"INSERT INTO prodotti (nome, prezzo) VALUES ('{nomeEsc}', 4.50)");

        ulong id = db.mysql_insert_id();
        ulong affected = db.mysql_affected_rows();
        Console.WriteLine($"  Inserito id={id}, affected={affected}");
        return id;
    }

    // ================================================================
    // 4. SELECT con mysql_store_result
    // ================================================================
    private static void EseguiSelect(MySql db)
    {
        Console.WriteLine("\n=== SELECT ===");

        db.mysql_query("SELECT id, nome, prezzo, attivo, creato FROM prodotti");

        using MysqlResult? res = db.mysql_store_result();
        if (res is null) return;

        Console.WriteLine($"  Righe={res.NumRows}  Colonne={res.NumFields}");

        // Metadati: mysql_fetch_fields non avanza il cursore di riga
        Console.WriteLine("  Campi:");
        foreach (MysqlField f in res.mysql_fetch_fields())
            Console.WriteLine($"    {f.Name,-20} tipo={f.Type,-12} PK={f.IsPrimaryKey}  NN={f.IsNotNull}");

        // Iterazione righe tramite IEnumerable<MysqlRow>
        Console.WriteLine("  Righe:");
        foreach (MysqlRow r in res)
        {
            Console.WriteLine(
                $"    id={r.GetInt32("id"),-4} " +
                $"nome={r["nome"],-30} " +
                $"prezzo={r.GetDecimal("prezzo"):F2}  " +
                $"attivo={r.GetBool("attivo")}  " +
                $"creato={r.GetDateTime("creato")}");
        }
    }

    // ================================================================
    // 5. UPDATE
    // ================================================================
    private static void EseguiUpdate(MySql db)
    {
        Console.WriteLine("\n=== UPDATE ===");

        db.mysql_query("UPDATE prodotti SET prezzo = prezzo * 1.10 WHERE attivo = 1");

        Console.WriteLine($"  Righe aggiornate : {db.mysql_affected_rows()}");
        Console.WriteLine($"  Warning          : {db.mysql_warning_count()}");
        Console.WriteLine($"  Info server      : {db.mysql_info()}");
    }

    // ================================================================
    // 6. DELETE
    // ================================================================
    private static void EseguiDelete(MySql db, ulong id)
    {
        Console.WriteLine("\n=== DELETE ===");

        db.mysql_query($"DELETE FROM prodotti WHERE id = {id}");
        Console.WriteLine($"  Righe eliminate: {db.mysql_affected_rows()}");
    }

    // ================================================================
    // 7. Prepared statement – INSERT parametrizzato
    // ================================================================
    private static void EseguiPreparedInsert(MySql db)
    {
        Console.WriteLine("\n=== Prepared INSERT ===");

        using MysqlStmt stmt = db.mysql_stmt_init();
        stmt.mysql_stmt_prepare("INSERT INTO prodotti (nome, prezzo, attivo) VALUES (?, ?, ?)");
        Console.WriteLine($"  Parametri attesi: {stmt.mysql_stmt_param_count()}");

        // Primo inserimento
        using (var bind = new MysqlBind(3))
        {
            bind.SetString(0, "Parmigiano Reggiano 24 mesi");
            bind.SetDecimal(1, 18.90m);
            bind.SetInt32(2, 1);

            stmt.mysql_stmt_bind_param(bind);
            stmt.mysql_stmt_execute();
            Console.WriteLine($"  Inserito id={stmt.mysql_stmt_insert_id()}");
        }

        // Secondo inserimento – reset e riuso dello stesso statement
        stmt.mysql_stmt_reset();
        using (var bind = new MysqlBind(3))
        {
            bind.SetString(0, "Pecorino Romano");
            bind.SetDecimal(1, 12.50m);
            bind.SetInt32(2, 1);

            stmt.mysql_stmt_bind_param(bind);
            stmt.mysql_stmt_execute();
            Console.WriteLine($"  Inserito id={stmt.mysql_stmt_insert_id()}");
        }
    }

    // ================================================================
    // 8. Prepared statement – SELECT con bind_result
    //
    // I buffer di output devono restare pinned per tutta la durata
    // del loop di fetch: si usa GCHandle.Alloc(..., Pinned) invece
    // di fixed{} che scade al termine del suo blocco lessicale.
    // ================================================================
    private static void EseguiPreparedSelect(MySql db)
    {
        Console.WriteLine("\n=== Prepared SELECT ===");

        using MysqlStmt stmtSel = db.mysql_stmt_init();
        stmtSel.mysql_stmt_prepare(
            "SELECT id, nome, prezzo FROM prodotti WHERE attivo = ?");

        // Parametro di input
        using var paramBind = new MysqlBind(1);
        paramBind.SetInt32(0, 1);
        stmtSel.mysql_stmt_bind_param(paramBind);

        // Buffer di output (uno array per colonna, compatibile con GCHandle.Pinned)
        var bufId = new int[1];
        var bufNome = new byte[101];
        var bufPrezzo = new double[1];     // DECIMAL → double via driver C
        var lenId = new nuint[1];
        var lenNome = new nuint[1];      // driver scrive qui la lunghezza effettiva
        var lenPrezzo = new nuint[1];
        var nullId = new byte[1];       // 0 = valore presente, 1 = SQL NULL
        var nullNome = new byte[1];
        var nullPrezzo = new byte[1];

        // Pin a lunga durata: restano validi per tutto il loop di fetch
        var hBufId = GCHandle.Alloc(bufId, GCHandleType.Pinned);
        var hBufNome = GCHandle.Alloc(bufNome, GCHandleType.Pinned);
        var hBufPrezzo = GCHandle.Alloc(bufPrezzo, GCHandleType.Pinned);
        var hLenId = GCHandle.Alloc(lenId, GCHandleType.Pinned);
        var hLenNome = GCHandle.Alloc(lenNome, GCHandleType.Pinned);
        var hLenPrezzo = GCHandle.Alloc(lenPrezzo, GCHandleType.Pinned);
        var hNullId = GCHandle.Alloc(nullId, GCHandleType.Pinned);
        var hNullNome = GCHandle.Alloc(nullNome, GCHandleType.Pinned);
        var hNullPrezzo = GCHandle.Alloc(nullPrezzo, GCHandleType.Pinned);

        try
        {
            using var resBind = new MysqlBind(3);
            unsafe
            {
                MysqlBindNative* p = resBind.NativeArray.Ptr;

                // colonna 0: id INT
                p[0].BufferType = MysqlFieldType.Long;
                p[0].Buffer = (void*)hBufId.AddrOfPinnedObject();
                p[0].BufferLength = sizeof(int);
                p[0].Length = (uint*)hLenId.AddrOfPinnedObject();
                p[0].IsNull = (byte*)hNullId.AddrOfPinnedObject();

                // colonna 1: nome VARCHAR
                p[1].BufferType = MysqlFieldType.VarString;
                p[1].Buffer = (void*)hBufNome.AddrOfPinnedObject();
                p[1].BufferLength = (uint)bufNome.Length;
                p[1].Length = (uint*)hLenNome.AddrOfPinnedObject();
                p[1].IsNull = (byte*)hNullNome.AddrOfPinnedObject();

                // colonna 2: prezzo DECIMAL → DOUBLE
                p[2].BufferType = MysqlFieldType.Double;
                p[2].Buffer = (void*)hBufPrezzo.AddrOfPinnedObject();
                p[2].BufferLength = sizeof(double);
                p[2].Length = (uint*)hLenPrezzo.AddrOfPinnedObject();
                p[2].IsNull = (byte*)hNullPrezzo.AddrOfPinnedObject();
            }

            stmtSel.mysql_stmt_bind_result(resBind);
            stmtSel.mysql_stmt_execute();
            stmtSel.mysql_stmt_store_result();
            Console.WriteLine($"  Righe nel result: {stmtSel.mysql_stmt_num_rows()}");

            // Il driver popola i buffer ad ogni mysql_stmt_fetch()
            while (stmtSel.mysql_stmt_fetch())
            {
                int id = nullId[0] == 0 ? bufId[0] : 0;
                string nome = nullNome[0] == 0
                    ? Encoding.UTF8.GetString(bufNome, 0, (int)lenNome[0])
                    : "NULL";
                double prz = nullPrezzo[0] == 0 ? bufPrezzo[0] : 0.0;
                Console.WriteLine($"    id={id,-4} nome={nome,-30} prezzo={prz:F2}");
            }

            stmtSel.mysql_stmt_free_result();
        }
        finally
        {
            // Libera i pin sempre, anche in caso di eccezione
            hBufId.Free(); hBufNome.Free(); hBufPrezzo.Free();
            hLenId.Free(); hLenNome.Free(); hLenPrezzo.Free();
            hNullId.Free(); hNullNome.Free(); hNullPrezzo.Free();
        }
    }

    // ================================================================
    // 9. Transazione – autocommit / commit / rollback
    // ================================================================
    private static void EseguiTransazione(MySql db)
    {
        Console.WriteLine("\n=== Transazione ===");

        db.mysql_autocommit(false);
        try
        {
            db.mysql_query("UPDATE prodotti SET prezzo = 99.99 WHERE id = 1");
            db.mysql_query("UPDATE prodotti SET prezzo = 88.88 WHERE id = 2");
            db.mysql_commit();
            Console.WriteLine("  Commit eseguito.");
        }
        catch
        {
            db.mysql_rollback();
            Console.WriteLine("  Rollback eseguito.");
            throw;
        }
        finally
        {
            db.mysql_autocommit(true);
        }
    }

    // ================================================================
    // 10. Multi-statement (richiede ClientFlags.MultiStatements)
    // ================================================================
    private static void EseguiMultiStatement(MySql db)
    {
        Console.WriteLine("\n=== Multi-statement ===");

        db.mysql_query("""
            SELECT COUNT(*) AS totale FROM prodotti;
            SELECT SUM(prezzo) AS totale_prezzi FROM prodotti WHERE attivo = 1
            """);

        int resultIndex = 0;
        do
        {
            using MysqlResult? res = db.mysql_store_result();
            if (res is not null)
            {
                MysqlRow? r = res.mysql_fetch_row();
                if (r is not null)
                    Console.WriteLine($"  Result #{resultIndex}: {r}");
            }
            resultIndex++;
        }
        while (db.mysql_next_result());
    }

    // ================================================================
    // 11. USE RESULT – lettura streaming riga per riga
    // ================================================================
    private static void EseguiStreamingSelect(MySql db)
    {
        Console.WriteLine("\n=== Streaming SELECT (mysql_use_result) ===");

        db.mysql_query("SELECT id, nome FROM prodotti ORDER BY id");

        using MysqlResult? res = db.mysql_use_result();
        if (res is null) return;

        MysqlRow? row;
        while ((row = res.mysql_fetch_row()) is not null)
            Console.WriteLine($"  id={row["id"]}  nome={row["nome"]}");
    }

    // ================================================================
    // 12. Ping
    // ================================================================
    private static void EseguiPing(MySql db)
    {
        Console.WriteLine("\n=== Ping ===");
        int rc = db.mysql_ping();
        Console.WriteLine($"  mysql_ping → {(rc == 0 ? "OK" : $"ERRORE (rc={rc})")}");
        Console.WriteLine($"  Stato server: {db.mysql_stat()}");
    }

    // ================================================================
    // 13. Cleanup – DROP TABLE
    // ================================================================
    private static void Cleanup(MySql db)
    {
        Console.WriteLine("\n=== Cleanup ===");
        db.mysql_query("DROP TABLE IF EXISTS prodotti");
        Console.WriteLine("  Tabella 'prodotti' eliminata.");
        Console.WriteLine("\nFine esempi.");
    }
}
