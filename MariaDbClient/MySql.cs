using System;
using System.Text;
using MariaDbClient.Exceptions;
using MariaDbClient.Native;

namespace MariaDbClient;

/// <summary>
/// Wrapper OOP per una connessione MariaDB/MySQL (<c>MYSQL*</c>).
/// <para>
/// I metodi pubblici replicano i nomi delle funzioni C della libmariadb
/// (es. <see cref="mysql_real_connect"/>, <see cref="mysql_query"/>, …)
/// e aggiungono varianti idiomatiche managed per comodità d'uso.
/// </para>
/// </summary>
public sealed unsafe class MySql : IDisposable
{
    // Handle opaco MYSQL*
    private IntPtr _mysql;
    private bool   _disposed;

    /// <summary>Handle nativo alla struttura MYSQL (MYSQL*).</summary>
    public IntPtr Handle => _mysql;

    /// <summary>
    /// Indica se la connessione è aperta (handle non nullo).
    /// Usa <see cref="mysql_ping"/> per un controllo reale lato server.
    /// </summary>
    public bool IsConnected => _mysql != IntPtr.Zero && !_disposed;

    // ================================================================
    //  Inizializzazione
    // ================================================================

    /// <summary>
    /// Inizializza l'handle MYSQL.
    /// Corrisponde a <c>mysql_init(NULL)</c>.
    /// </summary>
    public MySql()
    {
        _mysql = MariaDbImports.mysql_init(IntPtr.Zero);
        if (_mysql == IntPtr.Zero)
            throw new MySqlConnectionException("mysql_init ha restituito NULL: memoria insufficiente.");
    }

    /// <summary>
    /// Inizializza la libreria prima di creare connessioni in ambienti multi-thread.
    /// Corrisponde a <c>mysql_library_init</c>.
    /// </summary>
    public static int mysql_library_init(int argc = 0)
        => MariaDbImports.mysql_library_init(argc, null, null);

    /// <summary>Rilascia le risorse globali della libreria. Corrisponde a <c>mysql_library_end</c>.</summary>
    public static void mysql_library_end()
        => MariaDbImports.mysql_library_end();

    /// <summary>Inizializza il thread corrente per uso con MariaDB. Corrisponde a <c>mysql_thread_init</c>.</summary>
    public static int mysql_thread_init()
        => MariaDbImports.mysql_thread_init();

    /// <summary>Deinizia il thread corrente. Corrisponde a <c>mysql_thread_end</c>.</summary>
    public static void mysql_thread_end()
        => MariaDbImports.mysql_thread_end();

    // ================================================================
    //  Opzioni (mysql_options)
    // ================================================================

    /// <summary>
    /// Imposta un'opzione di connessione con un argomento puntatore generico.
    /// Corrisponde a <c>mysql_options</c>.
    /// </summary>
    public int mysql_options(MysqlOption option, void* arg)
    {
        ThrowIfDisposed();
        return MariaDbImports.mysql_options(_mysql, option, arg);
    }

    /// <summary>Imposta un'opzione con valore stringa UTF-8.</summary>
    public int mysql_options(MysqlOption option, string value)
    {
        ThrowIfDisposed();
        byte[] buf = Encoding.UTF8.GetBytes(value + "\0");
        fixed (byte* p = buf)
            return MariaDbImports.mysql_options(_mysql, option, p);
    }

    /// <summary>Imposta un'opzione con valore intero.</summary>
    public int mysql_options(MysqlOption option, int value)
    {
        ThrowIfDisposed();
        return MariaDbImports.mysql_options(_mysql, option, &value);
    }

    /// <summary>Imposta un'opzione con valore booleano (my_bool).</summary>
    public int mysql_options(MysqlOption option, bool value)
    {
        ThrowIfDisposed();
        byte b = value ? (byte)1 : (byte)0;
        return MariaDbImports.mysql_options(_mysql, option, &b);
    }

    // ================================================================
    //  Connessione (mysql_real_connect)
    // ================================================================

    /// <summary>
    /// Apre la connessione al server MariaDB/MySQL.
    /// Corrisponde a <c>mysql_real_connect</c>.
    /// </summary>
    /// <param name="host">Hostname o indirizzo IP; <c>null</c> = localhost.</param>
    /// <param name="user">Nome utente; <c>null</c> = utente corrente.</param>
    /// <param name="passwd">Password; <c>null</c> = nessuna password.</param>
    /// <param name="db">Database iniziale; <c>null</c> = nessuno.</param>
    /// <param name="port">Porta TCP; 0 = porta di default (3306).</param>
    /// <param name="unixSocket">Socket Unix; <c>null</c> = non usato.</param>
    /// <param name="clientFlag">Flag opzionali per la connessione.</param>
    public void mysql_real_connect(
        string?     host       = null,
        string?     user       = null,
        string?     passwd     = null,
        string?     db         = null,
        uint        port       = 0,
        string?     unixSocket = null,
        ClientFlags clientFlag = ClientFlags.Protocol41)
    {
        ThrowIfDisposed();

        fixed (byte* pHost   = ToUtf8NullOrNull(host))
        fixed (byte* pUser   = ToUtf8NullOrNull(user))
        fixed (byte* pPasswd = ToUtf8NullOrNull(passwd))
        fixed (byte* pDb     = ToUtf8NullOrNull(db))
        fixed (byte* pSock   = ToUtf8NullOrNull(unixSocket))
        {
            IntPtr result = MariaDbImports.mysql_real_connect(
                _mysql, pHost, pUser, pPasswd, pDb, port, pSock, clientFlag);

            if (result == IntPtr.Zero)
                throw new MySqlConnectionException(
                    mysql_error(), mysql_errno(), mysql_sqlstate());
        }
    }

    // ================================================================
    //  Selezione database (mysql_select_db)
    // ================================================================

    /// <summary>
    /// Seleziona il database corrente.
    /// Corrisponde a <c>mysql_select_db</c>.
    /// </summary>
    public void mysql_select_db(string db)
    {
        ThrowIfDisposed();
        fixed (byte* p = Utf8Z(db))
        {
            int rc = MariaDbImports.mysql_select_db(_mysql, p);
            if (rc != 0) ThrowLastError();
        }
    }

    // ================================================================
    //  Ping (mysql_ping)
    // ================================================================

    /// <summary>
    /// Verifica se la connessione è attiva; la riconnette se mysql_options(RECONNECT) è attivo.
    /// Corrisponde a <c>mysql_ping</c>.
    /// </summary>
    public int mysql_ping()
    {
        ThrowIfDisposed();
        return MariaDbImports.mysql_ping(_mysql);
    }

    // ================================================================
    //  Query semplici
    // ================================================================

    /// <summary>
    /// Esegue una query SQL senza risultati (INSERT, UPDATE, DELETE, DDL).
    /// Corrisponde a <c>mysql_query</c>.
    /// Lancia <see cref="MySqlQueryException"/> in caso di errore.
    /// </summary>
    public void mysql_query(string sql)
    {
        ThrowIfDisposed();
        fixed (byte* p = Utf8Z(sql))
        {
            int rc = MariaDbImports.mysql_query(_mysql, p);
            if (rc != 0) throw new MySqlQueryException(mysql_error(), sql, mysql_errno(), mysql_sqlstate());
        }
    }

    /// <summary>
    /// Esegue una query SQL con lunghezza esplicita (supporta dati binari nel testo).
    /// Corrisponde a <c>mysql_real_query</c>.
    /// </summary>
    public void mysql_real_query(ReadOnlySpan<byte> sql)
    {
        ThrowIfDisposed();
        fixed (byte* p = sql)
        {
            int rc = MariaDbImports.mysql_real_query(_mysql, p, (nuint)sql.Length);
            if (rc != 0) ThrowLastError();
        }
    }

    // ================================================================
    //  Result set
    // ================================================================

    /// <summary>
    /// Recupera l'intero result set in memoria client dopo una query SELECT.
    /// Corrisponde a <c>mysql_store_result</c>.
    /// Restituisce <c>null</c> se la query non produce righe (es. INSERT).
    /// </summary>
    public MysqlResult? mysql_store_result()
    {
        ThrowIfDisposed();
        IntPtr res = MariaDbImports.mysql_store_result(_mysql);
        return res == IntPtr.Zero ? null : new MysqlResult(res);
    }

    /// <summary>
    /// Avvia il recupero streaming del result set (una riga per volta).
    /// Corrisponde a <c>mysql_use_result</c>.
    /// </summary>
    public MysqlResult? mysql_use_result()
    {
        ThrowIfDisposed();
        IntPtr res = MariaDbImports.mysql_use_result(_mysql);
        return res == IntPtr.Zero ? null : new MysqlResult(res);
    }

    // ================================================================
    //  Stato post-query
    // ================================================================

    /// <summary>Numero di righe modificate dall'ultima query DML. Corrisponde a <c>mysql_affected_rows</c>.</summary>
    public ulong mysql_affected_rows()
    {
        ThrowIfDisposed();
        return MariaDbImports.mysql_affected_rows(_mysql);
    }

    /// <summary>Ultimo valore AUTO_INCREMENT generato. Corrisponde a <c>mysql_insert_id</c>.</summary>
    public ulong mysql_insert_id()
    {
        ThrowIfDisposed();
        return MariaDbImports.mysql_insert_id(_mysql);
    }

    /// <summary>Numero di warning generati dall'ultima query. Corrisponde a <c>mysql_warning_count</c>.</summary>
    public uint mysql_warning_count()
    {
        ThrowIfDisposed();
        return MariaDbImports.mysql_warning_count(_mysql);
    }

    /// <summary>Stringa informativa sull'ultima query (es. "Rows matched: 3"). Corrisponde a <c>mysql_info</c>.</summary>
    public string? mysql_info()
    {
        ThrowIfDisposed();
        return MariaDbImports.PtrToStringUtf8(MariaDbImports.mysql_info(_mysql));
    }

    /// <summary>Stringa di stato del server. Corrisponde a <c>mysql_stat</c>.</summary>
    public string? mysql_stat()
    {
        ThrowIfDisposed();
        return MariaDbImports.PtrToStringUtf8(MariaDbImports.mysql_stat(_mysql));
    }

    /// <summary>ID del thread di connessione sul server. Corrisponde a <c>mysql_thread_id</c>.</summary>
    public ulong mysql_thread_id()
    {
        ThrowIfDisposed();
        return MariaDbImports.mysql_thread_id(_mysql);
    }

    // ================================================================
    //  Errori
    // ================================================================

    /// <summary>Messaggio di errore dell'ultima operazione fallita. Corrisponde a <c>mysql_error</c>.</summary>
    public string mysql_error()
        => MariaDbImports.PtrToStringUtf8(MariaDbImports.mysql_error(_mysql)) ?? string.Empty;

    /// <summary>Codice di errore numerico. Corrisponde a <c>mysql_errno</c>.</summary>
    public uint mysql_errno()
        => MariaDbImports.mysql_errno(_mysql);

    /// <summary>SQLSTATE dell'ultima operazione. Corrisponde a <c>mysql_sqlstate</c>.</summary>
    public string mysql_sqlstate()
        => MariaDbImports.PtrToStringUtf8(MariaDbImports.mysql_sqlstate(_mysql)) ?? "00000";

    // ================================================================
    //  Metadati server
    // ================================================================

    /// <summary>Versione del server (es. "10.6.12-MariaDB"). Corrisponde a <c>mysql_get_server_info</c>.</summary>
    public string mysql_get_server_info()
    {
        ThrowIfDisposed();
        return MariaDbImports.PtrToStringUtf8(MariaDbImports.mysql_get_server_info(_mysql)) ?? string.Empty;
    }

    /// <summary>Informazioni sull'host di connessione. Corrisponde a <c>mysql_get_host_info</c>.</summary>
    public string mysql_get_host_info()
    {
        ThrowIfDisposed();
        return MariaDbImports.PtrToStringUtf8(MariaDbImports.mysql_get_host_info(_mysql)) ?? string.Empty;
    }

    /// <summary>Versione numerica del server (es. 100612). Corrisponde a <c>mysql_get_server_version</c>.</summary>
    public ulong mysql_get_server_version()
    {
        ThrowIfDisposed();
        return MariaDbImports.mysql_get_server_version(_mysql);
    }

    /// <summary>Versione del protocollo client/server. Corrisponde a <c>mysql_get_proto_info</c>.</summary>
    public uint mysql_get_proto_info()
    {
        ThrowIfDisposed();
        return MariaDbImports.mysql_get_proto_info(_mysql);
    }

    /// <summary>Versione della libreria client (statica, senza handle). Corrisponde a <c>mysql_get_client_info</c>.</summary>
    public static string mysql_get_client_info()
        => MariaDbImports.PtrToStringUtf8(MariaDbImports.mysql_get_client_info()) ?? string.Empty;

    /// <summary>Versione numerica della libreria client. Corrisponde a <c>mysql_get_client_version</c>.</summary>
    public static ulong mysql_get_client_version()
        => MariaDbImports.mysql_get_client_version();

    // ================================================================
    //  Transazioni
    // ================================================================

    /// <summary>
    /// Abilita o disabilita l'autocommit.
    /// Corrisponde a <c>mysql_autocommit</c>.
    /// </summary>
    public void mysql_autocommit(bool enable)
    {
        ThrowIfDisposed();
        byte rc = MariaDbImports.mysql_autocommit(_mysql, enable ? (byte)1 : (byte)0);
        if (rc != 0) ThrowLastError();
    }

    /// <summary>Esegue il commit della transazione corrente. Corrisponde a <c>mysql_commit</c>.</summary>
    public void mysql_commit()
    {
        ThrowIfDisposed();
        byte rc = MariaDbImports.mysql_commit(_mysql);
        if (rc != 0) ThrowLastError();
    }

    /// <summary>Esegue il rollback della transazione corrente. Corrisponde a <c>mysql_rollback</c>.</summary>
    public void mysql_rollback()
    {
        ThrowIfDisposed();
        byte rc = MariaDbImports.mysql_rollback(_mysql);
        if (rc != 0) ThrowLastError();
    }

    // ================================================================
    //  Escape
    // ================================================================

    /// <summary>
    /// Effettua l'escape di una stringa per uso sicuro in una query.
    /// Corrisponde a <c>mysql_real_escape_string</c>.
    /// </summary>
    public string mysql_real_escape_string(string input)
    {
        ThrowIfDisposed();
        byte[] from = Encoding.UTF8.GetBytes(input);
        // Il buffer di destinazione deve essere lungo almeno length*2+1
        byte[] to = new byte[from.Length * 2 + 1];
        fixed (byte* pFrom = from)
        fixed (byte* pTo   = to)
        {
            nuint outLen = MariaDbImports.mysql_real_escape_string(_mysql, pTo, pFrom, (nuint)from.Length);
            return Encoding.UTF8.GetString(to, 0, (int)outLen);
        }
    }

    /// <summary>
    /// Escape con carattere di quoting personalizzato.
    /// Corrisponde a <c>mysql_real_escape_string_quote</c>.
    /// </summary>
    public string mysql_real_escape_string_quote(string input, char quote = '\'')
    {
        ThrowIfDisposed();
        byte[] from = Encoding.UTF8.GetBytes(input);
        byte[] to   = new byte[from.Length * 2 + 1];
        fixed (byte* pFrom = from)
        fixed (byte* pTo   = to)
        {
            nuint outLen = MariaDbImports.mysql_real_escape_string_quote(
                _mysql, pTo, pFrom, (nuint)from.Length, (byte)quote);
            return Encoding.UTF8.GetString(to, 0, (int)outLen);
        }
    }

    // ================================================================
    //  Multi-statement
    // ================================================================

    /// <summary>
    /// Verifica se ci sono altri result set da leggere.
    /// Corrisponde a <c>mysql_more_results</c>.
    /// </summary>
    public bool mysql_more_results()
    {
        ThrowIfDisposed();
        return MariaDbImports.mysql_more_results(_mysql) != 0;
    }

    /// <summary>
    /// Avanza al prossimo result set in una query multi-statement.
    /// Corrisponde a <c>mysql_next_result</c>.
    /// Restituisce <c>false</c> quando non ci sono altri result set.
    /// </summary>
    public bool mysql_next_result()
    {
        ThrowIfDisposed();
        int rc = MariaDbImports.mysql_next_result(_mysql);
        if (rc > 0) ThrowLastError();
        return rc == 0;
    }

    // ================================================================
    //  Prepared statements
    // ================================================================

    /// <summary>
    /// Alloca e restituisce un nuovo <see cref="MysqlStmt"/>.
    /// Corrisponde a <c>mysql_stmt_init</c>.
    /// </summary>
    public MysqlStmt mysql_stmt_init()
    {
        ThrowIfDisposed();
        IntPtr stmt = MariaDbImports.mysql_stmt_init(_mysql);
        if (stmt == IntPtr.Zero)
            throw new MySqlStmtException("mysql_stmt_init ha restituito NULL: memoria insufficiente.");
        return new MysqlStmt(stmt);
    }

    // ================================================================
    //  Chiusura (mysql_close / Dispose)
    // ================================================================

    /// <summary>
    /// Chiude la connessione e libera la struttura MYSQL.
    /// Corrisponde a <c>mysql_close</c>.
    /// </summary>
    public void mysql_close()
    {
        if (_mysql != IntPtr.Zero)
        {
            MariaDbImports.mysql_close(_mysql);
            _mysql = IntPtr.Zero;
        }
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        mysql_close();
    }

    // ================================================================
    //  Helpers privati
    // ================================================================

    /// <summary>Converte una stringa C# in bytes UTF-8 NUL-terminated.</summary>
    private static byte[] Utf8Z(string s)
    {
        byte[] src = Encoding.UTF8.GetBytes(s);
        byte[] dst = new byte[src.Length + 1];
        src.CopyTo(dst, 0);
        return dst;
    }

    /// <summary>Restituisce un array UTF-8 NUL-terminated, o <c>null</c> se la stringa è null.</summary>
    private static byte[]? ToUtf8NullOrNull(string? s)
        => s is null ? null : Utf8Z(s);

    private void ThrowLastError() =>
        throw new MySqlException(mysql_error(), mysql_errno(), mysql_sqlstate());

    private void ThrowIfDisposed()
    {
        if (_disposed) throw new ObjectDisposedException(nameof(MySql));
    }
}
