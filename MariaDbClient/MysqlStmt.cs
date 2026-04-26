using System;
using System.Text;
using MariaDbClient.Exceptions;
using MariaDbClient.Native;

namespace MariaDbClient;

/// <summary>
/// Wrapper managed per un prepared statement nativo (<c>MYSQL_STMT*</c>).
/// I nomi dei metodi coincidono con le funzioni C <c>mysql_stmt_*</c>.
/// </summary>
public sealed unsafe class MysqlStmt : IDisposable
{
    private IntPtr _stmt;
    private bool   _disposed;

    /// <summary>Handle nativo al prepared statement (MYSQL_STMT*).</summary>
    public IntPtr Handle => _stmt;

    internal MysqlStmt(IntPtr stmt) => _stmt = stmt;

    // ------------------------------------------------------------------
    //  mysql_stmt_prepare
    // ------------------------------------------------------------------

    /// <summary>
    /// Prepara la query SQL sul server.
    /// Corrisponde a <c>mysql_stmt_prepare</c>.
    /// </summary>
    public void mysql_stmt_prepare(string query)
    {
        ThrowIfDisposed();
        byte[] utf8 = Encoding.UTF8.GetBytes(query);
        fixed (byte* p = utf8)
        {
            int rc = MariaDbImports.mysql_stmt_prepare(_stmt, p, (nuint)utf8.Length);
            if (rc != 0) ThrowStmtError();
        }
    }

    // ------------------------------------------------------------------
    //  mysql_stmt_bind_param / mysql_stmt_bind_result
    // ------------------------------------------------------------------

    /// <summary>
    /// Associa i parametri di input (<c>?</c>) allo statement.
    /// Corrisponde a <c>mysql_stmt_bind_param</c>.
    /// </summary>
    public void mysql_stmt_bind_param(MysqlBind bind)
    {
        ThrowIfDisposed();
        byte rc = MariaDbImports.mysql_stmt_bind_param(_stmt, bind.NativeArray.Ptr);
        if (rc != 0) ThrowStmtError();
    }

    /// <summary>
    /// Associa i buffer di output per i risultati.
    /// Corrisponde a <c>mysql_stmt_bind_result</c>.
    /// </summary>
    public void mysql_stmt_bind_result(MysqlBind bind)
    {
        ThrowIfDisposed();
        byte rc = MariaDbImports.mysql_stmt_bind_result(_stmt, bind.NativeArray.Ptr);
        if (rc != 0) ThrowStmtError();
    }

    // ------------------------------------------------------------------
    //  mysql_stmt_execute
    // ------------------------------------------------------------------

    /// <summary>
    /// Esegue lo statement preparato.
    /// Corrisponde a <c>mysql_stmt_execute</c>.
    /// </summary>
    public void mysql_stmt_execute()
    {
        ThrowIfDisposed();
        int rc = MariaDbImports.mysql_stmt_execute(_stmt);
        if (rc != 0) ThrowStmtError();
    }

    // ------------------------------------------------------------------
    //  mysql_stmt_store_result / mysql_stmt_free_result
    // ------------------------------------------------------------------

    /// <summary>
    /// Trasferisce l'intero result set in memoria client.
    /// Corrisponde a <c>mysql_stmt_store_result</c>.
    /// </summary>
    public void mysql_stmt_store_result()
    {
        ThrowIfDisposed();
        int rc = MariaDbImports.mysql_stmt_store_result(_stmt);
        if (rc != 0) ThrowStmtError();
    }

    /// <summary>
    /// Libera il result set del prepared statement.
    /// Corrisponde a <c>mysql_stmt_free_result</c>.
    /// </summary>
    public void mysql_stmt_free_result()
    {
        ThrowIfDisposed();
        MariaDbImports.mysql_stmt_free_result(_stmt);
    }

    // ------------------------------------------------------------------
    //  mysql_stmt_fetch / mysql_stmt_fetch_column
    // ------------------------------------------------------------------

    /// <summary>
    /// Legge la riga successiva nel buffer associato con bind_result.
    /// Restituisce <c>true</c> se c'è una riga, <c>false</c> a fine set.
    /// Corrisponde a <c>mysql_stmt_fetch</c>.
    /// </summary>
    public bool mysql_stmt_fetch()
    {
        ThrowIfDisposed();
        int rc = MariaDbImports.mysql_stmt_fetch(_stmt);
        // 0 = OK, 100 = MYSQL_NO_DATA, 101 = MYSQL_DATA_TRUNCATED
        if (rc == 0 || rc == 101) return true;
        if (rc == 100) return false;           // MYSQL_NO_DATA
        ThrowStmtError();
        return false;
    }

    /// <summary>
    /// Legge una singola colonna della riga corrente.
    /// Corrisponde a <c>mysql_stmt_fetch_column</c>.
    /// </summary>
    public void mysql_stmt_fetch_column(MysqlBind bind, uint column, nuint offset = 0)
    {
        ThrowIfDisposed();
        int rc = MariaDbImports.mysql_stmt_fetch_column(_stmt, bind.NativeArray.Ptr, column, offset);
        if (rc != 0) ThrowStmtError();
    }

    // ------------------------------------------------------------------
    //  mysql_stmt_reset
    // ------------------------------------------------------------------

    /// <summary>
    /// Resetta lo statement azzerando i buffer.
    /// Corrisponde a <c>mysql_stmt_reset</c>.
    /// </summary>
    public void mysql_stmt_reset()
    {
        ThrowIfDisposed();
        MariaDbImports.mysql_stmt_reset(_stmt);
    }

    // ------------------------------------------------------------------
    //  Metadati e informazioni di stato
    // ------------------------------------------------------------------

    /// <summary>Numero di colonne nel result set. Corrisponde a <c>mysql_stmt_field_count</c>.</summary>
    public uint mysql_stmt_field_count()
    {
        ThrowIfDisposed();
        return MariaDbImports.mysql_stmt_field_count(_stmt);
    }

    /// <summary>Numero di parametri (<c>?</c>). Corrisponde a <c>mysql_stmt_param_count</c>.</summary>
    public nuint mysql_stmt_param_count()
    {
        ThrowIfDisposed();
        return MariaDbImports.mysql_stmt_param_count(_stmt);
    }

    /// <summary>Righe modificate/inserite/cancellate. Corrisponde a <c>mysql_stmt_affected_rows</c>.</summary>
    public ulong mysql_stmt_affected_rows()
    {
        ThrowIfDisposed();
        return MariaDbImports.mysql_stmt_affected_rows(_stmt);
    }

    /// <summary>Ultimo AUTO_INCREMENT generato. Corrisponde a <c>mysql_stmt_insert_id</c>.</summary>
    public ulong mysql_stmt_insert_id()
    {
        ThrowIfDisposed();
        return MariaDbImports.mysql_stmt_insert_id(_stmt);
    }

    /// <summary>Numero di righe nel result set (solo dopo store_result). Corrisponde a <c>mysql_stmt_num_rows</c>.</summary>
    public ulong mysql_stmt_num_rows()
    {
        ThrowIfDisposed();
        return MariaDbImports.mysql_stmt_num_rows(_stmt);
    }

    /// <summary>
    /// Restituisce il result set dei metadati delle colonne.
    /// Corrisponde a <c>mysql_stmt_result_metadata</c>.
    /// </summary>
    public MysqlResult? mysql_stmt_result_metadata()
    {
        ThrowIfDisposed();
        var res = MariaDbImports.mysql_stmt_result_metadata(_stmt);
        return res == IntPtr.Zero ? null : new MysqlResult(res);
    }

    /// <summary>Cursore corrente per navigazione. Corrisponde a <c>mysql_stmt_row_tell</c>.</summary>
    public ulong mysql_stmt_row_tell()
    {
        ThrowIfDisposed();
        return MariaDbImports.mysql_stmt_row_tell(_stmt);
    }

    /// <summary>Sposta il cursore. Corrisponde a <c>mysql_stmt_row_seek</c>.</summary>
    public ulong mysql_stmt_row_seek(ulong offset)
    {
        ThrowIfDisposed();
        return MariaDbImports.mysql_stmt_row_seek(_stmt, offset);
    }

    /// <summary>Sposta il cursore assoluto. Corrisponde a <c>mysql_stmt_data_seek</c>.</summary>
    public void mysql_stmt_data_seek(ulong offset)
    {
        ThrowIfDisposed();
        MariaDbImports.mysql_stmt_data_seek(_stmt, offset);
    }

    // ------------------------------------------------------------------
    //  Errori dello statement
    // ------------------------------------------------------------------

    /// <summary>Messaggio di errore corrente. Corrisponde a <c>mysql_stmt_error</c>.</summary>
    public string mysql_stmt_error()
    {
        ThrowIfDisposed();
        return MariaDbImports.PtrToStringUtf8(MariaDbImports.mysql_stmt_error(_stmt)) ?? string.Empty;
    }

    /// <summary>Codice di errore numerico. Corrisponde a <c>mysql_stmt_errno</c>.</summary>
    public uint mysql_stmt_errno()
    {
        ThrowIfDisposed();
        return MariaDbImports.mysql_stmt_errno(_stmt);
    }

    /// <summary>SQLSTATE corrente. Corrisponde a <c>mysql_stmt_sqlstate</c>.</summary>
    public string mysql_stmt_sqlstate()
    {
        ThrowIfDisposed();
        return MariaDbImports.PtrToStringUtf8(MariaDbImports.mysql_stmt_sqlstate(_stmt)) ?? "00000";
    }

    /// <summary>
    /// Avanza al prossimo result set in una query multi-risultato.
    /// Corrisponde a <c>mysql_stmt_next_result</c>.
    /// </summary>
    public bool mysql_stmt_next_result()
    {
        ThrowIfDisposed();
        int rc = MariaDbImports.mysql_stmt_next_result(_stmt);
        if (rc > 0) ThrowStmtError();
        return rc == 0;
    }

    // ------------------------------------------------------------------
    //  mysql_stmt_close / Dispose
    // ------------------------------------------------------------------

    /// <summary>
    /// Chiude e dealloca lo statement sul server.
    /// Corrisponde a <c>mysql_stmt_close</c>.
    /// </summary>
    public void mysql_stmt_close()
    {
        if (_stmt != IntPtr.Zero)
        {
            MariaDbImports.mysql_stmt_close(_stmt);
            _stmt = IntPtr.Zero;
        }
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        mysql_stmt_close();
    }

    // ------------------------------------------------------------------
    //  Helper
    // ------------------------------------------------------------------

    private void ThrowStmtError() =>
        throw new MySqlStmtException(mysql_stmt_error(), mysql_stmt_errno(), mysql_stmt_sqlstate());

    private void ThrowIfDisposed()
    {
        if (_disposed) throw new ObjectDisposedException(nameof(MysqlStmt));
    }
}
